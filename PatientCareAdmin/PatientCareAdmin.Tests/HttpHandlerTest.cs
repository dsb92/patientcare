using System;
using System.Collections.Generic;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using PatientCareAdmin.Models;

namespace PatientCareAdmin.Tests
{
    [TestClass]
    public class HttpHandlerTest
    {
        [TestMethod]
        public void TestPostAsync()
        {
            var client = new HttpClient();

            var httpHandler = new HttpHandler<CallModel>(client);
            httpHandler.Uri = "api/call";

            var newCall = new CallModel()
            {
                PatientCPR = "123456-1234",
                Category = "Hvad er en constructor?",
                Choice = "Urhauer",
                Detail = "Din mor",
                CreatedOn = DateTime.Now.ToString("F")
            };

            var json = JsonConvert.SerializeObject(newCall);

            var result = httpHandler.Post(json);

            Assert.AreEqual(201,result.StatusCode);
        }

        [TestMethod]
        public void ResponseMessageDescription_DescriptionContainsStatusCode_ReturnsStatusCode201()
        {
            var message = new ResponseMessage()
            {
                StatusCode =  201,
                StatusDescription = "Contains Statuscode : 201"
            };

            var separator = message.StatusDescription.IndexOf(":");

            var test = message.StatusDescription.Substring(1 + separator + 1);

            Assert.AreEqual("201", test);
        }
        
        [TestMethod]
        public void TestGetAsync()
        {
            var client = new HttpClient();

            var httpHandler = new HttpHandler<CallModel>(client);
            httpHandler.Uri = "api/call";

            var returnedFromRequest = httpHandler.Get();

            Assert.IsNotNull(returnedFromRequest);
        }

        [TestMethod]
        public void HttpHandlerGet_GetSingleCallById_ReturnsSingleCallModel()
        {
            var client = new HttpClient();
            
            var httpHandler = new HttpHandler<CallModel>(client);
            httpHandler.Uri = "api/call";

            var singleCall = httpHandler.Get("563c930402a93d01d0d8506c");

            Assert.AreEqual(singleCall.GetType(), typeof(CallModel));
        }

