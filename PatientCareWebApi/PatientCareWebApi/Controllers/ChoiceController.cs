using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using PatientCareWebApi.DomainModels;
using PatientCareWebApi.Models;
using PatientCareWebApi.Repository.Interfaces;
using WebGrease.Css.Extensions;

namespace PatientCareWebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ChoiceController : ApiController
    {
        private IChoiceRepository _repository;
        /// <summary>
        /// Choice Controller constructor
        /// </summary>
        public ChoiceController(IChoiceRepository repository)
        {
            _repository = repository;
            
        }

        /// <summary>
        /// Get all choices from mongo
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public List<Choice> GetAllChoices()
        {
            try
            {
                var choiceList = new List<Choice>();

                var choices = _repository.GetAll();

                foreach (var item in choices)
                {
                    //Choice json = Newtonsoft.Json.JsonConvert.DeserializeObject<Choice>(item.ToString());

                    Choice json = BsonSerializer.Deserialize<Choice>(item);

                    choiceList.Add(new Choice()
                    {
                        ChoiceId = json.ChoiceId,
                        Name = json.Name,
                        CategoryId = json.CategoryId,
                        Details = json.Details
                    });
                }

                return choiceList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Add new choice
        /// </summary>
        /// <param name="choice"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public ActionResult Add(Choice choice)
        {
            try
            {
                var model = _repository.Add(choice);

                return new HttpStatusCodeResult(HttpStatusCode.Created, "New choice was created with id: " + model.ChoiceId);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        /// <summary>
        /// Delete a choice
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public ActionResult Delete(string id)
        {
            try
            {
               var result = _repository.Delete(id);
                if (result.IsAcknowledged && result.DeletedCount > 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.OK, "Choice was deleted");
                }
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);      
            }
        }
    }
}
