using MyFitness.Models;
using Razorpay.Api;
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
            var products = _context.Products.OrderByDescending(y => y.PId).ToList();
            return View(products);
        }

        public ActionResult Product(int id)
        {
            var product = _context.Products.FirstOrDefault(y => y.PId == id);
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

        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Register(string name, string email, string password)
        {
            User user = new User()
            {
                UName = name,
                UEmail = email,
                UPassword = password
            };
            _context.Users.Add(user);
            _context.SaveChanges();

            ViewBag.msg = "Registration Successfull....!";
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {

            var user = _context.Users.FirstOrDefault(x => x.UEmail == email && x.UPassword == password);

            if (user != null)
            {
                Session["user"] = user.UId;
                return RedirectToAction("profile");
            }
            else
            {
                ViewBag.msg = "Login UnSuccessfull....!";
            }

            return View();
        }

        public ActionResult Profile()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("login");
            }

            int UId = Convert.ToInt32(Session["user"]);

            var user = _context.Users.FirstOrDefault(y => y.UId == UId);

            ViewBag.user = user;

            var record = _context.Records.Where(y=>y.UId == UId).ToList();
            var products = _context.Products.ToList();


            List<RecordModel> recordModels = new List<RecordModel>();

            foreach (Record record1 in record)
            {
                int temp_Pid = (int)record1.PId;
                DateTime temp_Pdate = (DateTime)record1.RDate;

                var product = products.FirstOrDefault(y=>y.PId==temp_Pid);

                DateTime day = (DateTime)record1.RDate;


                RecordModel recordModel = new RecordModel()
                {
                    PDuration = product.PDuration, // 30
                    PId = temp_Pid,
                    PDate = temp_Pdate, // 27/01/2023
                    Validitity = calulcate(day,Convert.ToInt32(product.PDuration))
                };   
                recordModels.Add(recordModel);
            }
            return View(recordModels);
        }


        public ActionResult Buy(int id)
        {
            Session["user"] = 1;
            if (Session["user"] == null)
            {
                return RedirectToAction("login");
            }

            int uid = (int)Session["user"];


            var pur = _context.Purchases.FirstOrDefault(y=>y.uid == uid && y.pid == id && y.status=="pending");

            if (pur != null)
            {
                ViewBag.orderId = pur.orderId;
                return View();
            }

            var product = _context.Products.FirstOrDefault(y=>y.PId==id);
            int price = Convert.ToInt32(product.PPrice);
            string your_key_id = "rzp_test_K3ts82hO67KSNu";
            string your_secret = "qQS6NmcieitGY8LAUkQcDhYv";
            RazorpayClient client = new RazorpayClient(your_key_id, your_secret);
            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount",  price*100); // amount in the smallest currency unit
            options.Add("currency", "INR");
            Order order = client.Order.Create(options);
            string orderId = order["id"];
            ViewBag.orderId = orderId;

            Purchase purchase = new Purchase() { 
                uid = uid,
                pid = id,
                orderId = orderId,
                status = "pending"
            };

            _context.Purchases.Add(purchase);
            _context.SaveChanges();



            return View();
        }

        public ActionResult Subscribe(int id)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("login");
            }

            Record record = new Record()
            {
                UId = Convert.ToInt32(Session["user"]),
                PId = id,
                RDate = DateTime.Now,
            };
            _context.Records.Add(record);
            _context.SaveChanges();
            return RedirectToAction("profile");
        }

        public int calulcate(DateTime date,int duration)
        {
            duration = 30 * duration;
            int days = DateTime.Now.Subtract(date).Days;
            if (duration>days)
            {
                return duration-days;
            }
            return 0;
        }
    }

    public class RecordModel
    {
        public string PDuration { get; set; }
        public DateTime PDate { get; set; }
        public int Validitity { get; set; }
        public int PId { get; set; }
    }
}