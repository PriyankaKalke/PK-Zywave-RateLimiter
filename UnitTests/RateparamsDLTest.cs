using Moq;
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
    internal class RateparamsDLTest
    {
        private Mock<IRLLogger> LogerMock;
        private RateParamsDL rateparamsDL;

        [SetUp]
        public void Setup()
        {
            LogerMock = new Mock<IRLLogger>();
            rateparamsDL= new RateParamsDL(LogerMock.Object);
        }
        [Test]
        public void RateParamsDL_Test1()
        {
            var result=rateparamsDL.GetRecordByUserName("user1");
            Assert.IsNotNull(result);
            Assert.AreEqual(result.UserName, "user1");

        }
        [Test]
        public void RateParamsDL_Test2()
        {
            var result = rateparamsDL.GetRecordByUserName("user4");
            Assert.IsNull(result);           

        }
        [Test]
        public void RateParamsDL_Test3()
        {
            var result = rateparamsDL.GetAllRateRecords();
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count,3);

        }
        [Test]
        public void RateParamsDL_Test4()
        {
            var rateRecord = new RateParams { capacity = 10, duration = 10, UserName = "user4", CurrentRequests = 0 };
            var result = rateparamsDL.AddRateRecord(rateRecord);
            Assert.IsTrue(result);            

        }

        [Test]
        public void RateParamsDL_Test5()
        {
            var rateRecord = new RateParams { capacity = 10, duration = 20, UserName = "user1", CurrentRequests = 0 };
            var result = rateparamsDL.UpdateRateRecord(rateRecord);
            Assert.IsTrue(result);

        }
        [Test]
        public void RateParamsDL_Test6()
        {
            var rateRecord = new RateParams { capacity = 10, duration = 10, UserName = "user5", CurrentRequests = 0 };
            var result = rateparamsDL.UpdateRateRecord(rateRecord);
            Assert.IsFalse(result);

        }
    }
}
