using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using Logging;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using PatientCareWebApi.DomainModels;
using PatientCareWebApi.Models;

namespace PatientCareWebApi.Controllers
{
    /// <summary>
    /// 
    ///  </summary>
    public class CategoryController : ApiController
    {
        private readonly IMongoCollection<BsonDocument> _categories;
        private readonly IMongoDatabase _db;
        private readonly ILog _log;
        /// <summary>
        /// CategoryController constructor
        /// </summary>
        public CategoryController()
        {
            var client = new MongoClient(ConfigurationManager.ConnectionStrings["Mongo_patientcare"].ConnectionString);
            _log = new Logger("WebAPI:CategoryController");
            _db = client.GetDatabase("patientcaredb");
            _categories = _db.GetCollection<BsonDocument>("Category");
        }
        /// <summary>
        /// Get all Categories from mongoDB
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public List<Category> GetAllCategories()
        {
            try
            {
                var categoryList = new List<Category>();
                var categories =
                    _categories.Find(new BsonDocument()).ToListAsync().Result;

                foreach (var item in categories)
                {
                    //categoryList.Add(BsonSerializer.Deserialize<Category>(item));
                    categoryList.Add(new Category()
                    {
                        CategoryId = item.Values.ToArray()[0].ToString(),
                        Name = item.Values.ToArray()[1].ToString(),
                        Picture = item.Values.ToArray()[2].ToString()
                    });
                }

                return categoryList;
            }
            catch (Exception ex)
            {
                _log.Exception(ex.Message + ex.InnerException);
                throw;
            }
        }

        /// <summary>
        /// Get specific category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public Category Get(string id)
        {
            try
            {
                var category = _categories.Find(Builders<BsonDocument>.Filter.Eq("_id", id)).SingleAsync().Result;

                var newCategory = new Category
                {
                    CategoryId = category.Values.ToArray()[0].ToString(),
                    Name = category.Values.ToArray()[1].ToString(),
                    Picture = category.Values.ToArray()[2].ToString()
                };

                return newCategory;
            }
            catch (Exception ex)
            {
                _log.Exception(ex.Message + ex.InnerException);
                throw;
            }
        }

        /// <summary>
        /// Create new category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public ActionResult Add(Category category)
        {
            try
            {
                category.CategoryId = ObjectId.GenerateNewId().ToString();

                var cat = _categories.InsertOneAsync(category.ToBsonDocument());
                cat.Wait();

                if (category.CategoryId != null)
                {
                 return new HttpStatusCodeResult(HttpStatusCode.Created, "New Category was added with name: " + category.Name);   
                }
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No new category was added");
            }
            catch (Exception ex)
            {
                _log.Exception(ex.Message + ex.InnerException);
                throw;
            }
        }

        /// <summary>
        /// Delete a category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public ActionResult Delete(string id)
        {
            try
            {
                var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
                var deletedCategory = _categories.DeleteOneAsync(filter).Result;
                if(deletedCategory.IsAcknowledged && deletedCategory.DeletedCount > 0)
                return new HttpStatusCodeResult(HttpStatusCode.OK, "Category with id: " + id + " was deleted");

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Category was not deleted");
            }
            catch (Exception ex)
            {
                _log.Exception(ex.Message + ex.InnerException);
                throw;
            }
        }

        ///// <summary>
        ///// Update a category
        ///// </summary>
        ///// <param name="categoryModel"></param>
        ///// <returns></returns>
        //[System.Web.Http.HttpPut]
        //public ActionResult Put(Category categoryModel)
        //{
        //    try
        //    {
        //        var filter = Builders<BsonDocument>.Filter.Eq("_id", categoryModel.CategoryId);
        //        var update = Builders<BsonDocument>.Update.Set("Name", categoryModel.Name)
        //            .Set("Picture", categoryModel.Picture);

        //        var updatedCategory = _categories.UpdateOneAsync(filter, update).Result;

        //        if(updatedCategory.IsAcknowledged && updatedCategory.ModifiedCount > 0)
        //            return new HttpStatusCodeResult(HttpStatusCode.Accepted, "Category was updated id: " + categoryModel.CategoryId);

        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Category was not updated id: " + categoryModel.CategoryId);
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.Exception(ex.Message + ex.InnerException);
        //        throw;
        //    }
        //}
    }
}
