using External.ThirdParty.Services;
using OneOf;
using OneOf.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationManagement.Application.Services;

public interface INotificationService
{
    Task<OneOf<int, OneOf.Types.None>> GetJobId();
    Task AddNotificationToSend(int jobId);
}

public class NotificationService : INotificationService
{
    private readonly Queue<int> _jobQueue = new Queue<int>();
    private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
    private readonly UnreliableNotificationService _notificationSvc;

    public NotificationService()
    {
        _notificationSvc = new UnreliableNotificationService();
    }

    public async Task AddNotificationToSend(int jobId)
    {
        try
        {
            await _lock.WaitAsync();
            _jobQueue.Enqueue(jobId);
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<OneOf<int, None>> GetJobId()
    {
        try
        {
            await _lock.WaitAsync();

            return _jobQueue.TryDequeue(out var jobId) ?
                jobId :
                new None();
        }
        finally
        {
            _lock.Release();
        }
    }
}
