using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PatientCare.Shared.DTO;
using PatientCare.Shared.Interfaces;

namespace PatientCare.Shared
{
    public class HttpHandler
    {
        //private readonly  string BaseURL = "http://systematicopgaveURL.dk/tasks/"
       
        private readonly string BaseURL = "http://patientcareapi.azurewebsites.net/";
        public HttpHandler()
        {
        }

        public enum API
        {
            Patient,
            Task,
            Call,
            Category,
            Choice,
            Detail
        }

        public HttpClient PrepareHttpClient()
        {
            const string username = "60423881:";
            const string password = "dimmer";

            var byteArrayUsernamePassword = Encoding.UTF8.GetBytes(username + password);
            string encodedUsernamePassword = Convert.ToBase64String(byteArrayUsernamePassword);
            
            var client = new HttpClient();

            client.BaseAddress = new Uri(BaseURL);
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedUsernamePassword);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        public void PostData(StringContent content, API api)
        {
            var client = PrepareHttpClient();
            using (client)
            {
                string apiStr = Enum.GetName(typeof(API), api);
                
                var request = client.PostAsync("api/"+apiStr, content).Result;
                var result = request.Content.ReadAsStringAsync().Result;
                
                // result = "{\"StatusCode\":201,\"StatusDescription\":\"New call was created with id : 5631004a4ca8e9290cddd46b\"}" on success

                Dictionary<string, string> jsonDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);

                var statusCode = jsonDictionary["StatusCode"];
                var statusDesc = jsonDictionary["StatusDescription"];

                var seperator = statusDesc.IndexOf(":");

                var callId = statusDesc.Substring(seperator+1).Trim();

                MongoCallId = callId;

                // If result is not as below, throw exception
                if (!statusCode.Contains("201"))
                {
                    throw new Exception("Error sending call: Response result code not 201!");
                }

            }
        }

        public void PutData(StringContent content, API api)
        {
            var client = PrepareHttpClient();
            using (client)
            {
                string apiStr = Enum.GetName(typeof(API), api);

                var request = client.PutAsync("api/" + apiStr, content).Result;
                var result = request.Content.ReadAsStringAsync().Result;

                Dictionary<string, string> jsonDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);

                var statusCode = jsonDictionary["StatusCode"];

                // If result is not as below, throw exception
                if (!statusCode.Contains("202"))
                {
                    throw new Exception("Error sending call: Response result code not 202!");
                }
            }
        }

        public string GetData(API api, string id=null)
        {
            var client = PrepareHttpClient();
            using (client)
            {
                string apiStr = Enum.GetName(typeof (API), api);
                string apiStrFull = "api/" + apiStr;

                // If we are passing an id
                if (id != null)
                {
                    apiStrFull += "/" + id;
                }

                var request =  client.GetAsync(apiStrFull).Result;
                var result = request.Content.ReadAsStringAsync().Result;

                // If result is not as below, throw exception
                if (request.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("Error sending call: Response result code not 200!");
                }

                return result;
            }
        }


        public static string MongoCallId { get; set; }
    }
}

