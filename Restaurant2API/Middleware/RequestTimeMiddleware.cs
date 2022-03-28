using System.Diagnostics;

namespace Restaurant2API.Middleware
{
    public class RequestTimeMiddleware :IMiddleware
    {
        private readonly ILogger<RequestTimeMiddleware> _logger;
        private Stopwatch _stopwatch;

        public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger)
        {
            _logger = logger;
            _stopwatch = new Stopwatch();
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _stopwatch.Start();
            await next.Invoke(context);
            _stopwatch.Stop();

            var eclapsedMiliseconds = _stopwatch.ElapsedMilliseconds;

            if (eclapsedMiliseconds / 1000 > 4)
            {
                var message = $"Request [{context.Request.Method}] at {context.Request.Path} took {eclapsedMiliseconds} ms";
                _logger.LogInformation(message);
            }
        }
    }
}
