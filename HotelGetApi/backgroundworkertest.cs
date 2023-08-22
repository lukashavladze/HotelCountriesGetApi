using HotelGetApi.Controllers;
using System.ComponentModel;
using System.Threading;

namespace HotelGetApi
{
    public class backgroundworkertest : BackgroundService
    {
        public readonly ILogger<backgroundworkertest> _logger;
        public readonly HotelsController _controller;
        public backgroundworkertest(ILogger<backgroundworkertest> logger, HotelsController controller)
        {
            _logger = logger;
            _controller = controller;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("worker runs at :{time}", DateTimeOffset.Now);
                await _controller.Gethotels();
                await _controller.GetCountry();

                await Task.Delay(10000, stoppingToken);

            }
        }
    }
}
