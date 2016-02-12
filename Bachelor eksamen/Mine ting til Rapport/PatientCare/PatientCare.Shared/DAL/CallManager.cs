using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using PatientCare.Shared.Model;

namespace PatientCare.Shared.DAL
{
    public class CallManager
    {
        /// <summary>
        /// Omdanner et kald til Json og sender et kald til Web API.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="data"></param>
        public void PostCall(string content)
        {
            var httpHandler = new HttpHandler();

            StringContent httpContent = new StringContent(content, Encoding.UTF8, "application/json");

            httpHandler.PostData(httpContent, HttpHandler.API.Call);
        }

        /// <summary>
        /// Omdanner et kald til Json og opdater kaldet til Web API.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="data"></param>
        public void PutCall(string content)
        {
            var httpHandler = new HttpHandler();

            StringContent httpContent = new StringContent(content, Encoding.UTF8, "application/json");

            httpHandler.PutData(httpContent, HttpHandler.API.Call);
        }

        /// <summary>
        /// Henter alle kald fra Web API som en Patient har foretaget
        /// </summary>
        /// <returns>Returnere en Liste af kald.</returns>
        public String GetStatusCall(CallEntity callEntity)
        {
            var httpHandler = new HttpHandler();

            var callJson = httpHandler.GetData(HttpHandler.API.Call, callEntity._id);

            var json = JsonConvert.DeserializeObject(callJson);

            Dictionary<string, string> jsonDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json as string);

            var callStatus = jsonDictionary["Status"];

            return callStatus;
        }

    }
}
