using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using MongoDB.Bson;
using PatientCareWebApi.DomainModels;
using PatientCareWebApi.Repository.Interfaces;
using PatientCareWebApi.Repository.Repositories;

namespace PatientCareWebApi.Controllers
{
    /// <summary>
    /// PatientController to access datamock
    /// </summary>
    public class PatientController : ApiController
    {
        private IPatientRepository _repository;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="patientRepository"></param>
        public PatientController(IPatientRepository patientRepository)
        {
            _repository = patientRepository;
        }
        /// <summary>
        /// Get all Patients from datamock
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public List<Patient> Get()
        {
            try
            {
                var patientList = new List<Patient>();
                var patients = _repository.GetAll();

                foreach (var item in patients)
                {
                    patientList.Add(new Patient()
                    {
                        _id = item.Values.ToArray()[0].ToString(),
                        PatientCPR = item.Values.ToArray()[1].ToString(),
                        PatientName = item.Values.ToArray()[2].ToString(),
                        ImportantInfo = item.Values.ToArray()[3].ToString()
                    });    
                }
                return patientList;
            }
            catch (Exception ex)
            {
                return new List<Patient>();
            }
        }

        /// <summary>
        /// Get specific Patient from datamock
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Patient</returns>
        [System.Web.Http.HttpGet]
        public Patient Get(string id)
        {
            try
            {
                var result = _repository.Get(id);
                var patient = new Patient
                {
                    _id = result.Values.ToArray()[0].ToString(),
                    PatientCPR = result.Values.ToArray()[1].ToString(),
                    PatientName = result.Values.ToArray()[2].ToString(),
                    ImportantInfo = result.Values.ToArray()[3].ToString()
                };
                return patient;
            }
            catch (Exception ex)
            {
                return new Patient();
            }
        }

        ///// <summary>
        ///// Create new Patient in datamock
        ///// </summary>
        ///// <param name="value">PatientDTO</param>
        //[System.Web.Http.HttpPost]
        //public ActionResult Post([FromBody]Patient value)
        //{
        //    try
        //    {
        //        var result = _repository.Add(value);
        //        return new HttpStatusCodeResult(HttpStatusCode.Created, "Patient was added to datamock with id: " + result);
        //    }
        //    catch(Exception ex)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message + ex.InnerException);
        //    }
        //}

        ///// <summary>
        ///// Update existing patient in datamock
        ///// </summary>
        ///// <param name="value"></param>
        //[System.Web.Http.HttpPut]
        //public ActionResult Put([FromBody]Patient value)
        //{
        //    try
        //    {
        //        var update = _repository.Update(value);

        //        if(update.IsAcknowledged && update.MatchedCount > 0)
        //            return new HttpStatusCodeResult(HttpStatusCode.OK, "Patient was updated");
        //        else
        //        {
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Patient was not updated");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message + ex.InnerException);
        //    }
        //}

        ///// <summary>
        ///// Delete patient from datamock
        ///// </summary>
        ///// <param name="id"></param>
        //[System.Web.Http.HttpDelete]
        //public ActionResult Delete(string id)
        //{
        //    try
        //    {
        //        var result = _repository.Delete(id);
        //        if (result.IsAcknowledged && result.DeletedCount > 0)
        //        {
        //            return new HttpStatusCodeResult(HttpStatusCode.OK, "Patient with id: " + id + " was removed");
        //        }
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Patient was not removed");
        //    }
        //    catch (Exception ex)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message + ex.InnerException);
        //    }
        //}
    }
}
