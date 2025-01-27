using System.Text.Json.Serialization;

namespace DashboardSignalR.Models
{
    public sealed class StockPriceResponse()
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }
        [JsonPropertyName("msg")]
        public string Msg { get; set; }
        [JsonPropertyName("data")]
        public List<Datum> Data { get; set; }

    }


    public class Datum
    {
        public string instType { get; set; }
        public string instId { get; set; }
        public string last { get; set; }
        public string lastSz { get; set; }
        public string askPx { get; set; }
        public string askSz { get; set; }
        public string bidPx { get; set; }
        public string bidSz { get; set; }
        public string open24h { get; set; }
        public string high24h { get; set; }
        public string low24h { get; set; }
        public string volCcy24h { get; set; }
        public string vol24h { get; set; }
        public string ts { get; set; }
        public string sodUtc0 { get; set; }
        public string sodUtc8 { get; set; }
    }

}
