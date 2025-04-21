using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Service.Abstractions;

namespace Presentation.Attributes
{
    public class CacheAttribute(int durationInSec) : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
          var CacheService =   context.HttpContext.RequestServices.GetRequiredService<IServiceManager>().chacheService;
            var cacheKey = GenenerateCacheKey(context.HttpContext.Request);

           var result = await  CacheService.GetCacheValueAsync(cacheKey);
            if(!string.IsNullOrEmpty(result))
            {
                // Return Response
                context.Result = new ContentResult()
                {
                    ContentType="application/json",
                    StatusCode = StatusCodes.Status200OK,
                    Content = result

                };
                return;
            }

            //Execute The EndPoint 
           var contextResult =  await next.Invoke();
            if(contextResult.Result is OkObjectResult okObject)
            {
               await CacheService.SetCacheValueAsync(cacheKey, okObject.Value, TimeSpan.FromSeconds(durationInSec));
            }
        }

        private string GenenerateCacheKey(HttpRequest request)
        {
            var Key = new StringBuilder();
            Key.Append(request.Path);
            foreach (var item in request.Query.OrderBy(q => q.Key))
            
            {
                Key.Append($"|{item.Key}-{item.Value}");
            
            }
            return Key.ToString();
        }
    }
}
