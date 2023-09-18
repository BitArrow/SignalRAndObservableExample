using Microsoft.AspNetCore.SignalR;
using SignalRTest.Blazor.Server.Hubs;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace SignalRTest.Blazor.Server.Data
{
    public class TimeService
    {
        private readonly Timer _timer;
        private readonly Timer _timer2;

        private readonly IHubContext<ClockHub> _clockHub;

        private static readonly BehaviorSubject<string?> _timeChange = new (null);
        private static readonly BehaviorSubject<string?> _utcTimeChange = new(null);

        public BehaviorSubject<string?> CurrentTime { get; set; } = _timeChange;
        public BehaviorSubject<string?> CurrentUtcTime { get; set; } = _utcTimeChange;

        public TimeService(IHubContext<ClockHub> clockHub)
        {
            _clockHub = clockHub;
            _timer = new Timer(TimeTick, null, 250, 250);
            _timer2 = new Timer(TimeUtcTick, null, 1500, 1500);

            CurrentTime.Subscribe(UpdateHubTime);
            CurrentUtcTime.Subscribe(UpdateUtcHubTime);

            var combined = Observable.CombineLatest(
                CurrentTime,
                CurrentUtcTime,
                (ct, cut) => new TimeModel()
                {
                    CurrentTime = ct,
                    UtcTime = cut
                }
            );

            combined.Subscribe(UpdateCombinedTime);
        }

        private void TimeTick(object? state)
        {
            _timeChange.OnNext(DateTime.Now.ToString("O"));
        }

        private void TimeUtcTick(object? state)
        {
            _utcTimeChange.OnNext(DateTime.UtcNow.ToString("O"));
        }

        private async void UpdateHubTime(string? time)
        {
            await _clockHub.Clients.All.SendAsync("ReceiveTime", time);
        }

        private async void UpdateUtcHubTime(string? utcTime)
        {
            await _clockHub.Clients.All.SendAsync("ReceiveUtcTime", utcTime);
        }

        private async void UpdateCombinedTime(TimeModel? combined)
        {
            await _clockHub.Clients.All.SendAsync("ReceiveCombinedTime", combined);
        }
    }

    public class TimeModel
    {
        public string? CurrentTime { get; set; }

        public string? UtcTime { get; set; }
    }
}
