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


        public ActionResult Edit(int id)
        {
            var product = _context.Products.FirstOrDefault(y=>y.PId==id);
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(int id,ProductModel model)
        {
            var product = _context.Products.FirstOrDefault(y => y.PId == id);
            product.PDesc = model.PDesc;
            product.PPrice = model.PPrice;
            product.PDuration = model.PDuration;
            _context.SaveChanges();
            return View(product);
        }
        public ActionResult Show()
        {
            var products = _context.Products.ToList();
            return View(products);
        }
    }
}