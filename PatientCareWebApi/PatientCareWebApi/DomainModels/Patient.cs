using MongoDB.Bson.Serialization.Attributes;

namespace PatientCareWebApi.DomainModels
{
    /// <summary>
    /// Details concerning the patient
    /// </summary>
    public class Patient
    {
        /// <summary>
        /// PatientI 
        /// </summary>
        [BsonId]
        public string _id { get; set; }
        /// <summary>
        /// The callers CPR
        /// </summary>
        public string PatientCPR { get; set; }
        /// <summary>
        /// PatientName
        /// </summary>
        public string PatientName { get; set; }
        /// <summary>
        /// ImportantInfo about the patient
        /// </summary>
        public string ImportantInfo { get; set; }
    }
}