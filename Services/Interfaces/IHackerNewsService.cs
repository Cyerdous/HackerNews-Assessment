namespace HackerNews_Assessment.Services;

public interface IHackerNewsService
{
	Task<List<int>> GetNewStories();
}