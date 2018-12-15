using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCASS.Helpers;

namespace MVCASS.Controllers
{
    [SessionAuthorize]
    public class DashboardController : Controller
    {
        public DashboardController()
        {
           
        }
        // GET: Dashboard
        public ActionResult Index()
        {
            if (Helper.checkSession())
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
    }
}