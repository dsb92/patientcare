using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatientCareWebApi.DomainModels
{
    public class Department
    {
        public string DepartmentId { get; set; }

        public string Name { get; set; }
          
        public string Abbreviation { get; set; }
    }
}