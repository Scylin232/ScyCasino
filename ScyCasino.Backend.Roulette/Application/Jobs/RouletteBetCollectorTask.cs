using Application.CQRS.Roulette.Commands;
using MediatR;
using Microsoft.Extensions.Hosting;

namespace Application.Jobs;

// This is not the most suitable solution, as it causes problems with distributed systems.
// It is better to use a cloud scheduler or something more organized like Kubernetes CronJob.
// Implemented to simplify the example.

public class RouletteBetCollectorTask(ISender sender) : BackgroundService
{
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(1);
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        DateTime currentTime = DateTime.UtcNow;
        DateTime nextFullMinute = currentTime.AddMinutes(1).AddSeconds(-currentTime.Second);
        TimeSpan waitTime = nextFullMinute - currentTime;
        
        await Task.Delay(waitTime, stoppingToken);
        
        using PeriodicTimer timer = new PeriodicTimer(_interval);
        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            CollectRouletteBetsCommand command = new();
            await sender.Send(command, stoppingToken);
        }
    }
}