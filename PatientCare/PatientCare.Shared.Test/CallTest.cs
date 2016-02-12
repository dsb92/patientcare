using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using PatientCare.Shared.Managers;
using PatientCare.Shared.Interfaces;
using PatientCare.Shared.Model;
using PatientCare.Shared.Util;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace PatientCare.Shared.Test
{
    [TestClass]
    public class CallTest
    {
        private const string CPRNUMMER = "1111111118";

        [TestMethod]
        public void SerializeCallJSON_JSONConverted()
        {
            // Arrange
            var testString = "Test";
            CallEntity call = new CallEntity
            {
                _id = Guid.NewGuid().ToString(),
                Category = "Mad/Drikke",
                Choice = "Cola",
                CreatedOn = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff"),
                Detail = "Sukker",
                PatientCPR = Guid.NewGuid().ToString(),
                Status = (int)CallUtil.StatusCode.Active
            };


            //Act
            string json = JsonConvert.SerializeObject(call);

            //Assert
            Assert.IsTrue(ReferenceEquals(json.GetType(), testString.GetType()));
        }

        [TestMethod]
        public void SendingCallJSONCategory_JSONConverted()
        {
            try
            {
                // Arrange
                var testString = "Test";
                CategoryEntity categoryEntity = new CategoryEntity
                {
                    CategoryId = Guid.NewGuid().ToString(),
                    Name = "TestTestTest"
                };
                CallEntity callEntity = CallWrapper.WrapCall(CPRNUMMER, CallUtil.StatusCode.Active, categoryEntity, null,
                    null);

                //Act
                PatientCall call = new PatientCall();

                var callId = call.MakeCall(callEntity);

                Assert.IsTrue(!String.IsNullOrEmpty(callId));
            }
            catch (Exception ex)
            {
                //Assert
                Assert.Fail("Status Code not OK " + ex.Message);
            }
        }

        [TestMethod]
        public void SendingCallJSONCategoryChoice_JSONConverted()
        {
            // Arrange
            var testString = "Test";

            var choice = new ChoiceEntity()
            {
                ChoiceId = Guid.NewGuid().ToString(),
                Name = "Morfin"
            };

            var category = new CategoryEntity()
            {
                CategoryId = Guid.NewGuid().ToString(),
                Name = "Smertestillende",
                //Picture = "http://multimedia.pol.dk/archive/00537/ITALY_CLONED_CHAMPI_537998a.jpg",
                Choices = new List<ChoiceEntity> { choice }
            };

            CallEntity callEntity = CallWrapper.WrapCall(CPRNUMMER, CallUtil.StatusCode.Active, category, choice, null);

            try
            {
                //Act
                PatientCall call = new PatientCall();
                var callId = call.MakeCall(callEntity);

                Assert.IsTrue(!String.IsNullOrEmpty(callId));
            }
            catch (Exception ex)
            {
                // Assert
                Assert.Fail("Status Code not OK " + ex.Message);
            }
        }

        [TestMethod]
        public void SendingCallJSONCategoryChoiceDetail_JSONConverted()
        {
            // Arrange
            var testString = "Test";

            var detail = new DetailEntity()
            {
                DetailId = Guid.NewGuid().ToString(),
                Name = "Mælk",
            };

            var choice = new ChoiceEntity()
            {
                ChoiceId = Guid.NewGuid().ToString(),
                Name = "Kaffe",
                Details = new List<DetailEntity> { detail }
            };

            var category = new CategoryEntity()
            {
                CategoryId = Guid.NewGuid().ToString(),
                Name = "Dinmor",
                //Picture = "http://multimedia.pol.dk/archive/00537/ITALY_CLONED_CHAMPI_537998a.jpg",
                Choices = new List<ChoiceEntity> { choice }
            };

            CallEntity callEntity = CallWrapper.WrapCall(CPRNUMMER, CallUtil.StatusCode.Active, category, choice, detail);

            try
            {
                //Act
                PatientCall call = new PatientCall();
                var callId = call.MakeCall(callEntity);

                Assert.IsTrue(!String.IsNullOrEmpty(callId));
            }
            catch (Exception ex)
            {
                Assert.Fail("Status Code not OK " + ex.Message);
            }
        }

        [TestMethod]
        public void UpdatingCallJSON_CalledUpdated()
        {
            //Arrange
            // Arrange
            var testString = "Test";

            var choice = new ChoiceEntity()
            {
                ChoiceId = Guid.NewGuid().ToString(),
                Name = "Morfin"
            };

            var category = new CategoryEntity()
            {
                CategoryId = Guid.NewGuid().ToString(),
                Name = "Smertestillende",
                //Picture = "http://multimedia.pol.dk/archive/00537/ITALY_CLONED_CHAMPI_537998a.jpg",
                Choices = new List<ChoiceEntity> { choice }
            };

            CallEntity callEntity = CallWrapper.WrapCall(CPRNUMMER, CallUtil.StatusCode.Active, category, choice, null);

            //Act
            PatientCall call = new PatientCall();

            try
            {
                callEntity._id = "5641c5dd02a93d27a8910f9c";
                callEntity.Status = (int) CallUtil.StatusCode.Canceled;
                call.UpdateCall(callEntity);
            }
            catch (Exception e)
            {
                // Assert
                Assert.Fail("No calls has been updated");
            }
        }

        [TestMethod]
        public void RetreivingCallJSON_JSONConverted()
        {
            // Arrange
            var testString = "Test";
            var httpHandler = new HttpHandler();

            // Act
            var callJson = httpHandler.GetData(HttpHandler.API.Call, "5638662c4ca8e92f7cf1fcc7");

            var json = JsonConvert.DeserializeObject(callJson);

            Dictionary<string, string> jsonDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json as string);

            var callStatus = jsonDictionary["Status"];

            // Assert
            Assert.IsTrue(ReferenceEquals(callJson.GetType(), testString.GetType()));
            Assert.IsTrue(callStatus != null);
        }
    }
}
