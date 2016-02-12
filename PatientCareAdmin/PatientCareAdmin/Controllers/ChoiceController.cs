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
    public class ChoiceController : Controller
    {
        private readonly ILog _log;
        private readonly HttpHandler<ChoiceModel> _handler;
        private readonly List<CategoryModel> _categories;
        private List<DetailModel> _details;

        public ChoiceController()
        {
            _log = new Logger("PatientCareAdmin : ChoiceController");
            var client = new HttpClient();
            _handler = new HttpHandler<ChoiceModel>(client);
            _handler.Uri = "api/choice";
            _categories = GetCategories();
            _details = GetDetails();
        }
        // GET: Choice
        public ActionResult Index()
        {
            try
            {// TODO: Mangler error handling på om der er adgang til data eller ej
                List<ChoiceModel> choiceList = new List<ChoiceModel>();

                var query = _handler.Get();
                if (query == null)
                {
                    return View(choiceList);
                }
                foreach (var item in query)
                {
                    var detailList = new List<DetailModel>();

                    detailList.AddRange(item.Details);

                    choiceList.Add(new ChoiceModel()
                    {
                        ChoiceId = item.ChoiceId,
                        Category = _categories.First(k => k.CategoryId == item.CategoryId).Name,
                        Name = item.Name,
                        Details = detailList
                    });
                }
                _log.Debug("Get all Choices from Mongo");

                return View(choiceList); 
            }
            catch (Exception ex)
            {
                _log.Exception(ex.Message + ex.InnerException);
                return View();
            }
        }

        // GET: Choice/Create
        public ActionResult Create()
        {
            //TODO: Det skal ikke være muligt at tilføje nyt valg hvis der ikke er valgt en kategori
            ViewBag.CategoryID = new SelectList(_categories, "CategoryId", "Name", _categories.Where( c => c.CategoryId == "00000"));
            ViewBag.DetailID = new SelectList(_details, "DetailId", "Name", _details.Where(d => d.DetailId == "000000"));
            return View();
        }

        // POST: Choice/Create
        [HttpPost]
        public ActionResult Create(ChoiceModel choiceModel)
        {
            try
            {
                if (choiceModel.Name != null)
                {
                    if (ModelState.IsValid)
                    {
                        char delimiter = ',';

                        var detailsList = choiceModel.DetailsList.Split(delimiter).ToList();

                        var ListDetails = new List<DetailModel>();

                        foreach (var item in detailsList)
                        {
                            if (item.Length > 0)
                            {
                               ListDetails.Add(new DetailModel()
                               {
                                   Name = _details.Find(x => x.Name == item).Name,
                                   DetailId = _details.Find(x => x.Name == item).DetailId
                               });
                            }
                        }

                        var newchoice = new ChoiceModel()
                        {
                            ChoiceId = choiceModel.ChoiceId,
                            Category = choiceModel.Category,
                            CategoryId = choiceModel.CategoryId,
                            Name = choiceModel.Name,
                            Details = ListDetails,
                        };

                        var choice = Newtonsoft.Json.JsonConvert.SerializeObject(newchoice);
                        if (choice != null)
                        {
                            var response = _handler.Post(choice);
                            if (response.StatusCode == 201)
                            {
                                _log.Debug("Answer from Web API: " + response.StatusCode + response.StatusDescription);
                            }
                        }
                    }
                    return RedirectToAction("Index");
                }
                _log.Debug("No 'Navn' added to new choice or no detail was selected, exiting");
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                _log.Exception(ex.Message + ex.InnerException);
                return RedirectToAction("Index");
            }
        }

      // POST: Choice/Delete/5
        public ActionResult Delete(string id)
        {
            try
            {
                var response = _handler.Delete(id);
                if (response != null)
                {
                    _log.Debug("Answer from Web API: " + response.StatusCode + " " + response.StatusDescription);
                    return RedirectToAction("Index");
                }
                _log.Debug("Choice was not deleted");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        private List<CategoryModel> GetCategories()
        {
            var client = new HttpClient();
            var handler = new HttpHandler<CategoryModel>(client);
            handler.Uri = "api/category";
            var categories = handler.Get();

            var List = new List<CategoryModel>();

            List.Add(new CategoryModel()
            {
                CategoryId = "000000",
                Name = "Ingen kategori valgt"
            });
            
            if (categories != null)
            {
                if (categories.Count > 0)
                {
                    foreach (var item in categories)
                    {
                        List.Add(new CategoryModel()
                        {
                            CategoryId = item.CategoryId,
                            Name = item.Name
                        });
                    }
                }
            }
            else
            {
                List.Add(new CategoryModel()
                {
                    CategoryId = "00000",
                    Name = "Ingen kategorier fundet"
                });
            }

            return List;
        }

        private List<DetailModel> GetDetails()
        {
            var client = new HttpClient();
            var handler = new HttpHandler<DetailModel>(client);
            handler.Uri = "api/detail";
            var details = handler.Get();

            var List = new List<DetailModel>();

            List.Add(new DetailModel()
            {
                DetailId = "000000",
                Name = "Ingen tilbehør valgt",
            });

            if (details != null)
            {
                if (details.Count > 0)
                {
                    foreach (var item in details)
                    {
                        List.Add(new DetailModel()
                        {
                            DetailId = item.DetailId,
                            Name = item.Name,
                        });
                    }
                }
            }
            
            return List;
        } 
    }
}
