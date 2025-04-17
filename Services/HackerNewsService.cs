using HackerNews_Assessment.Repositories;

namespace HackerNews_Assessment.Services;

public class HackerNewsService : IHackerNewsService
{
	private readonly ILogger<HackerNewsService> _logger;
	private readonly IHackerNewsRepository _repository;

	public HackerNewsService(ILogger<HackerNewsService> logger, IHackerNewsRepository repository)
    {
        _logger = logger;
		_repository = repository;
    }

	public async Task<List<int>> GetNewStories()
	{
		return await _repository.ReadNewStories();
	}

	//When no id url is $"https://news.ycombinator.com/item?id={itemId}"

}