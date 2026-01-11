using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using RateLimiterWeb.Bussiness;
using RateLimiterWeb.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using RateLimiterWeb.Common;


namespace RateLimiterWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateController : ControllerBase
    {   
        private readonly IRLLogger logger;
        private readonly IRateValidator validator;
        public RateController(IRateValidator _validator,IRLLogger _logger) 
        {
            validator = _validator;
            logger = _logger;
        }

        /// <summary>
        /// Allows only specified number of request per given time
        /// </summary>
        /// <param name="Username"></param>
        /// <returns>HTTP Status 200 if request rate is valid else 429</returns>
        [HttpGet]
        public ActionResult ValidateRequestRate(string Username)
        {
            logger.DEBUG("Request Received in controller.");
            if (Username == null)
            {
                logger.DEBUG("Username is empty");
                return BadRequest("Please provide username");
            }

            if(validator.IsRequestAllowed(Username))
            {
                logger.DEBUG("Request allowed.");
                return Ok("This request was allowed.");                
            }
            else
            {
                logger.DEBUG("Request denied due to higher rate.");
                return StatusCode(StatusCodes.Status429TooManyRequests, "Too many requests. Please try again after some time.");
            }

        }

        
        [HttpPut]
        public ActionResult UpdateRequestRate([FromBody] RateParams rateParams)
        {
            logger.DEBUG("UpdateRequestRate api called.");
            if (rateParams == null)
            {
                logger.DEBUG("Invalid input");
                return BadRequest("invalid input - Please provide valid input");
            }

            if (validator.SetRequestRate(rateParams))
            {
                logger.INFO("Rate parameters updated successfully for "+rateParams.UserName);
                return Ok("Rate parameters updated successfully");
            }
            else {
                logger.ERROR("Could not update Rate parameters for "+rateParams.UserName);
                return StatusCode(StatusCodes.Status500InternalServerError,"Could not update rate parameters");
            }
        }
    }
}
