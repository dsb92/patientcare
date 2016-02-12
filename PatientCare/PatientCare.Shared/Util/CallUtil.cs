namespace PatientCare.Shared.Util
{
    /// <summary>
    /// Statisk klasse der håndter alt ekstra ting, der skal være tilknyttet et kald.
    /// </summary>
    public static class CallUtil
    {
        /// <summary>
        /// Active = Kaldet er afventende
        /// Completed = Kaldet er udført
        /// Canceled = Kaldet er fortrudt
        /// </summary>
        public enum StatusCode
        {
            Active,
            Waiting,
            Canceled,
            Completed
        }
    }
}
