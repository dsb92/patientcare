using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq.Language.Flow;
using PatientCareWebApi.DomainModels;
using PatientCareWebApi.Models;
using PatientCareWebApi.Repository.Interfaces;

namespace PatientCareWebApi.Repository.Repositories
{
    /// <summary />
    public class PatientRepository : IPatientRepository
    {
        private readonly ILog _log;
        private readonly IMongoDatabase _db;
        private readonly IMongoCollection<BsonDocument> _patients;

        /// <summary />
        public PatientRepository()
        {
            var client = new MongoClient(ConfigurationManager.ConnectionStrings["Mongo_patientcare_datamock"].ConnectionString);
            _log = new Logger("WebAPI : PatientRepository");
            _db = client.GetDatabase("patientcare_datamock");
            _patients = _db.GetCollection<BsonDocument>("Patients");
        }
        /// <summary>
        /// Get all Patients from datamock
        /// </summary>
        /// <returns>List of Patients</returns>
        public List<BsonDocument> GetAll()
        {
            try
            {
                _log.Debug("Get all patients from datamock");
                return _patients.Find(new BsonDocument()).ToListAsync().Result;
            }
            catch (Exception ex)
            {
                _log.Exception(ex.Message + ex.InnerException);
                throw;
            }
        }

        /// <summary>
        /// Get Speficic Patient
        /// </summary>
        /// <param name="id">PatientCPR</param>
        /// <returns>Speficic patient</returns>
        public BsonDocument Get(string id)
        {
            try
            {
                _log.Debug("Getting patient by id: " + id);
                return _patients.Find(Builders<BsonDocument>.Filter.Eq("PatientCPR", id)).SingleAsync().Result.ToBsonDocument();
            }
            catch (Exception ex)
            {
                _log.Exception(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Add Patient to Datamock
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        public string Add(Patient patient)
        {
            try
            {
                _log.Debug("Adding new Patient to datamock");
                _patients.InsertOneAsync(patient.ToBsonDocument()).Wait();
                return patient._id;
            }
            catch (Exception ex)
            {
                _log.Exception(ex.Message + ex.InnerException);
                throw;
            }
        }
        /// <summary>
        /// Remove patient from datamock
        /// </summary>
        /// <param name="id"></param>
        public DeleteResult Delete(string id)
        {
            try
            {
                var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
                return _patients.DeleteOneAsync(filter).Result;
            }
            catch (Exception ex)
            {
                _log.Exception(ex.Message + ex.InnerException);
                throw;
            }
        }
        /// <summary>
        /// Update patient in datamock
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        public UpdateResult Update(Patient patient)
        {
            try
            {
                var filter = Builders<BsonDocument>.Filter.Eq("_id", patient._id);
                var update =
                    Builders<BsonDocument>.Update.Set("PatientCPR", patient.PatientCPR)
                        .Set("PatientName", patient.PatientName)
                        .Set("ImportantInfo", patient.ImportantInfo);
                return _patients.UpdateOneAsync(filter, update).Result;
            }
            catch (Exception ex)
            {
                _log.Exception(ex.Message + ex.InnerException);
                throw;
            }
        }
    }
}