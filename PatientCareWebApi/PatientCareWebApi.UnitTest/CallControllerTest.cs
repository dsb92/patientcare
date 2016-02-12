using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.VisualStudio.QualityTools.UnitTestFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using Moq;
using PatientCareWebApi.Controllers;
using PatientCareWebApi.DomainModels;
using PatientCareWebApi.Models;
using PatientCareWebApi.Repository;
using PatientCareWebApi.Repository.Interfaces;
using PatientCareWebApi.Repository.Repositories;


namespace PatientCareWebApi.UnitTest
{
    [TestClass]
    public class CallControllerTest
    {
        [TestMethod]
        public void CallController_GetAllActiveCalls_ContainsActiveCalls_ReturnsAllActiveCalls()
        {
            //Arrange
            Mock<ICallRepository> mock = new Mock<ICallRepository>();
            mock.Setup(m => m.GetAll()).Returns(new List<BsonDocument>()
            {
               new CallModel {_id = ObjectId.GenerateNewId().ToString(), PatientName = "Test", PatientCPR = "123456-1234", Status = 0}.ToBsonDocument(),
               new CallModel {_id = ObjectId.GenerateNewId().ToString(), PatientName = "Test2", PatientCPR = "654321-4321", Status = 0}.ToBsonDocument(),
               new CallModel {_id = ObjectId.GenerateNewId().ToString(), PatientName = "Test3", PatientCPR = "132465-1324", Status = 0}.ToBsonDocument()
            });
            //Act
            var target = new CallController(mock.Object);

            var result = target.GetAllActiveCalls();
            //Assert

            Assert.AreEqual(typeof(string), result.GetType()); //API'et returner en string med alle kald
            Assert.AreEqual(result.Contains("123456-1234"), true);
            Assert.AreEqual(result.Contains("Test3"), true);
        }

        [TestMethod]
        public void CallController_GetCallById_ContainsACall_ReturnsSpecificCall()
        {
            Mock<ICallRepository> mock = new Mock<ICallRepository>();
            var id = ObjectId.GenerateNewId().ToString();
            mock.Setup(m => m.Get(It.IsAny<string>())).Returns(new BsonDocument()
            {
                new CallModel
                {
                    _id = id,
                    PatientName = "Test",
                    PatientCPR = "123456-1234",
                    Status = 0
                }.ToBsonDocument()
            });

            var CUT = new CallController(mock.Object);

            var result = CUT.GetCallById(id);

            Assert.IsTrue(result.Contains(id));
            Assert.AreEqual(typeof(string), result.GetType());
        }

        [TestMethod]
        public void CallController_CreateNewCall_ReturnsActionResultWithCallIdInDescription()
        {
            Mock<ICallRepository> mock = new Mock<ICallRepository>();
            var CUT = new CallController(mock.Object);

            var patient = new Patient()
            {
                _id = ObjectId.GenerateNewId().ToString(),
                ImportantInfo = "Denne skal ikke bruges alligevel :<",
                PatientCPR = "654321-4321",
                PatientName = "PATester"
            };


            var newCall = new CreateCallModel()
            {
                PatientCPR = "654321-4321",
                Category = "TestCategory",
                Choice = "TestChoice",
                CreatedOn = DateTime.Now.ToString("F"),
                Detail = "TestDetail"
            };

            var callId = CUT.CreateCall(newCall);

            Assert.IsNotNull(callId);
        }
    }
}