        [TestMethod]
        public void DeserializeJsonFromWebAPI_IsSuccessful()
        {
            var jsonString = "[{ \"_id\" : \"5630e607a7149f1964950a71\", \"PatientCPR\" : \"123456-1234\", \"PatientName\" : \"Alice Maja\", \"Room\" : \"Room 42\", \"Bed\" : \"1\", \"Department\" : \"Gyn-obs\", \"Category\" : \"Mad/Drikke\", \"Choice\" : \"Kaffe\", \"Detail\" : null, \"CreatedOn\" : null, \"ModifiedOn\" : null, \"Status\" : 0 }, { \"_id\" : \"5630ee48a7149f0f50e18ced\", \"PatientCPR\" : \"123456-1234\", \"PatientName\" : \"Alice Maja\", \"Room\" : \"Room 42\", \"Bed\" : \"1\", \"Department\" : \"Gyn-obs\", \"Category\" : \"Mad/Drikke\", \"Choice\" : \"Kaffe\", \"Detail\" : null, \"CreatedOn\" : null, \"ModifiedOn\" : null, \"Status\" : 0 }, { \"_id\" : \"5630ef294ca8ec268405d8a3\", \"PatientCPR\" : \"123456-1234\", \"PatientName\" : \"Alice Maja\", \"Room\" : \"Room 42\", \"Bed\" : \"1\", \"Department\" : \"Gyn-obs\", \"Category\" : \"TestTestTest\", \"Choice\" : null, \"Detail\" : null, \"CreatedOn\" : \"onsdag, 28 oktober 15.27.31\", \"ModifiedOn\" : null, \"Status\" : 0 }, { \"_id\" : \"5630f15c4ca8ec268405d8a7\", \"PatientCPR\" : \"123456-1234\", \"PatientName\" : \"Alice Maja\", \"Room\" : \"Room 42\", \"Bed\" : \"1\", \"Department\" : \"Gyn-obs\", \"Category\" : \"TestTestTest\", \"Choice\" : null, \"Detail\" : null, \"CreatedOn\" : \"onsdag, 28 oktober 15.27.31\", \"ModifiedOn\" : null, \"Status\" : 0 }, { \"_id\" : \"5630f3524ca8ec268405d8a9\", \"PatientCPR\" : \"123456-1234\", \"PatientName\" : \"Alice Maja\", \"Room\" : \"Room 42\", \"Bed\" : \"1\", \"Department\" : \"Gyn-obs\", \"Category\" : \"TestTestTest\", \"Choice\" : null, \"Detail\" : null, \"CreatedOn\" : \"onsdag, 28 oktober 17.09.32\", \"ModifiedOn\" : null, \"Status\" : 0 }, { \"_id\" : \"5631004a4ca8e9290cddd46b\", \"PatientCPR\" : \"123456-1234\", \"PatientName\" : \"Alice Maja\", \"Room\" : \"Room 42\", \"Bed\" : \"1\", \"Department\" : \"Gyn-obs\", \"Category\" : \"TestTestTest\", \"Choice\" : null, \"Detail\" : null, \"CreatedOn\" : \"onsdag, 28 oktober 18.05.05\", \"ModifiedOn\" : null, \"Status\" : 0 }, { \"_id\" : \"5631011c4ca8e9290cddd46d\", \"PatientCPR\" : \"123456-1234\", \"PatientName\" : \"Alice Maja\", \"Room\" : \"Room 42\", \"Bed\" : \"1\", \"Department\" : \"Gyn-obs\", \"Category\" : \"TestTestTest\", \"Choice\" : null, \"Detail\" : null, \"CreatedOn\" : \"onsdag, 28 oktober 18.08.44\", \"ModifiedOn\" : null, \"Status\" : 0 }, { \"_id\" : \"563109d44ca8e926087dfc42\", \"PatientCPR\" : \"123456-1234\", \"PatientName\" : \"Alice Maja\", \"Room\" : \"Room 42\", \"Bed\" : \"1\", \"Department\" : \"Gyn-obs\", \"Category\" : \"Mad/Drikke\", \"Choice\" : \"Kaffe\", \"Detail\" : \"Sort kaffe\", \"CreatedOn\" : \"onsdag, 28 oktober 18:45:52\", \"ModifiedOn\" : null, \"Status\" : 0 }, { \"_id\" : \"563239d04ca8e90684a4b4b9\", \"PatientCPR\" : \"123456-1234\", \"PatientName\" : \"Alice Maja\", \"Room\" : \"Room 42\", \"Bed\" : \"1\", \"Department\" : \"Gyn-obs\", \"Category\" : \"Toilet\", \"Choice\" : null, \"Detail\" : null, \"CreatedOn\" : \"torsdag, 29 oktober 16:22:52\", \"ModifiedOn\" : null, \"Status\" : 0 }, { \"_id\" : \"563239fc4ca8e90684a4b4bb\", \"PatientCPR\" : \"123456-1234\", \"PatientName\" : \"Alice Maja\", \"Room\" : \"Room 42\", \"Bed\" : \"1\", \"Department\" : \"Gyn-obs\", \"Category\" : \"Smertestillende\", \"Choice\" : \"Morfin\", \"Detail\" : null, \"CreatedOn\" : \"torsdag, 29 oktober 16:23:40\", \"ModifiedOn\" : null, \"Status\" : 0 }, { \"_id\" : \"563110394ca8e92fecbc7245\", \"PatientCPR\" : \"123456-1234\", \"PatientName\" : \"Alice Maja\", \"Room\" : \"Room 42\", \"Bed\" : \"1\", \"Department\" : \"Gyn-obs\", \"Category\" : \"Toilet\", \"Choice\" : null, \"Detail\" : null, \"CreatedOn\" : \"onsdag, 28 oktober 19:13:10\", \"ModifiedOn\" : null, \"Status\" : 0 }, { \"_id\" : \"563111754ca8e92fecbc7247\", \"PatientCPR\" : \"123456-1234\", \"PatientName\" : \"Alice Maja\", \"Room\" : \"Room 42\", \"Bed\" : \"1\", \"Department\" : \"Gyn-obs\", \"Category\" : \"Smertestillende\", \"Choice\" : \"Panodil\", \"Detail\" : null, \"CreatedOn\" : \"onsdag, 28 oktober 19:18:28\", \"ModifiedOn\" : null, \"Status\" : 0 }, { \"_id\" : \"563117824ca8e92aa4704f73\", \"PatientCPR\" : \"123456-1234\", \"PatientName\" : \"Alice Maja\", \"Room\" : \"Room 42\", \"Bed\" : \"1\", \"Department\" : \"Gyn-obs\", \"Category\" : \"Toilet\", \"Choice\" : null, \"Detail\" : null, \"CreatedOn\" : \"onsdag, 28 oktober 19:44:15\", \"ModifiedOn\" : null, \"Status\" : 0 }, { \"_id\" : \"56323a124ca8e90684a4b4bd\", \"PatientCPR\" : \"123456-1234\", \"PatientName\" : \"Alice Maja\", \"Room\" : \"Room 42\", \"Bed\" : \"1\", \"Department\" : \"Gyn-obs\", \"Category\" : \"Mad/Drikke\", \"Choice\" : \"Kaffe\", \"Detail\" : \"Mælk\", \"CreatedOn\" : \"torsdag, 29 oktober 16:24:02\", \"ModifiedOn\" : null, \"Status\" : 0 }]";

            var json = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CallModel>>(jsonString);

            Assert.IsNotNull(json);
        }

