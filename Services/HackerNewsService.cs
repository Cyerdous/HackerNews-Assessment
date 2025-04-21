using System.Timers;
using HackerNews_Assessment.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace HackerNews_Assessment.Services;

public class HackerNewsService : IHackerNewsService
{
	private readonly ILogger<HackerNewsService> _logger;
	private readonly IHackerNewsRepository _repository;
	private readonly HNMemoryCache _cache;
	private readonly MemoryCacheEntryOptions _cacheOptions;
	const int itemsPerPage = 20; // Ideally this would be configurable either in the web frontend or config file, but a const will do for this implementation

	public HackerNewsService(ILogger<HackerNewsService> logger, IHackerNewsRepository repository, HNMemoryCache cache)
    {
        _logger = logger;
		_repository = repository;
		_cache = cache;
		_cacheOptions = new() { Size = 1 }; // Limit is 500

		// I know for a fact that this is bad practice, since this service is scoped re DI
		// So: If I were doing this on an actual project, I would be calling a modified version 
		// of this method (that returns on a cache hit) from a background service every hour or so.
		GetNewStories();
    }

	public async Task<int> GetStoryCount(string query = "")
	{
		return await GetNewStories().Where(story => story.Title.ToLower().Contains(query)).CountAsync();
	}
	public async Task<List<NewsStory>> GetNewStories(int page, string query = "")
	{
		return await GetNewStories().Where(story => story.Title.ToLower().Contains(query)).Skip(page*itemsPerPage).Take(itemsPerPage).ToListAsync();
	}

	// I had a lot of fun learning this particular method of parallelism.
	// However, I'm unsure how well this scales.
	private async IAsyncEnumerable<NewsStory> GetNewStories()
	{
		var storyIds = (await _repository.ReadNewStories()).ToList();

		foreach(var id in storyIds)
		{
			if (_cache.Cache.TryGetValue(id, out NewsStory story))
			{
				yield return story;
			}
			else
			{
				var item = await _repository.GetStoryById(id);
				
				yield return _cache.Cache.Set(id, new NewsStory()
				{
					Id = item.Id,
					By = item.By,
					Time = item.Time,
					Url = string.IsNullOrWhiteSpace(item.Url) ? $"https://news.ycombinator.com/item?id={item.Id}" : item.Url,
					Title = item.Title
				}, _cacheOptions);
			}
		}
	}
}