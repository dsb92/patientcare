using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientCare.Shared.Model.Attributes;

namespace PatientCare.Shared.Model
{
    public class CallObject
    {
        [ForeignKey(typeof(ChoiceEntity))]
        public int ChoiceId { get; set; }

        [ForeignKey(typeof(DetailEntity))]
        public int DetailId { get; set; }
    }
}
