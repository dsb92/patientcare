using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using PatientCareWebApi.DomainModels;

namespace PatientCareWebApi.Repository.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPatientRepository : IRepository
    {
        /// <summary>
        /// Add new patient to datamock
        /// </summary>
        /// <param name="patient"></param>
        /// <returns>Id that was created for patient document</returns>
        string Add(Patient patient);

        /// <summary>
        /// Delete patient from datamock
        /// </summary>
        /// <param name="id">patient CPR</param>
        DeleteResult Delete(string id);

        /// <summary>
        /// Update existing patient
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Update Result</returns>
        UpdateResult Update(Patient patient);
    }
}
