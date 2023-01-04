using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;

namespace Frontend.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }
        public ActionResult Login()
        {
            return View();
        }
        public IActionResult Submit(UserModel model)
        {
            model.UserType=CheckLogin(model).Result;
            if(model.UserType!=string.Empty)
            {
                HttpContext.Session.SetString("type",model.UserType);
                return RedirectToAction("Form","Form");
            }
            return View();
        }

        public async Task<string> CheckLogin(UserModel model)
        {
            if (model.UserName != string.Empty && model.Password != string.Empty)
            {
                using (var client = new HttpClient())
                {
                    using (var response = await client.GetAsync("http://localhost:5047/api/user/" + model.UserName + "/" + model.Password))
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<string>(result);
                    }
                }
            }
            return string.Empty;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}