using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatientCareWebApi.Models
{
    public class CreateCallModel
    {
        /// <summary>
        /// The callers CPR
        /// </summary>
        public string PatientCPR { get; set; }
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
    }
}