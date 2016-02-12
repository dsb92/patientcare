//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Text;
//using System.Web.Http;
//using System.Web.Mvc;
//using Logging;
//using PatientCareWebApi.Models;

//namespace PatientCareWebApi.Controllers
//{
//    /// <summary>
//    /// PatientController
//    /// </summary>
//    public class AccountController : ApiController
//    {
//        private readonly ILog _log;
//        public AccountController()
//        {
//            _log = new Logger("WebAPI : PatientController");
//        }
//        /// <summary>
//        /// Check to See if PatientMayLogin 
//        /// </summary>
//        /// <param name="patient"></param>
//        /// <returns>True if found, false if not</returns>
//        public ActionResult PatientLogIn(PatientModel patient)
//        {
//            var util = new AccountUtil();
//            var validatedPatient = util.ValidatePatient(patient.CPR);

//            if (validatedPatient) //Hvis validatedPatient er true er personen fundet og må logge ind
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.Accepted);
//            }
//            else
//                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
//        }
//    }
//}
