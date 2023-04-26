using MyFitness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyFitness.Controllers
{
    public class AdminController : Controller
    {
        MyFitnessEntities _context = new MyFitnessEntities();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddProduct()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddProduct(ProductModel product)
        {
            if (ModelState.IsValid)
            {
                Product model = new Product() { 
                    PDuration = product.PDuration,
                    PPrice = product.PPrice,
                    PDesc = product.PDesc,
                };
                _context.Products.Add(model);
                _context.SaveChanges();
            }
            return View();
        }
    }
}