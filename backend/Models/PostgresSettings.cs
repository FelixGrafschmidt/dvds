namespace MovieAPI.Models;

public class PostgresSettings
{
	public const string Postgres = "Postgres";
	public string Port { get; set; } = "";
	public string Host { get; set; } = "";
	public string Username { get; set; } = "";
	public string Password { get; set; } = "";
}
