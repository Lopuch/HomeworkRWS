using UnreliableNotificationService = External.ThirdParty.Services.UnreliableNotificationService;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslationManagement.Application.Services;
using OneOf.Types;

namespace TranslationManagement.Application.BackgroundWorkers;
public class NotificationBackgroundWorker : BackgroundService
{
    readonly ILogger<NotificationBackgroundWorker> _logger;
    private readonly Queue<int> _jobQueue = new Queue<int>();
    private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
    private readonly UnreliableNotificationService _notificationSvc;

    private readonly INotificationService _notificationService;

    public NotificationBackgroundWorker(ILogger<NotificationBackgroundWorker> logger, INotificationService notificationService)
    {
        _logger = logger;
        _notificationSvc = new UnreliableNotificationService();
        _notificationService = notificationService;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(!stoppingToken.IsCancellationRequested)
        {
            var result = await _notificationService.GetJobId();

            if(result.Value is None)
            {
                await Task.Delay(1000, stoppingToken);
                continue;
            }

            var jobId = result.AsT0;

            SendNotification(jobId);
        }
    }

    private void SendNotification(int jobId)
    {
        while(true)
        {
            try
            {
                if(_notificationSvc.SendNotification("Job created: " + jobId).Result)
                {
                    _logger.LogInformation("New job notification sent");
                    return;
                }
            }
            catch { }
        }
    }
}
