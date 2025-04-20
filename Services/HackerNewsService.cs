using HackerNews_Assessment.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace HackerNews_Assessment.Services;

public class HackerNewsService : IHackerNewsService
{
	private readonly ILogger<HackerNewsService> _logger;
	private readonly IHackerNewsRepository _repository;
	private readonly IMemoryCache _cache;
	private readonly MemoryCacheEntryOptions _cacheOptions;
	const int itemsPerPage = 20; // Ideally this would be configurable either in the web frontend or config file, but a const will do for this implementation

	public HackerNewsService(ILogger<HackerNewsService> logger, IHackerNewsRepository repository, IMemoryCache cache)
    {
        _logger = logger;
		_repository = repository;
		_cache = cache;
		_cacheOptions = new() { SlidingExpiration = new TimeSpan(10 * TimeSpan.TicksPerMinute) };
    }

	// Nora: I had a lot of fun learning this particular method of parallelism.
	// However, I'm unsure how well this would scale.
	public async IAsyncEnumerable<NewsStory> GetNewStories(int page = 0)
	{
		var storyIds = (await _repository.ReadNewStories()).ToList();

		foreach(var id in storyIds.Skip(page*itemsPerPage).Take(itemsPerPage))
		{
			if (_cache.TryGetValue(id, out NewsStory story))
			{
				yield return story;
			}
			else
			{
				var item = await _repository.GetStoryById(id);
				
				yield return _cache.Set(id, new NewsStory()
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