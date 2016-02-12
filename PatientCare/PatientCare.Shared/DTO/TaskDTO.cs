namespace PatientCare.Shared.DTO
{
    public class TaskDTO
    {
        #region Mandatory
        public int CreatedTime { get; set; }
        public int LastChanged { get; set; }
        public int NoOfWorkersRequired {get; set; }
        public string SourceSystem { get; set; }
        public TaskRequester TaskRequester { get; set; }
        public string TaskStatus { get; set; }
        public string Type { get; set; }
        public string UniqueId { get; set; }
        public string Urgency { get; set; }
        #endregion
    }
    public class TaskRequester
    {
        public string Name { get; set; }
        public string OrganizationUserId { get; set; }
        public string Phone { get; set; }
    }
}
