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
	public async Task<List<int>> GetHackerNewsNewStories()
	{
		var client = new HttpClient();
		List<int> list = new();
		var json = await client.GetStringAsync(@"https://hacker-news.firebaseio.com/v0/newstories.json");


		list.AddRange( JsonSerializer.Deserialize<List<int>>(json) ?? Enumerable.Empty<int>() );
		
		return list;
	}
}
