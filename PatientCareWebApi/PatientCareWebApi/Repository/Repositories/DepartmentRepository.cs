using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace PatientCareWebApi.Repository.Repositories
{
    

    public class DepartmentRepository
    {
        private readonly ILog _log;
        private readonly IMongoDatabase _db;
        private readonly IMongoCollection<BsonDocument> _departments;

        public DepartmentRepository()
        {
            var client = new MongoClient(ConfigurationManager.ConnectionStrings["Mongo_patientcare_datamock"].ConnectionString);
            _log = new Logger("WebAPI : PatientRepository");
            _db = client.GetDatabase("patientcare_datamock");
            _departments = _db.GetCollection<BsonDocument>("Departments");
        }

        public BsonDocument GetDepartmentByRoom(string id)
        {
            return _departments.Find(Builders<BsonDocument>.Filter.Eq("PatientCPR", id)).SingleAsync().Result;
        }
    }
}