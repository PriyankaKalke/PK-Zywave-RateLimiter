using RateLimiterWeb.Common;
using RateLimiterWeb.Models;

namespace RateLimiterWeb.Data
{
    public class RateParamsDL : IRateParamsDL
    {
        private readonly IRLLogger logger;
        public static List<RateParams> RequestRateData = new List<RateParams>
        {
            new RateParams{ UserName="user1",capacity=5,duration=5,CurrentRequests=0} ,
            new RateParams{ UserName="user2",capacity=5,duration=10,CurrentRequests=0} ,
            new RateParams{ UserName="user3",capacity=5,duration=15,CurrentRequests=0}
        };
        public RateParamsDL(IRLLogger _logger) {
            logger = _logger;
        }

       /// <summary>
       /// Get the rate parameter record for given user
       /// </summary>
       /// <param name="UserName"></param>
       /// <returns>single rate parameter record null if dosent exists</returns>       
        public RateParams GetRecordByUserName(string UserName)
        {
            var User = RequestRateData.Where(x => x.UserName.Equals(UserName.ToLower())).ToList().FirstOrDefault();
            if (User != null)
            {
                return User;
            }
            return null;
        }

        /// <summary>
        /// Get  rate parameters for all users
        /// </summary>
        /// <returns>list of rateparameter object</returns>
        public List<RateParams> GetAllRateRecords()
        {
            return RequestRateData.ToList();
        }
        /// <summary>
        /// Add new record to the rateparameter records
        /// </summary>
        /// <param name="rateParams"></param>
        /// <returns>returns true or false</returns>
        public bool AddRateRecord(RateParams rateParams)
        {
            logger.DEBUG("Tryng to add new record for " + rateParams.UserName);
            var record = new RateParams()
            {
                UserName = rateParams.UserName.ToLower(),
                capacity = rateParams.capacity,
                duration = rateParams.duration
            };
            RequestRateData.Add(record);
            return true;
        }
        /// <summary>
        /// Updates rateparameters for the given user
        /// </summary>
        /// <param name="rateParams"></param>
        /// <returns>true or false</returns>
        public bool UpdateRateRecord(RateParams rateParams)
        {
            logger.DEBUG("Updating record for " + rateParams.UserName);
            var User = RequestRateData.Where(x => x.UserName.Equals(rateParams.UserName.ToLower())).ToList().FirstOrDefault();
            if (User != null)
            {
                RequestRateData.RemoveAll(x => x.UserName.Equals(rateParams.UserName.ToLower()));
                return true;
            }            
            return false;
        }

    }
}
