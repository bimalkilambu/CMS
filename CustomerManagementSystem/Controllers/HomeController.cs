using CustomerManagementSystem.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CustomerManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Customers()
        {
            ViewBag.Message = "Your contact page by bimal.";
            CMSEntities entities = new CMSEntities();
            List<Customer> customers = entities.Customers.Where(c => !c.DateDeleted.HasValue).ToList();

            return View(customers);
        }
    }
}