using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using PatientCareWebApi.Models;

namespace PatientCareWebApi.DomainModels
{
    /// <summary>
    /// 
    /// </summary>
    public class Choice
    {
        /// <summary>
        /// Choice Id
        /// </summary>
        [BsonId]
        public string ChoiceId { get; set; }
        /// <summary>
        /// Choice Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Choice -> Category
        /// </summary>
        public string CategoryId { get; set; }
        /// <summary>
        /// Choice -> Detail
        /// </summary>
        public List<Detail> Details { get; set; }
    }
}
