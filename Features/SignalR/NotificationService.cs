using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;

namespace Features.SignalR
{
    public class NotificationService : BackgroundService
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly TimeSpan _interval = TimeSpan.FromDays(1);

        public NotificationService(IHubContext<ChatHub> hubContext) { _hubContext = hubContext; }

        private TimeSpan GetTimeUntilMidnight()
        {
            var now = DateTime.Now;
            var nextMidnight = new DateTime(now.Year, now.Month, now.Day).AddDays(1); // 0h ngày hôm sau
            return nextMidnight - now;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Tính toán thời gian đến 0h ngày mới
                var timeUntilMidnight = GetTimeUntilMidnight();

                // Chờ đến 0h của ngày mới
                await Task.Delay(timeUntilMidnight, stoppingToken);

                // Gửi thông báo tới tất cả các client vào 0h
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", "Server", "Chúc mừng ngày mới!");

                // Sau khi đã gửi thông báo, đợi thêm 24 giờ (24h = 86400000 ms) trước khi gửi thông báo lần tiếp theo
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }
    }
}