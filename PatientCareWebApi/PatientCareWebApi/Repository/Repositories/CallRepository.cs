using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Logging;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using PatientCareWebApi.DomainModels;
using PatientCareWebApi.Models;
using PatientCareWebApi.Repository.Interfaces;

namespace PatientCareWebApi.Repository.Repositories
{
    /// <summary>
    /// </summary>
    public class CallRepository : ICallRepository
    {
        private readonly IMongoCollection<BsonDocument> _calls;
        private readonly IMongoDatabase _db;
        private readonly ILog _log;

        /// <summary>
        /// </summary>
        public CallRepository()
        {
            var client = new MongoClient(ConfigurationManager.ConnectionStrings["Mongo_patientcare"].ConnectionString);
            _log = new Logger("WebAPI : CallRepository");
            _db = client.GetDatabase("patientcaredb");
            _calls = _db.GetCollection<BsonDocument>("Calls");
        }

        /// <summary>
        ///     Create new call and add it to MongoDB
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string Add(CreateCallModel entity)
        {
            try
            {
                var date = DateTime.Parse(entity.CreatedOn);
                var _patient = GetPatient(entity.PatientCPR).Values.ToArray();
                var newCall = new CallModel
                {
                    _id = ObjectId.GenerateNewId().ToString(),
                    PatientCPR = entity.PatientCPR,
                    PatientName = _patient[2].ToString(),
                    Department = _patient[3].ToString(),
                    Room = _patient[4].ToString(),
                    Bed = _patient[5].ToString(),
                    Choice = entity.Choice,
                    Category = entity.Category,
                    Detail = entity.Detail,
                    CreatedOn = date.ToString("t"),
                    Status = (int) Enums.Active
                };

                var call = _calls.InsertOneAsync(newCall.ToBsonDocument());
                call.Wait();
                _log.Debug("New call added");
                return newCall._id;
            }
            catch (Exception ex)
            {
                _log.Exception(ex.Message);
                throw;
            }
        }

        /// <summary>
        ///     Update exisiting Call Status
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public UpdateResult Update(UpdateCallModel entity)
        {
            try
            {
                var filter = Builders<BsonDocument>.Filter.Eq("_id", entity._id);
                var update = Builders<BsonDocument>.Update.Set("Status", entity.Status)
                    .Set("ModifiedOn", DateTime.Now.ToString("F"));
                var callUpdated = _calls.UpdateOneAsync(filter, update).Result;
                _log.Debug(callUpdated.IsAcknowledged.ToString() + callUpdated.MatchedCount);
                return callUpdated;
            }
            catch (Exception ex)
            {
                _log.Exception(ex.Message);
                throw;
            }
        }

        /// <summary>
        ///     Get All Active Calls
        /// </summary>
        /// <returns>Retruns af Json Formattet string with all active calls in it</returns>
        public List<BsonDocument> GetAll()
        {
            try
            {
                var jsonSettings = new JsonWriterSettings {OutputMode = JsonOutputMode.Strict};
                var calls =
                    _calls.Find(Builders<BsonDocument>.Filter.Eq("Status", (int) Enums.Active)).ToListAsync().Result;
                    //.ToJson(jsonSettings);

                return calls;
            }
            catch (Exception ex)
            {
                _log.Exception(ex.Message + ex.InnerException);
                throw;
            }
        }

        /// <summary>
        ///     Get Speficic Call
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public BsonDocument Get(string Id)
        {
            try
            {
                var jsonSettings = new JsonWriterSettings {OutputMode = JsonOutputMode.Strict};
                return _calls.Find(Builders<BsonDocument>.Filter.Eq("_id", Id)).SingleAsync().Result;
            }
            catch (Exception ex)
            {
                _log.Exception(ex.Message + ex.InnerException);
                throw;
            }
        }

        /// <summary>
        ///     Get Speficic Patient
        /// </summary>
        /// <param name="patientCPR"></param>
        /// <returns></returns>
        public BsonDocument GetPatient(string patientCPR)
        {
            var patientRepo = new PatientRepository();

            return patientRepo.Get(patientCPR);
        }
    }
}