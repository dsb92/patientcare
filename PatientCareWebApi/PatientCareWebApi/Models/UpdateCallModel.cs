using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson.Serialization.Attributes;

namespace PatientCareWebApi.Models
{
    public class UpdateCallModel
    {
        /// <summary>
        /// MongoDocument id
        /// </summary>
        [BsonId]
        public string _id { get; set; }
        /// <summary>
        /// Call Status
        /// </summary>
        public int Status { get; set; }
    }
}