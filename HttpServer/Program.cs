using Common;
using Common.Utils;
using HttpServer.Controllers;
using Microsoft.Extensions.FileProviders;

namespace HttpServer
{
    public class Program
    {
        private static readonly Logger c = new("HTTP", ConsoleColor.Green);

        public static void Main()
        {
            Thread.CurrentThread.IsBackground = true;
            var builder = WebApplication.CreateBuilder();

            var app = builder.Build();

            app.UsePathBase("/");
            app.Urls.Add($"http://*:{Global.config.Http.HttpPort}");
            app.Urls.Add($"https://*:{Global.config.Http.HttpsPort}");

            DispatchController.AddHandlers(app);
            AccountController.AddHandlers(app);
            ConfigController.AddHandlers(app);

            app.UseMiddleware<RequestLoggingMiddleware>();
            c.Log($"HTTP 服务器已在 {Global.config.Http.HttpPort} & {Global.config.Http.HttpsPort} 上启动"); // A lie
            app.Run();
        }

        private class RequestLoggingMiddleware
        {
            private readonly RequestDelegate _next;
            private static readonly string[] SurpressedRoutes = new string[] { "/report", "/sdk/dataUpload" };

            public RequestLoggingMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public async Task Invoke(HttpContext context)
            {
                try
                {
                    await _next(context);
                }
                finally
                {
                    if ((int)Global.config.VerboseLevel > (int)VerboseLevel.Normal)
                    {
#if DEBUG
                        c.Log($"{context.Response.StatusCode} {context.Request.Method.ToUpper()} {context.Request.Path}");
#endif
                    }
                    else if(((int)Global.config.VerboseLevel > (int)VerboseLevel.Silent) && !SurpressedRoutes.Contains(context.Request.Path.ToString()))
                    {
#if DEBUG
                        c.Log($"{context.Response.StatusCode} {context.Request.Method.ToUpper()} {context.Request.Path}");
#endif
                    }
                }
            }
        }
    }
}