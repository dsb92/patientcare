using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatientCareWebApi.DomainModels
{
    public class Bed
    {
        public string Number { get; set; }

        public Patient patient { get; set; }
    }
}