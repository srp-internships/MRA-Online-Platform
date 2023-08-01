using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Core.Filters.Behaviours
{
    public class PerformanceBehaviour<TRequest, TResponce> : IPipelineBehavior<TRequest, TResponce> where TRequest : IRequest<TResponce>
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;

        public PerformanceBehaviour(ILogger<TRequest> logger)
        {
            _logger = logger;
            _timer = new Stopwatch();
        }
        public async Task<TResponce> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponce> next)
        {
            _timer.Start();
            var response = await next();
            _timer.Stop();
            var elapsedMilliseconds = _timer.ElapsedMilliseconds;
            if (elapsedMilliseconds > 1000)
            {
                var requestName = typeof(TRequest).Name;
                _logger.LogWarning($"Long Running Request: {requestName}, millisecond: {elapsedMilliseconds}");
            }
            return response;
        }
    }
}
