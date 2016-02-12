using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.ConstrainedExecution;
using System.Text;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace PatientCareAdmin.Models
{
    public class HttpHandler<T>
    {
        private HttpClient _client { get; set; }
        private readonly string BaseURL = "http://patientcareapi.azurewebsites.net/";
        //private readonly string BaseURL = "http://localhost:2102/";
        public string Uri { get; set; }

        public HttpHandler(HttpClient client)
        {
            _client = client;
        }

        private HttpClient PrepareHttpClient()
        {
            _client.BaseAddress = new Uri(BaseURL);
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return _client;
        }

        public ResponseMessage Post(string dataToPost)
        {
            try
            {
                using (var client = PrepareHttpClient())
                {
                    StringContent content = new StringContent(dataToPost, Encoding.UTF8, "application/json");

                    var request = client.PostAsync(Uri, content).Result;

                    var result = request.Content.ReadAsStringAsync().Result;

                    var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseMessage>(result);

                    return obj;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        public List<T> Get()
        {
            try
            {
                using (var client = PrepareHttpClient())
                {
                    var response = client.GetAsync(Uri).Result;
                    var jsonResult = response.Content.ReadAsStringAsync().Result;

                    var s = jsonResult.Replace(@"\", string.Empty);

                    //var result = s.Trim().Substring(1, (s.Length) - 2);
                    List<T> json = null;

                    if (s.Contains("["))
                    {
                        json = Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(s);
                    }
                    else
                    {
                        s = "[" + s + "]";
                        json = Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(s);
                    }

                    if (json != null)
                    {
                        return json;
                    }
                }
                return (List<T>)Convert.ChangeType(null, typeof(List<T>));
            }
            catch (Exception ex)
            {
                return (List<T>)Convert.ChangeType(null, typeof(List<T>));
            }
        }

        public T Get(string id)
        {
            try
            {
                using (var client = PrepareHttpClient())
                {
                    var response = client.GetAsync(Uri + "/" + id).Result;
                    var jsonResult = response.Content.ReadAsStringAsync().Result;

                    var s = jsonResult.Replace(@"\", String.Empty);
                    var result = s.Trim().Substring(1, (s.Length) - 2);

                    var json = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(result);
                   
                    if (json != null)
                    {
                        return (T)Convert.ChangeType(json, typeof(T));
                    }
                    return (T)Convert.ChangeType(null, typeof(T));
                }
            }
            catch (Exception ex)
            {
                return (T)Convert.ChangeType(null,typeof(T));
            }
        }

        public ResponseMessage Delete(string id)
        {
            try
            {
                using (var client = PrepareHttpClient())
                {
                    var response = client.PostAsync(Uri + "/" + id, null).Result;
                    var jsonResult = response.Content.ReadAsStringAsync().Result;

                    var ResonseMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseMessage>(jsonResult);

                    return ResonseMessage;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        public ResponseMessage Put(string dataToPut)
        {
            try
            {
                using (var client = PrepareHttpClient())
                {
                    StringContent content = new StringContent(dataToPut, Encoding.UTF8, "application/json");

                    var request = client.PutAsync(Uri, content).Result;

                    var result = request.Content.ReadAsStringAsync().Result;

                    var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseMessage>(result);

                    return obj;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }
    }
}