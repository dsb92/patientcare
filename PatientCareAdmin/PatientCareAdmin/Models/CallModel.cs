using System;
using MongoDB.Bson.Serialization.Attributes;

namespace PatientCareAdmin.Models
{
    public class CallModel
    {
        /// <summary>
        /// Call id
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
        /// Room where the Bed is located
        /// </summary>
        public string Room { get; set; }
        /// <summary>
        /// The Patient's Bed
        /// </summary>
        public string Bed { get; set; }
        /// <summary>
        /// Department containing the Room e.g. Patient
        /// </summary>
        public string Department { get; set; }
        /// <summary>
        /// The calls Category
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// The calls choice
        /// </summary>
        public string Choice { get; set; }
        /// <summary>
        /// The calls Detail
        /// </summary>
        public string Detail { get; set; }
        /// <summary>
        /// Time for call creation
        /// </summary>
        public string CreatedOn { get; set; }
        /// <summary>
        /// Time for Modified call status
        /// </summary>
        public string ModifiedOn { get; set; }
        /// <summary>
        /// Call Status
        /// </summary>
        public int Status { get; set; }
    }
}