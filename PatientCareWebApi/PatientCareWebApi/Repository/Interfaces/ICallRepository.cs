using MongoDB.Driver;
using PatientCareWebApi.Models;

namespace PatientCareWebApi.Repository.Interfaces
{
    /// <summary>
    /// Adding speficic functions for CallRepository
    /// </summary>
    public interface ICallRepository : IRepository
    {
        /// <summary>
        /// Add call to Database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        string Add(CreateCallModel entity);
        /// <summary>
        /// Update Existing call in Database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        UpdateResult Update(UpdateCallModel entity);
    }
}
