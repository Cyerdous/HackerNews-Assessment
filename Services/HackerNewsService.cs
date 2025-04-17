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

	public async Task<List<NewsStory>> GetNewStories()
	{
		List<NewsStory> stories = new();
		var storyIds = await _repository.ReadNewStories();
		await foreach(var item in _repository.GetStoriesById(storyIds))
		{
			if (item.Type != "story") continue;
			stories.Add(new NewsStory(){
				Id = item.Id,
				By = item.By,
				Time = item.Time,
				Text = item.Text,
				Url = item.Url,
				Title = item.Title
			});
		}
		return stories;
	}

	//When no id url is $"https://news.ycombinator.com/item?id={itemId}"

}