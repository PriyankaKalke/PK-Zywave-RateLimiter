using RateLimiterWeb.Models;

namespace RateLimiterWeb.Data
{
    public interface IRateParamsDL
    {
        List<RateParams> GetAllRateRecords();
        RateParams GetRecordByUserName(string UserName);
        bool AddRateRecord(RateParams rateParams);
        bool UpdateRateRecord(RateParams rateParams); 
    }
}