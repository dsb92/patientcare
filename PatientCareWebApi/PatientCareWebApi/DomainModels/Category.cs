using MongoDB.Bson.Serialization.Attributes;

namespace PatientCareWebApi.DomainModels
{
    /// <summary>
    /// Category Model for PatientCare Web API
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Category Id 
        /// </summary>
        [BsonId]
        public string CategoryId { get; set; }
        /// <summary>
        /// Category Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Id to a picture stored somewhere else
        /// </summary>
        public string Picture { get; set; }
    }
}
