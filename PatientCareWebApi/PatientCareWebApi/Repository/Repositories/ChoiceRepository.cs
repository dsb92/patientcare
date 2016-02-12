using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using PatientCareWebApi.DomainModels;
using PatientCareWebApi.Repository.Interfaces;

namespace PatientCareWebApi.Repository.Repositories
{
    public class ChoiceRepository : IChoiceRepository
    {
        private readonly IMongoCollection<BsonDocument> _choices;
        private readonly IMongoDatabase _db;
        private readonly ILog _log;
        public ChoiceRepository()
        {
            var client = new MongoClient(ConfigurationManager.ConnectionStrings["Mongo_patientcare"].ConnectionString);
            _log = new Logger("WebAPI:ChoiceController");
            _db = client.GetDatabase("patientcaredb");
            _choices = _db.GetCollection<BsonDocument>("Choice");
        }

        public List<BsonDocument> GetAll()
        {
            try
            {
                return _choices.Find(new BsonDocument()).ToListAsync().Result;
            }
            catch (Exception ex) 
            {
                _log.Exception(ex.Message + ex.InnerException);
                throw;
            }
        }

        public BsonDocument Get(string id)
        {
            try
            {
                return _choices.Find(Builders<BsonDocument>.Filter.Eq("_id", id)).SingleAsync().Result;
            }
            catch (Exception ex)
            {
                _log.Exception(ex.Message + ex.InnerException);
                throw;
            }
        }

        public Choice Add(Choice choice)
        {
            try
            {
                var newChoice = new Choice()
                {
                    ChoiceId = ObjectId.GenerateNewId().ToString(),
                    CategoryId = choice.CategoryId,
                    Details = choice.Details,
                    Name = choice.Name
                };

                var returned = _choices.InsertOneAsync(newChoice.ToBsonDocument());
                returned.Wait();
                _log.Debug("New choice added with id: " + newChoice.ChoiceId);
                return newChoice;
            }
            catch (Exception ex)
            {
                _log.Exception(ex.Message + ex.InnerException);
                throw;
            }
        }

        public DeleteResult Delete(string id)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            var deletedChoice = _choices.DeleteOneAsync(filter).Result;

            return deletedChoice;
        }
    }
}