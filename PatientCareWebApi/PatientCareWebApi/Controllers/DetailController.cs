using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using PatientCareWebApi.DomainModels;
using PatientCareWebApi.Models;

namespace PatientCareWebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class DetailController : ApiController
    {
        private readonly IMongoCollection<BsonDocument> _details;
        private readonly IMongoDatabase _db;
        private readonly ILog _log;

        /// <summary>
        /// Details controller constructor
        /// </summary>
        public DetailController()
        {
            var client = new MongoClient(ConfigurationManager.ConnectionStrings["Mongo_patientcare"].ConnectionString);
            _log = new Logger("WebAPI:DetailController");
            _db = client.GetDatabase("patientcaredb");
            _details = _db.GetCollection<BsonDocument>("Detail");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Detail> GetAllDetails()
        {
            try
            {
                var detailList = new List<Detail>();

                var details = _details.Find(new BsonDocument()).ToListAsync().Result;

                foreach (var item in details)
                {
                    var detail = BsonSerializer.Deserialize <Detail>(item);

                    detailList.Add(new Detail()
                    {
                        DetailId = detail.DetailId,
                        Name = detail.Name
                    });
                }

                return detailList;
            }
            catch (Exception ex)
            {
                _log.Exception(ex.Message + ex.InnerException);
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public ActionResult Add(Detail detail)
        {
            try
            {
                detail.DetailId = ObjectId.GenerateNewId().ToString();

                var cat = _details.InsertOneAsync(detail.ToBsonDocument());
                cat.Wait();

                if (detail.DetailId != null && cat.Status == TaskStatus.RanToCompletion)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Created, "New detail was added with name: " + detail.Name);
                }
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No new detail was added");
            }
            catch (Exception ex)
            {
                _log.Exception(ex.Message + ex.InnerException);
                throw;
            }
        }

        /// <summary>
        /// Delete a category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public ActionResult Delete(string id)
        {
            try
            {
                var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
                var deletedCategory = _details.DeleteOneAsync(filter).Result;
                if (deletedCategory.IsAcknowledged && deletedCategory.DeletedCount > 0)
                    return new HttpStatusCodeResult(HttpStatusCode.OK, "Category with id: " + id + " was deleted");

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Category was not deleted");
            }
            catch (Exception ex)
            {
                _log.Exception(ex.Message + ex.InnerException);
                throw;
            }
        }
    }
}
