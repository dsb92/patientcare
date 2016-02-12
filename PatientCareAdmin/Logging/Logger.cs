using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Logging
{
    public class Logger : ILog
    {
        public string _logger { get; set; }
        private readonly IMongoDatabase _db;
        private readonly IMongoCollection<BsonDocument> _logCollection;

        public Logger(string logger)
        {
            var client = new MongoClient(Properties.Settings.Default.Mongo_log);
            _db = client.GetDatabase("patientcarelog");
            _logCollection = _db.GetCollection<BsonDocument>("Logs");
            _logger = logger;
        }

        public void Debug(string message)
        {
            var newLog = new Log()
            {
                TimeStamp = DateTime.Now,
                Logger = _logger,
                Message = message,
                Type = "Debug"
            };
            _logCollection.InsertOneAsync(newLog.ToBsonDocument());

        }

        public void Exception(string message)
        {
            var newLog = new Log()
            {
                TimeStamp = DateTime.Now,
                Logger = _logger,
                Message = message,
                Type = "Exception"
            };
            _logCollection.InsertOneAsync(newLog.ToBsonDocument());
        }
    }
}
