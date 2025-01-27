namespace DashboardSignalR.Models
{
    public class StockPrice
    {
        required public string StockName { get; set; }
        required public string Price { get; set; }
        required public string? Hight { get; set; }
        required public string? Low { get; set; }
        required public DateTime Date { get; set; }
    }
}
