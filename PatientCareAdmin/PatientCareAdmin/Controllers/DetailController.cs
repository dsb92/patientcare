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
    public class DetailController : Controller
    {
        private readonly ILog _log;
        private HttpHandler<DetailModel> _handler;
        private List<DetailModel> _details;

        public DetailController()
        {
            _log = new Logger("PatientCareAdmin : DetailController");
            var client = new HttpClient();
            _handler = new HttpHandler<DetailModel>(client);
            _handler.Uri = "api/detail";
        }

        // GET: Detail
        public ActionResult Index()
        {
            var query = _handler.Get();

            return View(query);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(DetailModel DetailModel)
        {
            try
            {
                if (DetailModel.Name != null)
                {
                    if (ModelState.IsValid)
                    {
                        var detail = Newtonsoft.Json.JsonConvert.SerializeObject(DetailModel);
                        if (detail != null)
                        {
                            var response = _handler.Post(detail);
                            if (response.StatusCode == 201)
                            {
                                _log.Debug("Answer from Web API: " + response.StatusCode + response.StatusDescription);
                            }
                        }
                    }
                    return RedirectToAction("Index");
                }
                _log.Debug("No 'Navn' added to new detail");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _log.Exception(ex.Message + ex.InnerException);
                throw;
            }
            return null;
        }

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
    }
}