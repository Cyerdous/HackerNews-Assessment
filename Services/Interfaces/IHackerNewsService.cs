namespace HackerNews_Assessment.Services;

public interface IHackerNewsService
{
	Task<List<NewsStory>> GetNewStories();
}