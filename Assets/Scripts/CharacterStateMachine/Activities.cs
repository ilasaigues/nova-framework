using System;
using System.Threading;
using System.Threading.Tasks;
namespace HSM
{
    public enum ActivityStatus { Inactive, Activating, Active, Deactivating }

    public interface IActivity
    {
        ActivityStatus Status { get; }
        Task ActivateAsync(CancellationToken ct);
        Task DeactivateAsync(CancellationToken ct);
    }

    public class DelayActivationActivity : Activity
    {
        public float seconds = 0.2f;

        public override async Task ActivateAsync(CancellationToken ct)
        {
            await Task.Delay(TimeSpan.FromSeconds(seconds), ct);
            await base.ActivateAsync(ct);
        }
    }

    public abstract class Activity : IActivity
    {
        public ActivityStatus Status { get; protected set; } = ActivityStatus.Inactive;

        public virtual async Task ActivateAsync(CancellationToken ct)
        {
            if (Status != ActivityStatus.Inactive) return;
            Status = ActivityStatus.Activating;
            await Task.CompletedTask;
            Status = ActivityStatus.Active;
        }

        public virtual async Task DeactivateAsync(CancellationToken ct)
        {
            if (Status != ActivityStatus.Active) return;

            Status = ActivityStatus.Deactivating;
            await Task.CompletedTask;
            Status = ActivityStatus.Inactive;
        }
    }

}