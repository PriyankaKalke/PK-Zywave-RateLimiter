using Microsoft.AspNetCore.Mvc;
using Moq;
using RateLimiterWeb.Bussiness;
using RateLimiterWeb.Common;
using RateLimiterWeb.Controllers;
using RateLimiterWeb.Data;
using RateLimiterWeb.Models;
using System.Net;

namespace RateLimiterWeb.UnitTests
{
    [TestFixture]
    public class RateLimiterControllerTests
    {
        private Mock<IRateValidator> RateValidatorMock;
        private Mock<IRateParamsDL> RateParamsDLMock;
        private Mock<IRLLogger> LogerMock;
        private RateLimiterController controller;

        [SetUp]
        public void Setup()
        {   
            LogerMock = new Mock<IRLLogger>();
            RateValidatorMock = new Mock<IRateValidator>();// (LogerMock.Object,RateParamsDLMock.Object);
            controller= new RateLimiterController(RateValidatorMock.Object, LogerMock.Object);
            
            RateValidatorMock.Setup(x=>x.IsRequestAllowed(It.IsAny<string>())).Returns(true);
            RateValidatorMock.Setup(y=>y.SetRequestRate(It.IsAny<RateParams>())).Returns(true);
        }

        [Test]
        public void When_Request_is_Validated_Successfully_returns_OK()
        {
            RateValidatorMock.Setup(x => x.IsRequestAllowed(It.IsAny<string>())).Returns(true);
            
            IActionResult result = controller.ValidateRequestRate("User1");
            ObjectResult r= (ObjectResult)result;
            Assert.AreEqual(200, r.StatusCode);
        }
        [Test]
        public void When_Request_is_Rejected_returns_TooManyRequests()
        {
            RateValidatorMock.Setup(x => x.IsRequestAllowed(It.IsAny<string>())).Returns(false);
                
            IActionResult result = controller.ValidateRequestRate("User1");
            ObjectResult p = (ObjectResult)result;
            
            Assert.AreEqual(429, p.StatusCode);
        }

        public void When_RateParams_set_Successfully_returns_OK()
        {
            RateValidatorMock.Setup(y => y.SetRequestRate(It.IsAny<RateParams>())).Returns(true);
            RateParams record = new RateParams { UserName="user1",capacity=10,duration=10};

            IActionResult result = controller.UpdateRequestRate(record);
            ObjectResult r = (ObjectResult)result;
            Assert.AreEqual(200, r.StatusCode);
        }
        [Test]
        public void When_RateParams_Counld_not_be_returns_internalServerError()
        {
            RateValidatorMock.Setup(x => x.SetRequestRate(It.IsAny<RateParams>())).Returns(false);
            RateParams record = new RateParams { UserName = "user1", capacity = 10, duration = 10 };
            IActionResult result = controller.UpdateRequestRate(record);
            ObjectResult res = (ObjectResult)result;

            Assert.AreEqual(500, res.StatusCode);
        }
    }
}