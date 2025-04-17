using System.Linq;
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

	public async Task<List<int>> ReadNewStories()
	{
		List<int> list = new();

		var json = await _client.GetStringAsync(@"https://hacker-news.firebaseio.com/v0/newstories.json");
		list.AddRange( JsonSerializer.Deserialize<List<int>>(json) ?? Enumerable.Empty<int>() );

		return list;
	}

	public async Task<HackerNewsItem> GetStoryById(int id)
	{
		return await _client.GetFromJsonAsync<HackerNewsItem>(@$"https://hacker-news.firebaseio.com/v0/item/{id}.json") ?? new HackerNewsItem { Id = -1 };
	}

	public async IAsyncEnumerable<HackerNewsItem> GetStoriesById(List<int> ids)
	{
		foreach(var id in ids)
		{
			yield return await GetStoryById(id);
		}
	}
}