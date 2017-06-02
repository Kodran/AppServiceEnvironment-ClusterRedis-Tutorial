using MyAPI.Demo.Models;
using StackExchange.Redis;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace MyAPI.Demo.Controllers
{
    public class RedisController : ApiController
    {
        private static string redisClusterConnection = "My StackExchange.Redis primary connection string";
        public static IDatabase redisCache = ConnectionMultiplexer.Connect(redisClusterConnection).GetDatabase();

        [SwaggerOperation("GetByKey")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public async Task<IHttpActionResult> Get(string key)
        {
            var result = string.Empty;
            try
            {
                result = await redisCache.StringGetAsync(key);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Ok(result);
        }

        [SwaggerOperation("Create")]
        [SwaggerResponse(HttpStatusCode.Created)]
        public async Task<IHttpActionResult> Post([FromBody]CreateRequest request)
        {
            var result = false;
            try
            {
                result =  await redisCache.StringSetAsync(request.key, request.value);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Ok(result);
        }
    }
}
