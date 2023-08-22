
using System.ComponentModel;
using System.Threading;

namespace HotelGetApi
{
    public class backgroundworkertest : BackgroundService
    {
        public readonly ILogger<backgroundworkertest> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public backgroundworkertest(ILogger<backgroundworkertest> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var dataService = scope.ServiceProvider.GetRequiredService<IHotelDataService>();
                    await dataService.ProcessHotelDataAsync();
                }
                _logger.LogInformation("worker runs at :{time}", DateTimeOffset.Now);
                

                await Task.Delay(30000, stoppingToken);

            }
        }
    }
}
