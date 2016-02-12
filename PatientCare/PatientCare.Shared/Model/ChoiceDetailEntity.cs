using System;
using PatientCare.Shared.Model.Attributes;
using SQLite.Net.Attributes;

namespace PatientCare.Shared.Model
{
    public class ChoiceDetailEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(ChoiceEntity))]
        public String ChoiceId { get; set; }

        [ForeignKey(typeof(DetailEntity))]
        public String DetailId { get; set; }
    }
}
