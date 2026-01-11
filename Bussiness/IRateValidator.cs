using RateLimiterWeb.Models;

namespace RateLimiterWeb.Bussiness
{
    public interface IRateValidator
    {
        bool IsRequestAllowed(string Username);
        bool SetRequestRate(RateParams param);
    }
}