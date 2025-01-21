using System.Diagnostics;
using System.Runtime.CompilerServices;
using AutorizationAndCRUD.Models;
using AutorizationAndCRUD.SessionTools;
using Microsoft.AspNetCore.Mvc;

namespace AutorizationAndCRUD.Controllers
{
    public class HomeController : Controller
    {
        ApplicationContext db;
        SessionTool st;

        public HomeController(ApplicationContext db)
        {
            this.db = db;
            this.st = new SessionTool(db);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(User user)
        {
            var userfromDb = db.Users.Where(item => item.Login == user.Login && item.Password == user.Password).FirstOrDefault();
            if (userfromDb!=null)
            {
                st.CreateSession(userfromDb, HttpContext);
                return RedirectToAction("ProductsList", "Products");  
            }

            return RedirectToAction(actionName:"Error", routeValues: new { error = "Incorrect parameters" });
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(User user)
        {
            if (db.Users.Where(item => item.Login == user.Login).Any())
            {
                return RedirectToAction(actionName: "Error", routeValues: new { error = "User is exist"});
            }

            db.Users.Add(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Error(string error)
        {
            ViewBag.Error = error;
            return View();
        }
    }

}
