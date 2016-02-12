using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using PatientCareAdmin.Models;
using Logging;


namespace PatientCareAdmin.Controllers
{
    public class ConfigController : Controller
    {
        private readonly ILog _log;

        public ConfigController()
        {
            _log = new Logger("PatientCareAdmin:ConfigController");
        }

        public ActionResult Index()
        {
            ViewData["Personale"] = new PersonaleModel();
            return View();
        }

        public ActionResult AddDepartment()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPersonale([Bind(Include = "Navn,Funktion")]PersonaleModel personaleModel, string afd)
        {
            //    try
            //    {
            //        if (personaleModel.Navn != null || personaleModel.Funktion != null)
            //        {
            //            if (ModelState.IsValid)
            //            {
            //                var newPersonale = new Personale
            //                {
            //                    PersonaleID = Guid.NewGuid(),
            //                    Navn = personaleModel.Navn,
            //                    Funktion = personaleModel.Funktion,
            //                };
            //            }
            //            return RedirectToAction("AddPersonale");
            //        }
            //        _log.Debug("No 'Navn' or 'Funktion' added to the Personale. Exiting");
            //        return RedirectToAction("AddPersonale");
            //    }
            //    catch (Exception ex)
            //    {
            //        _log.Exception(ex.Message);
            //        return RedirectToAction("AddPersonale");
            //    }
            return View();
        }

        public ActionResult AddPersonale()
        {
            //try
            //{
            //    List<PersonaleModel> personaleList = new List<PersonaleModel>();
            //    var query = from p in _dataMock.Personales
            //                join afd in _dataMock.Afdelings on p.AfdelingID equals afd.AfdelingID
            //                orderby p.Funktion
            //                select new
            //                {
            //                    PersonaleId = p.PersonaleID,
            //                    PersonaleNavn = p.Navn,
            //                    PersonaleFunktion = p.Funktion,
            //                    Afdeling = afd.Forkortelse
            //                };
            //    foreach (var item in query)
            //    {
            //        personaleList.Add(new PersonaleModel
            //        {
            //            Navn = item.PersonaleNavn,
            //            Funktion = item.PersonaleFunktion,
            //            Afdeling = item.Afdeling
            //        });
            //    }

            //    List<SelectListItem> afdList = new List<SelectListItem>();
            //    var afdQuery = from afd in _dataMock.Afdelings
            //                   orderby afd.Forkortelse
            //                   select new
            //                   {
            //                       Id = afd.Forkortelse,
            //                       Name = afd.Navn
            //                   };

            //    foreach (var item in afdQuery)
            //    {
            //        afdList.Add(new SelectListItem
            //        {
            //            Text = item.Name,
            //            Value = item.Id
            //        });
            //    }

            //    _log.Debug("Henter alt Personale");
            //    ViewData["afd"] = afdList;
            //    ViewData["PersonaleData"] = personaleList;
            //    return View();
            //}
            //catch (Exception Ex)
            //{
            //    _log.Debug(Ex.Message);
            //    throw;
            //}
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddChoice(ChoiceModel choiceModel, string cat)
        {
            //try
            //{
            //    if (choiceModel.Name != null)
            //    {
            //        if (ModelState.IsValid)
            //        {
            //            var newChoice = new Choice
            //            {
            //                ChoiceId = Guid.NewGuid(),
            //                CategoryId = choiceModel.CategoryId,
            //                Name = choiceModel.Name
            //            };
            //            _log.Debug("New Choice added: " + newChoice.Name);
            //        }
            //        return RedirectToAction("AddChoice");
            //    }
            //    _log.Debug("No 'Navn' added to new choice, exiting");
            //    return RedirectToAction("AddChoice");
            //}
            //catch (Exception ex)
            //{
            //    _log.Exception(ex.Message);
            //    return RedirectToAction("AddChoice");
            //}
            return View();
        }
        //public ActionResult AddChoice()
        //{
        //    try
        //    {
        //        List<ChoiceModel> choiceList = new List<ChoiceModel>();
        //        var query = from choice in _context.Choices
        //                    join cat in _context.Categories on choice.CategoryId equals cat.CategoryId
        //                    select new
        //                    {
        //                        choice.ChoiceId,
        //                        catName = cat.Name,
        //                        choice.Name
        //                    };

        //        foreach (var item in query)
        //        {
        //            choiceList.Add(new ChoiceModel
        //            {
        //                ChoiceId = item.ChoiceId,
        //                Name = item.Name,
        //                Category = item.catName
        //            });
        //        }

        //        List<SelectListItem> catList = new List<SelectListItem>();
        //        var catQuery = from category in _context.Categories
        //                       select new
        //                       {
        //                           Id = category.CategoryId,
        //                           category.Name
        //                       };

        //        foreach (var item in catQuery)
        //        {
        //            catList.Add(new SelectListItem
        //            {
        //                Text = item.Name,
        //                Value = item.Id.ToString()
        //            });
        //        }

        //        _log.Debug("Get All choices for Config Page");
        //        ViewData["choiceList"] = choiceList;
        //        ViewData["catList"] = catList;
        //        return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.Exception(ex.Message);
        //        return View();
        //    }
        //}

        public ActionResult AddDetail()
        {

            return null;
        }

        public ActionResult Remove(Guid Id)
        {
            //var query = (from category in _context.Categories
            //             where category.CategoryId == cat.CategoryId
            //             select category).First();

            //if (query != null)
            //{
            //    _context.Categories.DeleteOnSubmit(query);
            //    _context.SubmitChanges();
            //    _log.Debug("Succesfully deleted category: " + query.Name);
            //}
            //else
            //{
            //    _log.Debug("Couldn't find Category to delete. Quiting");
            //}

            //return RedirectToAction("Category");
            return new EmptyResult();
        }

        //private Guid GetDepartmentId(string forkortelse)
        //{
        //    var query = (from afd in _dataMock.Afdelings where afd.Forkortelse == forkortelse select afd.AfdelingID).FirstOrDefault();

        //    return query;
        //}

    }
}