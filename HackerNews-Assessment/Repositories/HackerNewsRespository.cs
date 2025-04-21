using System.Linq;
using System.Text.Json;

namespace HackerNews_Assessment.Repositories;

public class HackerNewsRepository : IHackerNewsRepository
{
	private readonly HttpClient _client;
	private readonly ILogger<HackerNewsRepository> _logger;
	public HackerNewsRepository(ILogger<HackerNewsRepository> logger, HttpClient client)
	{
		_client = client;
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
		var json = await _client.GetStringAsync(@$"https://hacker-news.firebaseio.com/v0/item/{id}.json");
		var options = new JsonSerializerOptions() {PropertyNameCaseInsensitive = true};
		return JsonSerializer.Deserialize<HackerNewsItem>(json,options) ?? new HackerNewsItem() {Id = -1};
	}
}