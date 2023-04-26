using MyFitness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyFitness.Controllers
{
    public class HomeController : Controller
    {
        MyFitnessEntities _context = new MyFitnessEntities();

        public ActionResult Index()
        {
            var products = _context.Products.OrderByDescending(y=>y.PId).ToList();
            return View(products);
        }

        public ActionResult Product(int id)
        {
            var product = _context.Products.FirstOrDefault(y=> y.PId==id);
            return View(product);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}