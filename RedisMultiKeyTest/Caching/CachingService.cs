using Microsoft.Extensions.Caching.Distributed;
using NRedisStack.RedisStackCommands;
using NRedisStack;
using StackExchange.Redis;
using NRedisStack.Search.Literals.Enums;
using NRedisStack.Search;

namespace RedisMultiKeyTest.Caching
{
    /*
     To access Redis Stack capabilities, you should use appropriate interface like this:
    IBloomCommands bf = db.BF();
    ICuckooCommands cf = db.CF();
    ICmsCommands cms = db.CMS();
    IGraphCommands graph = db.GRAPH();
    ITopKCommands topk = db.TOPK();
    ITdigestCommands tdigest = db.TDIGEST();
    ISearchCommands ft = db.FT();
    IJsonCommands json = db.JSON();
    ITimeSeriesCommands ts = db.TS();
     */

    public class CachingService : ICachingService
    {
        private readonly IDistributedCache _cache;
        private readonly DistributedCacheEntryOptions _options;

        public CachingService(IDistributedCache cache)
        {
            _cache = cache;
            _options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(1_200),
                SlidingExpiration = TimeSpan.FromSeconds(600),
            };
        }
        public async Task<string> GetAsync(string key)
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379");

            IDatabase db = redis.GetDatabase();
            SearchCommands ft = db.FT();

            var res = ft.Search("idx:users", new Query("Paul @Age:[30 40]")).Documents.Select(x => x["json"]);
            //Console.WriteLine(string.Join("\n", res));
            
            return string.Join("\n", res);
        }

        public async Task SetAsync(string key, string value)
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379");

            IDatabase db = redis.GetDatabase();
            SearchCommands ft = db.FT();
            JsonCommands json = db.JSON();

            var user1 = new User("Paul John", "paul.john@example.com", 42, "London");
            var user2 = new User("Eden Zamir", "eden.zamir@example.com", 29, "Tel Aviv");
            var user3 = new User("Paul Zamir", "paul.zamir@example.com", 35, "Tel Aviv");

            //Create an index. In this example, all JSON documents with the key prefix user: are indexed.
            //For more information, see Query syntax.

            var schema = new Schema()
                .AddTextField(new FieldName("$.Name", "Name"))
                .AddTagField(new FieldName("$.City", "City"))
                .AddNumericField(new FieldName("$.Age", "Age"));

            ft.Create("idx:users",
                    new FTCreateParams().On(IndexDataType.JSON).Prefix("user:"),
                    schema);

            json.Set("user:1", "$", user1);
            json.Set("user:2", "$", user2);
            json.Set("user:3", "$", user3);
        }
    }
}
