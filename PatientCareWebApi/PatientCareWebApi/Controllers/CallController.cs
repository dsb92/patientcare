using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using PatientCareWebApi.DomainModels;
using PatientCareWebApi.Models;
using PatientCareWebApi.Repository.Interfaces;

namespace PatientCareWebApi.Controllers
{
    /// <summary>
    /// </summary>
    //[IdentityBasicAuthentication]
    //[System.Web.Http.Authorize]
    public class CallController : ApiController
    {
        private readonly ICallRepository _callRepo;

        /// <summary>
        ///     Callcontroller using DI
        /// </summary>
        /// <param name="callRepository"></param>
        public CallController(ICallRepository callRepository)
        {
            _callRepo = callRepository;
        }

        /// <summary>
        ///     All Active Calls
        /// </summary>
        /// <returns>Returns a json formattet string, containing all Active Patientcalls</returns>
        [System.Web.Http.HttpGet]
        public string GetAllActiveCalls()
        {
            return _callRepo.GetAll().ToJson();
        }

        //[System.Web.Http.HttpGet]
        //public List<CallModel> GetAllActiveCalls()
        //{
        //   try
        //    {
        //        var calls = _callRepo.GetAll();
        //        var callList = new List<CallModel>();

        //        foreach (var item in calls)
        //        {
        //            callList.Add(BsonSerializer.Deserialize<CallModel>(item));
        //        }
        //        return callList;
        //    }
        //    catch (Exception ex)
        //    {
        //        return new List<CallModel>();
        //    }
        //}

        /// <summary>
        ///     Get Call by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns specific call </returns>
        public string GetCallById(string id)
        {
            return _callRepo.Get(id).ToJson();
        }

        /// <summary>
        ///     Create New Call
        /// </summary>
        /// <param name="callModel"></param>
        [System.Web.Http.HttpPost]
        public ActionResult CreateCall([FromBody] CreateCallModel callModel)
        {
            if (callModel != null)
            {
                if (callModel.PatientCPR == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "PatientCPR is null");
                }
                try
                {
                    var call = _callRepo.Add(callModel);
                    if (call != null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.Created, "New call was created with id : " + call);
                    }
                }
                catch (Exception ex)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message + ex.InnerException);
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No data in request");
        }

        /// <summary>
        ///     Update existing call
        /// </summary>
        /// <param name="callModel"></param>
        /// <returns>HttpResponse with Statuscode and Description</returns>
        [System.Web.Http.HttpPut]
        public ActionResult UpdateCall([FromBody] UpdateCallModel callModel)
        {
            if (callModel == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Context could not be serialized");
            }
            var call = _callRepo.Update(callModel);
            if (call.IsAcknowledged && call.MatchedCount > 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Accepted,
                    $"Call with id: {callModel._id} was updated with Status: {Enum.GetName(typeof (Enums), callModel.Status)}");
            }
            return new HttpStatusCodeResult(HttpStatusCode.Conflict, "Call status was not updated");
        }

        //public ActionResult DeleteCall([FromBody]string callId)
        //[System.Web.Http.HttpDelete]
        ///// <param name="callId"></param>
        ///// </summary>
        ///// Cancel a Call
        ///// <summary>
        //{
        //    if (callId == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    //CallRepository.Cancel(callId);
        //    return new HttpStatusCodeResult(HttpStatusCode.Accepted);
        //}
    }
}