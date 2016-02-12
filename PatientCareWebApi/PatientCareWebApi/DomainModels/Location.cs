using System;

namespace PatientCareWebApi.DomainModels
{
    /// <summary>
    /// Details concering the location of the patient
    /// </summary>
    public class Location
    {
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
    }
}