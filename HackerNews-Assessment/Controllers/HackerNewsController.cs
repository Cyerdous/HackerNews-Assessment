using HackerNews_Assessment.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HackerNews_Assessment.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class HackerNewsController : ControllerBase
{
    private readonly ILogger<HackerNewsController> _logger;
	private readonly IHackerNewsService _service;

    public HackerNewsController(ILogger<HackerNewsController> logger, IHackerNewsService service)
    {
        _logger = logger;
		_service = service;
    }
	
	//This one is going to get all the stories, which we probably don't want since it'll take ages.
	// [HttpGet]
	// public async Task<List<NewsStory>> GetNewStories()
	// {
	// 	return await _service.GetNewStories();
	// }

	[HttpGet("{page}/{query?}")]
	public async Task<List<NewsStory>> GetNewsByPage(int page, string? query = "")
	{
		return await _service.GetNewStories(page, query ?? "");
	}

	[HttpGet]
	[Route("{query?}")]
	public async Task<int> GetStoryCount(string? query = "")
	{
		return await _service.GetStoryCount(query ?? "");
	}
}