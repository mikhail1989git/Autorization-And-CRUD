using AutorizationAndCRUD.Models;
using AutorizationAndCRUD.SessionTools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace AutorizationAndCRUD.Controllers
{
    public class ProductsController : Controller
    {
        ApplicationContext db;
        SessionTool st;
        public ProductsController(ApplicationContext db)
        {
            this.db = db;
            this.st = new SessionTool(db);
        }

        public async Task<IActionResult> ProductsList()
        {
            if (!st.IsActiveSession(HttpContext.Session))
                return RedirectToAction("Index", "Home");

            return View(await db.Products.ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!st.IsActiveSession(HttpContext.Session))
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (!st.IsActiveSession(HttpContext.Session))
                return RedirectToAction("Index", "Home");

            db.Products.Add(product);
            await db.SaveChangesAsync();
            return RedirectToAction("ProductsList");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (!st.IsActiveSession(HttpContext.Session))
                return RedirectToAction("Index", "Home");

            if (id != null)
            {
                Product? product = await db.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (product != null) return View(product);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            if (!st.IsActiveSession(HttpContext.Session))
                return RedirectToAction("Index", "Home");

            db.Products.Update(product);
            await db.SaveChangesAsync();
            return RedirectToAction("ProductsList");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (!st.IsActiveSession(HttpContext.Session))
                return RedirectToAction("Index","Home");

            if (id != null)
            {
                Product? human = await db.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (human != null)
                {
                    db.Products.Remove(human);
                    await db.SaveChangesAsync();
                    return RedirectToAction("ProductsList");
                }
            }
            return NotFound();
        }
    }
}
