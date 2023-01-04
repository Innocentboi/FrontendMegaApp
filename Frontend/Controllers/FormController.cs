using Frontend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace Frontend.Controllers
{
    public class FormController : Controller
    {
        // GET: FormController
        public ActionResult Form()
        {
            init();
            if (ViewBag.type == null) return RedirectToAction("Login", "Login");
            return View();
        }
        private void init()
        {
            ViewBag.type = HttpContext.Session.GetString("type");
            var locations = (from loc in GetLocations().Result
                             select new SelectListItem()
                             {
                                 Text = loc.LocationName,
                                 Value = loc.LocationId
                             }).ToList();
            ViewBag.locations = locations;
        }
        public async Task<List<MsStorageLocationModel>> GetLocations()
        {
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync("http://localhost:5047/api/location/"))
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<MsStorageLocationModel>>(result);
                }
            }
        }
        // GET: FormController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FormController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FormController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(BpkbModel model,string userType)
        {
            try
            {
                bool isSaved = commitSave(model, userType).Result;
                if(isSaved)
                {
                    ViewBag.message = "Data Saved Successfully!";
                }
                else
                {
                    ViewBag.message = "Failed to Save Data!";
                }
                init();
                return View("Form");
            }
            catch(Exception ex)
            {
                ViewBag.message = ex.Message;
                init();
                return View("Form");
            }
        }
        public async Task<bool> commitSave(BpkbModel model, string userType)
        {
            string trans="";
            if (userType == "001")
            {
                trans = "Bpkb1";
            }
            else if (userType == "002")
            {
                trans = "Bpkb2";
            }
            using (var client = new HttpClient())
            {
                StringContent param = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync("http://localhost:5047/api/" + trans, param))
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<bool>(result);
                }
            }
        }
        // GET: FormController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FormController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FormController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FormController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
