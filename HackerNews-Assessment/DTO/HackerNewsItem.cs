namespace HackerNews_Assessment;
public class HackerNewsItem
{
	public int Id { get; set; }
	public bool Deleted { get; set; }
	public string Type { get; set; } = "";
	public string By { get; set; } = "";
	public int Time { get; set; }
	public string Text { get; set; } = "";
	public bool Dead { get; set; }
	public List<int> Kids { get; set; } = new();
	public string Url { get; set; } = "";
	public int Score { get; set; }
	public string Title { get; set; } = "";
	public List<int> Parts { get; set; } = new();
	public int Descendants { get; set; }
	
}