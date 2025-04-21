namespace HackerNews_Assessment.Services;

public interface IHackerNewsService
{
	Task<List<NewsStory>> GetNewStories(int page, string query = "");
	Task<int> GetStoryCount(string query = "");
}