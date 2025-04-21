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
		// So: If I were doing this on an actual project, I would be calling this method 
		// from a background service every hour or so.
		GetNewStoriesParallel();
    }

	// This whole thing sadly feels rather slow. At some point I may see if implementing this whole project
	// in Blazor is any faster, but that'll have to be when I'm not busy
	public async Task<int> GetStoryCount(string query = "")
	{
		return (await GetNewStories()).Where(story => story.Title.ToLower().Contains(query)).Count();
	}
	public async Task<List<NewsStory>> GetNewStories(int page, string query = "")
	{
		return (await GetNewStories()).Where(story => story.Title.ToLower().Contains(query)).Skip(page*itemsPerPage).Take(itemsPerPage).ToList();
	}

	// This method used to be making use of yield return, but ultimately I changed it to the Task style
	// because it simply wasn't performant. Fun thing is that this preserves order.
	private async Task<List<NewsStory>> GetNewStories()
	{
		var storyIds = (await _repository.ReadNewStories()).ToList();
		
		return (await Task.WhenAll(storyIds.Select(id => Task.Run(async () => 
			{
				if (_cache.Cache.TryGetValue(id, out NewsStory story))
				{
					return story;
				}
				else
				{
					var item = await _repository.GetStoryById(id);

					return _cache.Cache.Set(id, new NewsStory()
					{
						Id = item.Id,
						By = item.By,
						Time = item.Time,
						Url = string.IsNullOrWhiteSpace(item.Url) ? $"https://news.ycombinator.com/item?id={item.Id}" : item.Url,
						Title = item.Title
					}, _cacheOptions);
				}
			})))).ToList();		
	}

	// Cache doesn't care about the order items are added in, so I can do a parallel method to cache things ahead of time
	private async Task GetNewStoriesParallel()
	{
		var storyIds = (await _repository.ReadNewStories()).ToList();

		Parallel.ForEach(storyIds, async id => {
			if (!_cache.Cache.TryGetValue(id, out NewsStory story))
			{
				var item = await _repository.GetStoryById(id);

				_cache.Cache.Set(id, new NewsStory()
				{
					Id = item.Id,
					By = item.By,
					Time = item.Time,
					Url = string.IsNullOrWhiteSpace(item.Url) ? $"https://news.ycombinator.com/item?id={item.Id}" : item.Url,
					Title = item.Title
				}, _cacheOptions);
			}
		});
	}
}