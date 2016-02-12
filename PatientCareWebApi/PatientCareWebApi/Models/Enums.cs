using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatientCareWebApi.Models
{
    public enum Enums
    {
        /// <summary>
        /// Call is active and waiting to be completed
        /// </summary>
        Active = 0,
        /// <summary>
        /// Call has been completed
        /// </summary>
        Completed = 1,
        /// <summary>
        /// Call has been canceled
        /// </summary>
        Canceled = 2

    }
}