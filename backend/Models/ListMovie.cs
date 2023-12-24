using System.Text.Json.Serialization;

namespace MovieAPI.Models;

public class ListMovie
{
	[JsonPropertyName("id")]
	public int ID { get; set; } = -1;

	[JsonPropertyName("title")]
	public string Title { get; set; } = "";

	[JsonPropertyName("releaseYear")]
	public int ReleaseYear { get; set; } = -1;

	[JsonPropertyName("language")]
	public string Language { get; set; } = "";
}
