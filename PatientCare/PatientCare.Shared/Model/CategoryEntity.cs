using System;
using System.Collections.Generic;
using PatientCare.Shared.Model.Attributes;
using SQLite.Net.Attributes;

namespace PatientCare.Shared.Model
{
    public class CategoryEntity
    {
        [PrimaryKey]
        public String CategoryId { get; set; }

        public string Name { get; set; }

        public Action Tapped = null;

        public string Picture { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<ChoiceEntity> Choices { get; set; }
    }
}
