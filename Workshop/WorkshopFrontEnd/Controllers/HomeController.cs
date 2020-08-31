using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WorkshopFrontEnd.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewCounter()
        {
            if (Session["Count"] == null)
            {
                Session["Count"] = 0;
            }

            int i = (int)Session["Count"];
            i++;
            Session["Count"] = i;

            ViewBag.Count = Session["Count"];

            return View();
        }

        public ActionResult ApiClient()
        {
            ViewBag.Message = "API Client";

            return View();
        }
    }
}