using System.Text.Json;

namespace HackerNews_Assessment.Repositories;

public class HackerNewsRepository : IHackerNewsRepository
{
	private readonly HttpClient _client;
	private readonly ILogger<HackerNewsRepository> _logger;
	public HackerNewsRepository(ILogger<HackerNewsRepository> logger)
	{
		_client = new HttpClient();
		_logger = logger;
	}

	public async Task<List<int>> HackerNews_ReadNewStories()
	{
		List<int> list = new();

		var json = await _client.GetStringAsync(@"https://hacker-news.firebaseio.com/v0/newstories.json");
		list.AddRange( JsonSerializer.Deserialize<List<int>>(json) ?? Enumerable.Empty<int>() );

		return list;
	}

//Might be able to get away with a yield return
	public async Task HackerNews_GetStoryById()
	{
		throw new NotImplementedException();
	}
}