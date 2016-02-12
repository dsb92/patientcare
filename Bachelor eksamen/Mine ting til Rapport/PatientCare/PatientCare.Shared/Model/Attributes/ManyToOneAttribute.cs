using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientCare.Shared.Model.Attributes
{
    public class ManyToOneAttribute : RelationshipAttribute
    {
        public ManyToOneAttribute(string foreignKey = null, string inverseProperty = null)
            : base(foreignKey, null, inverseProperty)
        {
        }

    }
}
