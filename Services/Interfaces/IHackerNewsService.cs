namespace HackerNews_Assessment.Services;

public interface IHackerNewsService
{
	IAsyncEnumerable<NewsStory> GetNewStories();
	Task<List<NewsStory>> GetNewsStoriesByPage(int page);
}