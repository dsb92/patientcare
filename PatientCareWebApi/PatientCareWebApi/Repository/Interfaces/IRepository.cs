using System.Collections.Generic;
using MongoDB.Bson;

namespace PatientCareWebApi.Repository.Interfaces
{
   /// <summary>
   /// Base interface for all Repostories
   /// </summary>
    public interface IRepository
    {
        List<BsonDocument> GetAll();
        BsonDocument Get(string id);
    }
}