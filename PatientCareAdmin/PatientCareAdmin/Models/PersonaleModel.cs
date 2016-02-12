using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientCareAdmin.Models
{
    public class PersonaleModel
    {
        public Guid PersonaleId { get; set; }
        public string Navn { get; set; }
        public string Funktion { get; set; }
        public string Afdeling { get; set; }
    }
}
