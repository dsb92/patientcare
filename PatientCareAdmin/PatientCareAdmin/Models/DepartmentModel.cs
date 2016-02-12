using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientCareAdmin.Models
{
    public class DepartmentModel
    {
        public Guid AfdelingId { get; set; }
        public string Navn {get; set; }
        public string Forkortelse { get; set; }
    }
}
