namespace HackerNews_Assessment.Services;

public interface IHackerNewsService
{
	IAsyncEnumerable<NewsStory> GetNewStories(int page = 0);
}