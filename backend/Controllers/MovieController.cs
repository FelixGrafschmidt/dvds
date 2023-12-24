namespace MovieAPI.Controllers;

using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using MovieAPI.Services;

[ApiController]
[Route("movies")]
public class MovieController : ControllerBase
{
	private readonly MovieService _movieService;

	public MovieController(MovieService movieService) =>
	_movieService = movieService;

	[HttpGet]
	[Route("/movies")]
	[Produces(MediaTypeNames.Application.Json)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetMovieList([FromQuery] string filter = "")
	{
		if (filter == "")
		{
			filter = "%%";
		}
		else
		{
			filter = "%" + filter + "%";
		}
		var result = await _movieService.GetMovieList(filter);
		if (result.Count == 0)
		{
			return NotFound();
		}
		else
		{
			return Ok(result);
		}
	}

	[HttpGet("{id:int}")]
	[Produces(MediaTypeNames.Application.Json)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetMovieDetails([FromRoute] int id)
	{
		var result = await _movieService.GetMovieDetails(id);
		if (result == null)
		{
			return NotFound();
		}
		else
		{
			return Ok(result);
		}
	}
}
