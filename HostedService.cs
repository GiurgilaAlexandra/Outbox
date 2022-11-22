using MassTransit;
using POC.Outbox.WebAPI.Models.DB;

namespace POC.Outbox.WebAPI
{
    public class HostedService : IHostedService
    {
        PocOutboxContext _context;
        private readonly IBus _publishEndpoint;
        public HostedService(PocOutboxContext context, IBus publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var timer = new System.Timers.Timer();
            // timer.Interval = 2_000;
            timer.Interval = 20_000;
            timer.Elapsed += OnElapsedHandler;
            timer.AutoReset = true;
            timer.Enabled = true;
            return Task.CompletedTask;
        }

        private async void OnElapsedHandler(object? sender, System.Timers.ElapsedEventArgs e)
        {
            var events = _context.Outboxes.OrderBy(o => o.WriteTime).ToList();
            foreach (var ev in events)
            {
                await _publishEndpoint.Publish(new Models.DB.Outbox { Id = ev.Id });
                Console.WriteLine("Published: " + ev.Data + " " + ev.WriteTime);
                _context.Outboxes.Remove(ev);
                await _context.SaveChangesAsync();
            }
            Console.WriteLine("Inside OnElapsedHandler");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
