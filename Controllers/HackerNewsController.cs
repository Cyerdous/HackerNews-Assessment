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
	
	[HttpGet]
	public async Task<List<NewsStory>> GetNewStories()
	{
		return await _service.GetNewStories();
	}
}
