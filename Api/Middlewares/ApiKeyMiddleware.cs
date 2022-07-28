using RecipesAPI.Api.Data;

namespace RecipesAPI.Api.Middlewares{

    public class ApiKeyMiddleware{
        private readonly RequestDelegate _next;
        private const string APIKEY = "XApiKey";

        public ApiKeyMiddleware(RequestDelegate next){
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IAppRepo repo){
            if(!context.Request.Headers.TryGetValue(APIKEY, out var extractedApiKey)){
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Api Key was not provided ");
                return;
            }
            
            if(await repo.ValidKeyAsync(extractedApiKey)){
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized client");
                return;
            }

            await _next(context);
        }
    }
}