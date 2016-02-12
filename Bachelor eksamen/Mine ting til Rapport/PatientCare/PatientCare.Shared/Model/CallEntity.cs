using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PatientCare.Shared.Interfaces;
using SQLite.Net.Attributes;

namespace PatientCare.Shared.Model
{
    public class CallEntity
    {
        [PrimaryKey]
        public String _id { get; set; }
        public string PatientCPR { get; set; } 
        public string Category { get; set; }
        public string Choice { get; set; }
        public string Detail { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedOn { get; set; }
        public int Status { get; set; }
    }
}
