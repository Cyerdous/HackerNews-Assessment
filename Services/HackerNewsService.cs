using HackerNews_Assessment.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace HackerNews_Assessment.Services;

public class HackerNewsService : IHackerNewsService
{
	private readonly ILogger<HackerNewsService> _logger;
	private readonly IHackerNewsRepository _repository;
	private readonly IMemoryCache _cache;
	private readonly MemoryCacheEntryOptions _cacheOptions;
	const int itemsPerPage = 30;

	public HackerNewsService(ILogger<HackerNewsService> logger, IHackerNewsRepository repository, IMemoryCache cache)
    {
        _logger = logger;
		_repository = repository;
		_cache = cache;
		_cacheOptions = new() { SlidingExpiration = new TimeSpan(10 * TimeSpan.TicksPerMinute) };
    }

	// Nora: I had a lot of fun learning this particular method of parallelism.
	// However, I'm unsure how well this would scale.
	public async IAsyncEnumerable<NewsStory> GetNewStories()
	{
		var storyIds = (await _repository.ReadNewStories()).ToList();

		foreach(var id in storyIds)
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

	public async Task<List<NewsStory>> GetNewsStoriesByPage(int page)
	{
		return await GetNewStories().Skip(page*itemsPerPage).Take(itemsPerPage).ToListAsync();
		
		throw new NotImplementedException(); //TODO after cache
	}
}