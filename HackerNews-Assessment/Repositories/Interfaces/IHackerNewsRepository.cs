namespace HackerNews_Assessment.Repositories;

public interface IHackerNewsRepository
{
	Task<List<int>> ReadNewStories();
	Task<HackerNewsItem> GetStoryById(int id);
}