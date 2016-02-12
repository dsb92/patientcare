using System;

namespace PatientCareWebApi.DomainModels
{
    /// <summary>
    /// Attributes about the call
    /// </summary>
    public class CallAttributes
    {
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