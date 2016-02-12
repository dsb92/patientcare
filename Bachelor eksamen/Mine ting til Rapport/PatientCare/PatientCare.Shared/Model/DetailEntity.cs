using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientCare.Shared.Model.Attributes;
using SQLite.Net.Attributes;

namespace PatientCare.Shared.Model
{
    public class DetailEntity
    {
        [PrimaryKey]
        public String DetailId { get; set; }

        public string Name { get; set; }

        [ManyToMany(typeof(ChoiceDetailEntity), CascadeOperations = CascadeOperation.All)]
        public List<ChoiceEntity> Choices { get; set; }
    }
}
