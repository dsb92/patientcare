using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Driver;
using PatientCareWebApi.DomainModels;

namespace PatientCareWebApi.Repository.Interfaces
{
    public interface IChoiceRepository : IRepository
    {
        Choice Add(Choice choice);
        DeleteResult Delete(string id);

    }
}