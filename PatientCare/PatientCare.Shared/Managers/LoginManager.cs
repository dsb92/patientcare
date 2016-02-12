using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PatientCare.Shared.Managers
{
    /// <summary>
    /// Klasse til håndtering af Login. 
    /// </summary>
    public class LoginManager
    {
        /// <summary>
        /// Tjekker om det valide CPR nummer findes i databasen, hvor vi antager at hvis det gør, så er patienten indlagt
        /// </summary>
        /// <param name="userCpr"></param>
        /// <returns>Returner CPR-nummer fra server</returns>
        public String GetPatient(string userCpr)
        {
            var httpHandler = new HttpHandler();

            var patientJson = httpHandler.GetData(HttpHandler.API.Patient, userCpr);

            Dictionary<string, string> jsonDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(patientJson);

            var patientCpr = jsonDictionary["PatientCPR"];

            return patientCpr;
        }
    }
}
