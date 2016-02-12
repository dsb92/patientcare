using System;
using System.Collections.Generic;
using PatientCare.Shared.Model.Attributes;
using SQLite.Net.Attributes;

namespace PatientCare.Shared.Model
{
    public class ChoiceEntity
    {
        [PrimaryKey]
        public String ChoiceId { get; set; }

        public string Name { get; set; }

        [ForeignKey(typeof(CategoryEntity))]
        public String CategoryId { get; set; }

        [ManyToMany(typeof(ChoiceDetailEntity), CascadeOperations = CascadeOperation.All)]
        public List<DetailEntity> Details { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.All)]
        public CategoryEntity CategoryEntity { get; set; }
    }
}