        [TestMethod]
        public void DeserializeJsonFromWebAPI_IsSuccessful2()
        {
            var jsonString = "\"[{ \\\"_id\\\" : \\\"5630e607a7149f1964950a71\\\", \\\"PatientCPR\\\" : \\\"123456-1234\\\", \\\"PatientName\\\" : \\\"Alice Maja\\\", \\\"Room\\\" : \\\"Room 42\\\", \\\"Bed\\\" : \\\"1\\\", \\\"Department\\\" : \\\"Gyn-obs\\\", \\\"Category\\\" : \\\"Mad/Drikke\\\", \\\"Choice\\\" : \\\"Kaffe\\\", \\\"Detail\\\" : null, \\\"CreatedOn\\\" : null, \\\"ModifiedOn\\\" : null, \\\"Status\\\" : 0 }, { \\\"_id\\\" : \\\"5630ee48a7149f0f50e18ced\\\", \\\"PatientCPR\\\" : \\\"123456-1234\\\", \\\"PatientName\\\" : \\\"Alice Maja\\\", \\\"Room\\\" : \\\"Room 42\\\", \\\"Bed\\\" : \\\"1\\\", \\\"Department\\\" : \\\"Gyn-obs\\\", \\\"Category\\\" : \\\"Mad/Drikke\\\", \\\"Choice\\\" : \\\"Kaffe\\\", \\\"Detail\\\" : null, \\\"CreatedOn\\\" : null, \\\"ModifiedOn\\\" : null, \\\"Status\\\" : 0 }, { \\\"_id\\\" : \\\"5630ef294ca8ec268405d8a3\\\", \\\"PatientCPR\\\" : \\\"123456-1234\\\", \\\"PatientName\\\" : \\\"Alice Maja\\\", \\\"Room\\\" : \\\"Room 42\\\", \\\"Bed\\\" : \\\"1\\\", \\\"Department\\\" : \\\"Gyn-obs\\\", \\\"Category\\\" : \\\"TestTestTest\\\", \\\"Choice\\\" : null, \\\"Detail\\\" : null, \\\"CreatedOn\\\" : \\\"onsdag, 28 oktober 15.27.31\\\", \\\"ModifiedOn\\\" : null, \\\"Status\\\" : 0 }, { \\\"_id\\\" : \\\"5630f15c4ca8ec268405d8a7\\\", \\\"PatientCPR\\\" : \\\"123456-1234\\\", \\\"PatientName\\\" : \\\"Alice Maja\\\", \\\"Room\\\" : \\\"Room 42\\\", \\\"Bed\\\" : \\\"1\\\", \\\"Department\\\" : \\\"Gyn-obs\\\", \\\"Category\\\" : \\\"TestTestTest\\\", \\\"Choice\\\" : null, \\\"Detail\\\" : null, \\\"CreatedOn\\\" : \\\"onsdag, 28 oktober 15.27.31\\\", \\\"ModifiedOn\\\" : null, \\\"Status\\\" : 0 }, { \\\"_id\\\" : \\\"5630f3524ca8ec268405d8a9\\\", \\\"PatientCPR\\\" : \\\"123456-1234\\\", \\\"PatientName\\\" : \\\"Alice Maja\\\", \\\"Room\\\" : \\\"Room 42\\\", \\\"Bed\\\" : \\\"1\\\", \\\"Department\\\" : \\\"Gyn-obs\\\", \\\"Category\\\" : \\\"TestTestTest\\\", \\\"Choice\\\" : null, \\\"Detail\\\" : null, \\\"CreatedOn\\\" : \\\"onsdag, 28 oktober 17.09.32\\\", \\\"ModifiedOn\\\" : null, \\\"Status\\\" : 0 }, { \\\"_id\\\" : \\\"5631004a4ca8e9290cddd46b\\\", \\\"PatientCPR\\\" : \\\"123456-1234\\\", \\\"PatientName\\\" : \\\"Alice Maja\\\", \\\"Room\\\" : \\\"Room 42\\\", \\\"Bed\\\" : \\\"1\\\", \\\"Department\\\" : \\\"Gyn-obs\\\", \\\"Category\\\" : \\\"TestTestTest\\\", \\\"Choice\\\" : null, \\\"Detail\\\" : null, \\\"CreatedOn\\\" : \\\"onsdag, 28 oktober 18.05.05\\\", \\\"ModifiedOn\\\" : null, \\\"Status\\\" : 0 }, { \\\"_id\\\" : \\\"5631011c4ca8e9290cddd46d\\\", \\\"PatientCPR\\\" : \\\"123456-1234\\\", \\\"PatientName\\\" : \\\"Alice Maja\\\", \\\"Room\\\" : \\\"Room 42\\\", \\\"Bed\\\" : \\\"1\\\", \\\"Department\\\" : \\\"Gyn-obs\\\", \\\"Category\\\" : \\\"TestTestTest\\\", \\\"Choice\\\" : null, \\\"Detail\\\" : null, \\\"CreatedOn\\\" : \\\"onsdag, 28 oktober 18.08.44\\\", \\\"ModifiedOn\\\" : null, \\\"Status\\\" : 0 }, { \\\"_id\\\" : \\\"563109d44ca8e926087dfc42\\\", \\\"PatientCPR\\\" : \\\"123456-1234\\\", \\\"PatientName\\\" : \\\"Alice Maja\\\", \\\"Room\\\" : \\\"Room 42\\\", \\\"Bed\\\" : \\\"1\\\", \\\"Department\\\" : \\\"Gyn-obs\\\", \\\"Category\\\" : \\\"Mad/Drikke\\\", \\\"Choice\\\" : \\\"Kaffe\\\", \\\"Detail\\\" : \\\"Sort kaffe\\\", \\\"CreatedOn\\\" : \\\"onsdag, 28 oktober 18:45:52\\\", \\\"ModifiedOn\\\" : null, \\\"Status\\\" : 0 }, { \\\"_id\\\" : \\\"563239d04ca8e90684a4b4b9\\\", \\\"PatientCPR\\\" : \\\"123456-1234\\\", \\\"PatientName\\\" : \\\"Alice Maja\\\", \\\"Room\\\" : \\\"Room 42\\\", \\\"Bed\\\" : \\\"1\\\", \\\"Department\\\" : \\\"Gyn-obs\\\", \\\"Category\\\" : \\\"Toilet\\\", \\\"Choice\\\" : null, \\\"Detail\\\" : null, \\\"CreatedOn\\\" : \\\"torsdag, 29 oktober 16:22:52\\\", \\\"ModifiedOn\\\" : null, \\\"Status\\\" : 0 }, { \\\"_id\\\" : \\\"563239fc4ca8e90684a4b4bb\\\", \\\"PatientCPR\\\" : \\\"123456-1234\\\", \\\"PatientName\\\" : \\\"Alice Maja\\\", \\\"Room\\\" : \\\"Room 42\\\", \\\"Bed\\\" : \\\"1\\\", \\\"Department\\\" : \\\"Gyn-obs\\\", \\\"Category\\\" : \\\"Smertestillende\\\", \\\"Choice\\\" : \\\"Morfin\\\", \\\"Detail\\\" : null, \\\"CreatedOn\\\" : \\\"torsdag, 29 oktober 16:23:40\\\", \\\"ModifiedOn\\\" : null, \\\"Status\\\" : 0 }, { \\\"_id\\\" : \\\"563110394ca8e92fecbc7245\\\", \\\"PatientCPR\\\" : \\\"123456-1234\\\", \\\"PatientName\\\" : \\\"Alice Maja\\\", \\\"Room\\\" : \\\"Room 42\\\", \\\"Bed\\\" : \\\"1\\\", \\\"Department\\\" : \\\"Gyn-obs\\\", \\\"Category\\\" : \\\"Toilet\\\", \\\"Choice\\\" : null, \\\"Detail\\\" : null, \\\"CreatedOn\\\" : \\\"onsdag, 28 oktober 19:13:10\\\", \\\"ModifiedOn\\\" : null, \\\"Status\\\" : 0 }, { \\\"_id\\\" : \\\"563111754ca8e92fecbc7247\\\", \\\"PatientCPR\\\" : \\\"123456-1234\\\", \\\"PatientName\\\" : \\\"Alice Maja\\\", \\\"Room\\\" : \\\"Room 42\\\", \\\"Bed\\\" : \\\"1\\\", \\\"Department\\\" : \\\"Gyn-obs\\\", \\\"Category\\\" : \\\"Smertestillende\\\", \\\"Choice\\\" : \\\"Panodil\\\", \\\"Detail\\\" : null, \\\"CreatedOn\\\" : \\\"onsdag, 28 oktober 19:18:28\\\", \\\"ModifiedOn\\\" : null, \\\"Status\\\" : 0 }, { \\\"_id\\\" : \\\"563117824ca8e92aa4704f73\\\", \\\"PatientCPR\\\" : \\\"123456-1234\\\", \\\"PatientName\\\" : \\\"Alice Maja\\\", \\\"Room\\\" : \\\"Room 42\\\", \\\"Bed\\\" : \\\"1\\\", \\\"Department\\\" : \\\"Gyn-obs\\\", \\\"Category\\\" : \\\"Toilet\\\", \\\"Choice\\\" : null, \\\"Detail\\\" : null, \\\"CreatedOn\\\" : \\\"onsdag, 28 oktober 19:44:15\\\", \\\"ModifiedOn\\\" : null, \\\"Status\\\" : 0 }, { \\\"_id\\\" : \\\"56323a124ca8e90684a4b4bd\\\", \\\"PatientCPR\\\" : \\\"123456-1234\\\", \\\"PatientName\\\" : \\\"Alice Maja\\\", \\\"Room\\\" : \\\"Room 42\\\", \\\"Bed\\\" : \\\"1\\\", \\\"Department\\\" : \\\"Gyn-obs\\\", \\\"Category\\\" : \\\"Mad/Drikke\\\", \\\"Choice\\\" : \\\"Kaffe\\\", \\\"Detail\\\" : \\\"Mælk\\\", \\\"CreatedOn\\\" : \\\"torsdag, 29 oktober 16:24:02\\\", \\\"ModifiedOn\\\" : null, \\\"Status\\\" : 0 }]\"";

            var s = jsonString.Replace(@"\", String.Empty);

            var result = s.Trim().Substring(1, (s.Length) - 2);

            var json = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CallModel>>(result);

            Assert.IsNotNull(json);
        }

        [TestMethod]
        public void DeserializeJsonFromWebAPI_IsSuccessful3()
        {
            var jsonString =
                "{\"ChoiceId\":\"56446d90a7149f1d84000ae5\",\"Name\":\"Kaffe\",\"CategoryId\":\"56421f4c02a93d2c081a5f12\",\"DetailId\":[\"5643969402a93f11ac3fc8ba\",\"5643b80102a94125d426eadc\"]}";

            var json = JsonConvert.DeserializeObject<ChoiceModel>(jsonString);

            Assert.IsNotNull(json);
        }
    }
}
