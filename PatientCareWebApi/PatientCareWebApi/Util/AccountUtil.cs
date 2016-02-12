//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using Logging;
//using PatientCareWebApi.Models;

//namespace PatientCareWebApi.Data
//{
//    /// <summary>
//    /// This Class is taking care of all Patient speficic data queries to the DB
//    /// </summary>
//    public class AccountUtil
//    {
        
//        private readonly ILog _log;
//        /// <summary>
//        /// 
//        /// </summary>
//        public AccountUtil()
//        {
//            _log = new Logger("WebAPI: Util");
//        }

//        /// <summary>
//        /// Validating Patient on datamock DB
//        /// </summary>
//        /// <param name="CPR"></param>
//        /// <returns>True if validated, false if not</returns>
//        public bool ValidatePatient(string CPR)
//        {
//            var PatientFound = SearchForPatientByCPR(CPR);

//            if (PatientFound)
//            {
//                _log.Debug(string.Format("Patient with CPR: {0} was found in dataMock", CPR));
//                return true;
//            }
//            _log.Debug(string.Format("Couldn't find patient with CPR: {0} in dataMock", CPR));
//            return false;
//        }
//        /// <summary>
//        /// Searching for Patient in datamock
//        /// </summary>
//        /// <param name="CPR"></param>
//        /// <returns>True or false. See function description for more detailed</returns>
//        private bool SearchForPatientByCPR(string CPR)
//        {
//            var query = (from patient in _dataMock.Patients
//                         where patient.CPR_Nummer == CPR
//                         select patient).FirstOrDefault();

//            if (query != null) //Hvis patient er fundet er CPR-Nr. udfyldt
//            {
//                if (query.TimeOfDischarge != null) //Er udskrivelses tidspunkt udfyldt? != null (Ja) == null (Nej)
//                {
//                    if (query.TimeOfDischarge < DateTime.Now) //Er udskrivelses tidspunkt før idag?
//                    {
//                        return false;
//                    }
//                }
//            }
//            return true;
//        }
//        /// <summary>
//        /// Gets a PatientId from PatientCPR
//        /// </summary>
//        /// <param name="CPR"></param>
//        /// <returns>Patient Id</returns>
//        public Guid GetPatientIdByPatientCPR(string CPR)
//        {
//            var query = (from patient in _dataMock.Patients
//                where patient.CPR_Nummer == CPR
//                select patient.PatientId).FirstOrDefault();

//            return query;
//        }
//    }
//}