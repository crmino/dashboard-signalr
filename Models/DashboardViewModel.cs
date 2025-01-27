namespace DashboardSignalR.Models
{
    public class DashboardViewModel
    {
        public List<List<object>> columnChart { get; set; }
        public LineChart lineChart { get; set; }
        public List<int> circleChart { get; set; }
        public int progress1 { get; set; }
        public int progress2 { get; set; }
        public int progress3 { get; set; }
        public Card CardBtc { get; set; }
        public Card CardEth { get; set; }
        public Card CardSol { get; set; }
    }

    public class LineChart
    {
        public List<List<object>> running { get; set; }
        public List<List<object>> waiting { get; set; }
    }

    public class Card
    {
        public string Price { get; set; }
        public string? Hight { get; set; }
        public string? Low { get; set; }
    }
}
