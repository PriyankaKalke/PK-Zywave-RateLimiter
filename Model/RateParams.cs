namespace RateLimiterWeb.Models
{
    public class RateParams
    {
        public string UserName { get; set; }
        public int capacity{ get; set;}
        public int CurrentRequests { get; set;}
        public int duration{get;set; }
    }
}
