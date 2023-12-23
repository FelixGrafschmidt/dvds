using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "wildcard",
                      policy =>
                      {
                          policy.WithOrigins("*");
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Read config for postgres
var postgresConfig =
    builder.Configuration.GetSection("Postgres").Get<PostgresSettings>() ?? throw new Exception("Missing Postgres configuration");

var connectionString = string.Format("Host={0}:{1};Username={2};Password={3};Database=dvdrental", postgresConfig.Host, postgresConfig.Port, postgresConfig.Username, postgresConfig.Password);

await using var dataSource = NpgsqlDataSource.Create(connectionString);

app.UseHttpsRedirection();
app.UseCors("wildcard");

app.MapGet("/movies", async (context) =>
{
    var filter = context.Request.Query["filter"].ToString().ToLower();
    if (filter == "")
    {
        filter = "%%";
    }
    else
    {
        filter = "%" + filter + "%";
    }
    var conn = await dataSource.OpenConnectionAsync();
    await using var cmd = new NpgsqlCommand("SELECT films.film_id as id, films.title, films.release_year, name as language FROM film as films INNER JOIN language as languages ON films.language_id = languages.language_id WHERE films.title LIKE ($1)", conn);
    cmd.Parameters.AddWithValue(filter);
    var reader = await cmd.ExecuteReaderAsync();
    var result = new List<ListMovie>();
    while (await reader.ReadAsync())
    {
        var entry = new ListMovie
        {
            id = (int)reader["id"],
            title = (string)reader["title"],
            releaseYear = (int)reader["release_year"],
            language = (string)reader["language"]
        };
        result.Add(entry);
    }
    await conn.CloseAsync();
    await context.Response.WriteAsJsonAsync(result);
})
.WithName("Movies")
.WithOpenApi();

app.MapGet("/movie-details/{id:int}", async (context) =>
{
    var id = context.Request.RouteValues["id"];
    if (id == null)
    {
        context.Response.StatusCode = 401;
        await context.Response.CompleteAsync();
    }
    else
    {
        var conn = await dataSource.OpenConnectionAsync();
        await using var cmd = new NpgsqlCommand("SELECT films.fid, films.title, films.description, films.actors FROM film_list as films WHERE films.fid = ($1) LIMIT 1", conn);

        cmd.Parameters.AddWithValue(int.Parse((string)id));
        var reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            var entry = new DetailMovie
            {
                id = (int)reader["fid"],
                title = (string)reader["title"],
                description = (string)reader["description"],
                actors = (string)reader["actors"]
            };
            await conn.CloseAsync();
            await context.Response.WriteAsJsonAsync(entry);
        }
        else
        {
            await conn.CloseAsync();
            context.Response.StatusCode = 404;
            await context.Response.CompleteAsync();
        }
    }
})
.WithName("Movie-Details")
.WithOpenApi();

app.Run();
