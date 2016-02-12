using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using PatientCareWebApi.DomainModels;

namespace PatientCareWebApi.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Detail
    {
        /// <summary>
        /// DetailId
        /// </summary>
        [BsonId]
        public string DetailId { get; set; }
        /// <summary>
        /// Detail Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Detail -> Choice
        /// </summary>
        //public string ChoiceId { get; set; }
    }
}
