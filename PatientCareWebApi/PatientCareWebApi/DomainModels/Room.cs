using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatientCareWebApi.DomainModels
{
    public class Room
    {
        public string  _id { get; set; }
        public string Number { get; set; }

        public string NumberOfBeds { get; set; }

        public List<Bed> Beds { get; set; }
    }
}