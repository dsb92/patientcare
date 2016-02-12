using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Logging;
using PatientCareAdmin.Models;

namespace PatientCareAdmin.Controllers
{
    //[Authorize]
    public class CategoryController : Controller
    {
        private readonly ILog _log;
        private HttpHandler<CategoryModel> _handler ; 
        public CategoryController()
        {
            _log = new Logger("PatientCareAdmin : CategoryController");
            var client = new HttpClient();
            _handler = new HttpHandler<CategoryModel>(client);
            _handler.Uri = "api/category";
        }
        [AllowAnonymous]
        // GET: Category
        public ActionResult Index()
        {
            try
            {
                var query = _handler.Get();
                _log.Debug("Get All categories from Web API");

                return View(query);
            }
            catch (Exception ex)
            {
                _log.Exception(ex.Message);
                return View();
            }
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryModel categoryModel)
        {
            try
            {
                if (categoryModel.Name != null || categoryModel.Picture != null)
                {
                    if (ModelState.IsValid)
                    {
                        var category = Newtonsoft.Json.JsonConvert.SerializeObject(categoryModel);
                        if (category != null)
                        {
                            var response = _handler.Post(category);
                            if (response.StatusCode == 201)
                            {
                                //At this time do nothing
                                _log.Debug("Answer from Web API: " + response.StatusCode + response.StatusDescription);
                            }
                        }
                    }
                    return RedirectToAction("Index");
                }
                _log.Debug("No 'Navn' or 'Billede' added to new category, exiting");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _log.Exception(ex.Message + ex.InnerException);
                return RedirectToAction("Index");
            }
        }

        // GET: Category/Edit/5
        public ActionResult Edit(string id)
        {
            var category = _handler.Get(id);

            return View(category);
        }

        // POST: Category/Edit/5
        [HttpPost]
        public ActionResult Edit(CategoryModel categoryModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var category = Newtonsoft.Json.JsonConvert.SerializeObject(categoryModel);

                    var response = _handler.Put(category);
                    if (response.StatusCode == 202)
                    {
                        //At this time do nothing
                        _log.Debug("Answer from Web API: " + response.StatusCode + response.StatusDescription);
                    }
                }
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                _log.Exception(ex.Message + ex.InnerException);
                return RedirectToAction("Index");
            }
        }

       // POST: Category/Delete/5
        public ActionResult Delete(string id)
        {
            try
            {
                var Response = _handler.Delete(id);
                if (Response != null)
                {
                    _log.Debug("Answer from Web API: " + Response.StatusCode + " " + Response.StatusDescription);
                    return RedirectToAction("Index");
                }
                _log.Debug("Category was not deleted");
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                _log.Exception(ex.Message + ex.InnerException);
                return RedirectToAction("Index");
            }
        }
    }
}
