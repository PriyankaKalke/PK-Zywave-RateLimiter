using Microsoft.Extensions.Caching.Memory;
using Moq;
using RateLimiterWeb.Bussiness;
using RateLimiterWeb.Common;
using RateLimiterWeb.Data;
using RateLimiterWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateLimiterWeb.UnitTests
{
    [TestFixture]
    internal class RateValidatorTests
    {
        private Mock<IRateParamsDL> RateParamsDLMock;
        private Mock<IRLLogger> LogerMock;
        private Mock<IMemoryCache> MemoryCacheMock;
        private RateValidator rateValidator;

        public List<RateParams> RequestRateData = new List<RateParams>
        {
            new RateParams{ UserName="user1",capacity=5,duration=5,CurrentRequests=0} ,
            new RateParams{ UserName="user2",capacity=5,duration=10,CurrentRequests=0} ,
            new RateParams{ UserName="user3",capacity=5,duration=15,CurrentRequests=0}
        };

        [SetUp]
        public void Setup()
        {
            LogerMock = new Mock<IRLLogger>();
            RateParamsDLMock = new Mock<IRateParamsDL>();
            MemoryCacheMock= new Mock<IMemoryCache>();
            RateParamsDLMock.Setup(y => y.GetAllRateRecords()).Returns(RequestRateData);
            rateValidator = new RateValidator(MemoryCacheMock.Object,RateParamsDLMock.Object, LogerMock.Object);
                         
        }
        [Test]
        public void RateValidator_Test1()
        {
            var rateRecord = new RateParams { capacity = 10, duration = 10, UserName = "user1",CurrentRequests=0 };
            RateParamsDLMock.Setup(x => x.GetRecordByUserName(It.IsAny<string>())).Returns(rateRecord);
            RateParamsDLMock.Setup(y => y.GetAllRateRecords()).Returns(RequestRateData);
            MemoryCacheMock.Setup(y=>y.Get(It.IsAny<string>())).Returns(null);
            var result =rateValidator.IsRequestAllowed("UserName");
            Assert.IsTrue(result);

        }
        [Test]
        public void Ratevalidator_Test2()
        {
            var rateRecord = new RateParams { capacity = 10, duration = 10, UserName = "user1", CurrentRequests = 0 };
            RateParamsDLMock.Setup(x => x.GetRecordByUserName(It.IsAny<string>())).Returns(rateRecord);
            RateParamsDLMock.Setup(y => y.GetAllRateRecords()).Returns(RequestRateData);
            MemoryCacheMock.Setup(y => y.Get(It.IsAny<string>())).Returns(rateRecord);
            var result = rateValidator.IsRequestAllowed("UserName");
            Assert.IsTrue(result);

        }
        [Test]
        public void Ratevalidator_Test3()
        {
            var rateRecord = new RateParams { capacity = 10, duration = 10, UserName = "user1", CurrentRequests = 11 };
            RateParamsDLMock.Setup(x => x.GetRecordByUserName(It.IsAny<string>())).Returns(rateRecord);
            RateParamsDLMock.Setup(y => y.GetAllRateRecords()).Returns(RequestRateData);
            MemoryCacheMock.Setup(y => y.Get(It.IsAny<string>())).Returns(rateRecord);
            var result = rateValidator.IsRequestAllowed("UserName");
            Assert.IsFalse(result);

        }
        [Test]
        public void Ratevalidator_Test4()
        {
            RateParams rateRecord = null;// new RateParams { capacity = 10, duration = 10, UserName = "user1", CurrentRequests = 11 };
            RateParamsDLMock.Setup(x => x.GetRecordByUserName(It.IsAny<string>())).Returns(rateRecord);
            RateParamsDLMock.Setup(y => y.GetAllRateRecords()).Returns(RequestRateData);
            MemoryCacheMock.Setup(y => y.Get(It.IsAny<string>())).Returns(null);
            var result = rateValidator.IsRequestAllowed("UserName");
            Assert.IsFalse(result);

        }

    }
}
