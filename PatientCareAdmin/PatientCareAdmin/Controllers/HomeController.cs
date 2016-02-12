using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using System.Web.WebSockets;
using Logging;
using PatientCareAdmin.Models;


namespace PatientCareAdmin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILog _log;
        public HomeController()
        {
            _log = new Logger("PatientCareAdmin:HomeController");
        }
        
        public ActionResult Index()
        {
            return View();
        }
        //public ActionResult Category()
        //{
        //    try
        //    {
        //        List<CategoryModel> categoryList = new List<CategoryModel>();
        //        var query = from category in _context.Categories
        //            select new
        //            {
        //                category.CategoryId,
        //                category.Name,
        //                category.Picture
        //            };

        //        foreach (var item in query)
        //        {
        //            categoryList.Add(new CategoryModel
        //            {
        //                CategoryId = item.CategoryId,
        //                Name = item.Name,
        //                Picture = item.Picture
        //            });
        //        }
        //        return View(categoryList);
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.Exception(ex.Message);
        //        throw;
        //    }
        //}

        //public ActionResult RemoveCategory(CategoryModel cat)
        //{
        //    var query = (from category in _context.Categories
        //        where category.CategoryId == cat.CategoryId
        //        select category).First();

        //    if (query != null)
        //    {
        //        _context.Categories.DeleteOnSubmit(query);
        //        _context.SubmitChanges();
        //        _log.Debug("Succesfully deleted category: " + query.Name);
        //    }
        //    else
        //    {
        //        _log.Debug("Couldn't find Category to delete. Quiting");
        //    }

        //    return RedirectToAction("Category");
        //}
    }
}