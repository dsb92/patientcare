using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientCare.Shared
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
            Completed,
            Canceled
        }
    }
}
