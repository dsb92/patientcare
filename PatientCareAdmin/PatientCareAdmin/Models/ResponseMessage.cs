using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatientCareAdmin.Models
{
    public class ResponseMessage
    {
        public int StatusCode { get; set; }
        public string StatusDescription { get; set; }
    }
}