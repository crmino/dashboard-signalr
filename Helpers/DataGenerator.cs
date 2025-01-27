using DashboardSignalR.Models;

namespace DashboardSignalR.Helpers
{
    public class DataGenerator
    {
        private static Random random = new Random();

        public static DashboardViewModel GenerateRandomDashboardData()
        {
            return new DashboardViewModel
            {
                columnChart = GenerateRandomColumnChartData(),
                lineChart = new LineChart
                {
                    running = GenerateRandomLineChartData(),
                    waiting = GenerateRandomLineChartData()
                },
                circleChart = GenerateRandomCircleChartData(),
                progress1 = random.Next(0, 101),
                progress2 = random.Next(0, 101),
                progress3 = random.Next(0, 101)
            };
        }

        private static List<List<object>> GenerateRandomColumnChartData()
        {
            // Obtener la zona horaria de Santiago de Chile
            var baseTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("America/Santiago")).AddMinutes(-30);

            var data = new List<List<object>>();
            for (int i = 0; i < 12; i++)
            {
                data.Add(new List<object>
            {
                baseTime.AddMinutes(i * 5).ToString("o"), // ISO 8601 timestamp
                random.Next(10, 110) // Valor aleatorio
            });
            }
            return data;
        }

        private static List<List<object>> GenerateRandomLineChartData()
        {
            // Obtener la zona horaria de Santiago de Chile
            var baseTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("America/Santiago")).AddMinutes(-30);
            var data = new List<List<object>>();
            for (int i = 0; i < 12; i++)
            {
                data.Add(new List<object>
            {
                baseTime.AddMinutes(i * 5).ToString("o"), // ISO 8601 timestamp
                random.Next(30, 110) // Valor aleatorio
            });
            }
            return data;
        }

        private static List<int> GenerateRandomCircleChartData()
        {
            return new List<int>
        {
            random.Next(0, 101),
            random.Next(0, 101)
        };
        }
    }
}
