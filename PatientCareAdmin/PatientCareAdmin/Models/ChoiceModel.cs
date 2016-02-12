using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientCareAdmin.Models
{
    public class ChoiceModel
    {
        public string ChoiceId { get; set; }
        public string CategoryId { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public List<DetailModel> Details { get; set; }
        public string DetailsList { get; set; }
    }
}
