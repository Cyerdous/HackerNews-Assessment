using Microsoft.Extensions.Caching.Memory;

public class HNMemoryCache
{
	public IMemoryCache Cache {get; } = new MemoryCache(
		new MemoryCacheOptions
		{
			SizeLimit = 500 //Length of new stories from HN api
		}
	);
}