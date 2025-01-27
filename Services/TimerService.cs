using DashboardSignalR.Helpers;
using DashboardSignalR.Hubs;
using DashboardSignalR.Models;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace DashboardSignalR.Services
{
    public class TimerService : IHostedService, IAsyncDisposable
    {
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private Task _completedTask = Task.CompletedTask;
        private int _executionCount = 0;
        private Timer? _timer;
        private IHubContext<StockHub> _hubContext;

        public TimerService(ILogger<TimerService> logger, IHttpClientFactory httpClient, IHubContext<StockHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
            _httpClientFactory = httpClient;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Service} is running.", nameof(Services));
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(3));

            return _completedTask;
        }
        private async void DoWork(object? state)
        {
            int count = Interlocked.Increment(ref _executionCount);
            _logger.LogInformation(
                "{Service} is working, execution count: {Count:#,0}",
                nameof(Services),
                count);

            //get price
            StockPrice? stockPriceBtc = await GetStockPrice("BTC-USD-SWAP");
            StockPrice? stockPriceEth = await GetStockPrice("ETH-USD-SWAP");
            StockPrice? stockPriceSol = await GetStockPrice("SOL-USD-SWAP");

            var data = DataGenerator.GenerateRandomDashboardData();

            data.CardBtc = new()
            {
                Price = stockPriceBtc.Price ?? "",
                Hight = stockPriceBtc.Hight,
                Low = stockPriceBtc.Low
            };
            data.CardEth = new()
            {
                Price = stockPriceEth.Price ?? "",
                Hight = stockPriceEth.Hight,
                Low = stockPriceEth.Low
            };
            data.CardSol = new()
            {
                Price = stockPriceSol.Price ?? "",
                Hight = stockPriceSol.Hight,
                Low = stockPriceSol.Low
            };

            await _hubContext.Clients.All.SendAsync("Receive", data);
        }

        private async Task<StockPrice?> GetStockPrice(string stockName)
        {
            using var client = _httpClientFactory.CreateClient("TimerService");
            try
            {
                using var response = await client.GetAsync($"?instId={stockName}");

                if (!response.IsSuccessStatusCode) return null;

                var responseRe = await response.Content.ReadAsStringAsync();
                StockPriceResponse? stockData = JsonSerializer.Deserialize<StockPriceResponse>(responseRe);
                string? price = stockData?.Data.First().last;

                if (price is null) return null;
                long ts = long.TryParse(stockData?.Data.First().ts, out long temp) ? temp : 0;
                DateTime date = DateTimeOffset.FromUnixTimeMilliseconds(ts).DateTime;

                _logger.LogInformation("Completed getting stock price information for API STOCK, {@stockName}", stockName);
                return new StockPrice()
                {
                    StockName = stockName,
                    Price = price,
                    Hight = stockData?.Data?.First().high24h ?? "",
                    Low = stockData?.Data?.First().low24h ?? "",
                    Date = date
                };
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"HTTP request error: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
            }
            _logger.LogWarning("Failed to get stock price information for {API STOCK}", stockName);
            return null;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(
           "{Service} is stopping.", nameof(Services));

            _timer?.Change(Timeout.Infinite, 0);

            return _completedTask;
        }

        public async ValueTask DisposeAsync()
        {
            if (_timer is IAsyncDisposable timer)
            {
                await timer.DisposeAsync();
            }

            _timer = null;
        }
    }
}
