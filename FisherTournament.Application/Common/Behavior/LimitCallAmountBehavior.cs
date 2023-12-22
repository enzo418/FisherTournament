using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FisherTournament.Application.Common.Behavior;

// Basic sliding window rate limiter because I don't want to go broke
public class LimitCallAmountBehavior<TRequest, TResponse>
 : IPipelineBehavior<TRequest, TResponse>
    where TResponse : IErrorOr
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LimitCallAmountBehavior<TRequest, TResponse>> _logger;

    private readonly object _lock = new object();
    private readonly Queue<DateTime> _callTimes;
    private readonly TimeSpan _windowInterval;
    private readonly int _maxCalls = 3;

    public LimitCallAmountBehavior(ILogger<LimitCallAmountBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
        _callTimes = new Queue<DateTime>();
        _windowInterval = TimeSpan.FromSeconds(1);
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        lock (_lock)
        {
            CleanExpiredCalls();
            if (_callTimes.Count < _maxCalls)
            {
                _callTimes.Enqueue(DateTime.Now);
            } else
            {
                _logger.LogError("Error: Too many calls");

                var response = Error.Failure().ConvertToErrorOr<TResponse>();

                if (response is not null)
                {
                    return (TResponse)response;
                } else
                {
                    throw new Exception("Too many calls");
                }
            }
        }

        return await next();
    }

    private void CleanExpiredCalls()
    {
        var currentTime = DateTime.Now;
        while (_callTimes.Count > 0 && currentTime - _callTimes.Peek() > _windowInterval)
        {
            _callTimes.Dequeue();
        }
    }

}