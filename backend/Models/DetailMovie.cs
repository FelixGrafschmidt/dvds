using System.Text.Json.Serialization;

namespace MovieAPI.Models;

public class DetailMovie
{
	[JsonPropertyName("id")]
	public int ID { get; set; } = -1;

	[JsonPropertyName("title")]
	public string Title { get; set; } = "";

	[JsonPropertyName("description")]
	public string Description { get; set; } = "";

	[JsonPropertyName("actors")]
	public string Actors { get; set; } = "";
}
