namespace MovieAPI.Services;

using Microsoft.Extensions.Options;
using Npgsql;
using MovieAPI.Models;

public class MovieService
{

	private readonly NpgsqlDataSource dataSource;

	public MovieService(IOptions<PostgresSettings> postgresSettings)
	{
		var connectionString = string.Format("Host={0}:{1};Username={2};Password={3};Database=dvdrental", postgresSettings.Value.Host, postgresSettings.Value.Port, postgresSettings.Value.Username, postgresSettings.Value.Password);
		try
		{
			dataSource = NpgsqlDataSource.Create(connectionString);
		}
		catch (FormatException)
		{
			Console.WriteLine("Invalid connectionString, aborting...");
			Console.WriteLine(connectionString);
			throw;
		}
	}

	public async Task<List<ListMovie>> GetMovieList(string filter, int limit, int offset)
	{
		var conn = await dataSource.OpenConnectionAsync();
		await using var cmd = new NpgsqlCommand("SELECT films.film_id as id, films.title, films.release_year, name as language FROM film as films INNER JOIN language as languages ON films.language_id = languages.language_id WHERE films.title LIKE ($1) LIMIT ($2) OFFSET ($3)", conn);
		cmd.Parameters.AddWithValue(filter);
		cmd.Parameters.AddWithValue(limit);
		cmd.Parameters.AddWithValue(offset);
		var reader = await cmd.ExecuteReaderAsync();
		var result = new List<ListMovie>();
		while (await reader.ReadAsync())
		{
			var entry = new ListMovie
			{
				ID = (int)reader["id"],
				Title = (string)reader["title"],
				ReleaseYear = (int)reader["release_year"],
				Language = (string)reader["language"]
			};
			result.Add(entry);
		}
		await conn.CloseAsync();
		return result;
	}

	public async Task<DetailMovie?> GetMovieDetails(int id)
	{
		var conn = await dataSource.OpenConnectionAsync();
		await using var cmd = new NpgsqlCommand("SELECT films.fid, films.title, films.description, films.actors FROM film_list as films WHERE films.fid = ($1) LIMIT 1", conn);
		cmd.Parameters.AddWithValue(id);
		var reader = await cmd.ExecuteReaderAsync();

		if (await reader.ReadAsync())
		{
			var entry = new DetailMovie
			{
				ID = (int)reader["fid"],
				Title = (string)reader["title"],
				Description = (string)reader["description"],
				Actors = (string)reader["actors"]
			};
			await conn.CloseAsync();
			return entry;
		}
		else
		{
			await conn.CloseAsync();
			return null;
		}
	}
}
