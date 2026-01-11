

using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RateLimiterWeb.Common;
using RateLimiterWeb.Data;
using RateLimiterWeb.Models;
using System.ComponentModel;


namespace RateLimiterWeb.Bussiness
{
    public class RateValidator : IRateValidator
    {
        private readonly IRateParamsDL rateParamsDL;
        private readonly IMemoryCache cache; 
        private readonly IRLLogger logger;
        public RateValidator(IMemoryCache _cache,IRateParamsDL _rateParamsDL,IRLLogger _logger) { 
            cache = _cache;
            rateParamsDL= _rateParamsDL;
            logger= _logger;
            InitializeCache();
        }

        /// <summary>
        /// Updates the rateparameters for existing User and creates new record for new user
        /// </summary>
        /// <param name="param"></param>
        /// <returns>true or false</returns>
        public bool SetRequestRate(RateParams param)
        {
            logger.DEBUG("In Set request rate function");
            try
            {
                logger.DEBUG("Checking if record already exist for "+param.UserName);
                if (rateParamsDL.GetRecordByUserName(param.UserName) == null)
                {
                    logger.DEBUG("Record does not exist for " + param.UserName);
                    if (rateParamsDL.AddRateRecord(param))
                    {
                        AddToCache(param);
                        return true;
                    }
                    logger.ERROR("Rate record could not be added for " + param.UserName);
                    return false;
                }
                else
                {
                    logger.DEBUG("Record alredy exist for " + param.UserName);
                    if (rateParamsDL.UpdateRateRecord(param))
                    {
                        AddToCache(param);
                        return true;
                    }
                    logger.ERROR("Rate record could not be updated for " + param.UserName);
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }          
        
        /// <summary>
        /// validates the request rate. Allows or blocks request as per set values
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>true or false</returns>
        public bool IsRequestAllowed(string userName)
        {
            logger.DEBUG("verifying if request can be forwarded or denied for user "+userName);
            var param= cache.Get(userName.ToLower());

            if (param!=null)
            {
                return ValidateRequest((RateParams)param);                
            }
            else 
            {
                var record = rateParamsDL.GetRecordByUserName(userName);
                if (record != null)
                {
                    AddToCache(record);
                    return ValidateRequest(record);
                }
            }
            return false;
        }
        private void InitializeCache()
        {
            logger.DEBUG("Adding existing users rateparameter data to cache.");
            var records = rateParamsDL.GetAllRateRecords();
            foreach (var record in records)
            {
                var cachekey = record.UserName;
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(1))
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(record.duration))
                .RegisterPostEvictionCallback((key, value, reason, state) =>
                {
                    cache.Remove(record.UserName);
                });

                cache.CreateEntry(cacheEntryOptions);
                cache.Set(cachekey, record, cacheEntryOptions);
            }
            logger.DEBUG("Data added to cache successfully.");
        }
        private void AddToCache(RateParams param)
        {
            logger.DEBUG("Adding record to cache  " + param.UserName);
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(1))
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(param.duration))
                    .RegisterPostEvictionCallback((key, value, reason, state) =>
                    {
                        cache.Remove(key);
                    });

            cache.CreateEntry(cacheEntryOptions);

            var cacheKey = param.UserName;

            cache.Set(cacheKey, param);
            logger.DEBUG("Record added to cache for" + param.UserName);

        }
        private bool ValidateRequest(RateParams? param)
        {
            if (param?.CurrentRequests <= param?.capacity)
            {
                logger.DEBUG("For user "+param.UserName+" currentrequest= "+param.CurrentRequests);
                param.CurrentRequests++;
                cache.Set(param.UserName, param);
                Console.WriteLine("from cache:" + param.CurrentRequests);
                return true;
            }            
            return false;
        }
    }
}
