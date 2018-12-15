using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCASS.Models;
using MVCASS.Helpers;
using MVCASS.Context;

namespace MVCASS.Controllers
{
 
    public class MyAccountController : Controller
    {

        UserContext db = new UserContext();
        // GET: MyAccount
        public ActionResult Index()
        {
           
            return View(db.Users.ToList());
        }

        [HttpPost] public ActionResult Signin(User user)
        {
         
            if (ModelState.IsValid)
            {
                User u = new User();
                u.email = user.email;
                u.password = Helper.MyDecrypt(user.password);
                ViewBag.user = u;
                UserContext dbset = new UserContext();
                if (dbset.Login(u.email, Helper.MyDecrypt(user.password))==true){
                    //return View();
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }

            }
                return RedirectToAction("Index", "Home");
          
            
        }
        [HttpGet] public ActionResult Signout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult Signup()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }
        [HttpPost]
        public ActionResult SignupDB(User user)
        {
        
            if (ModelState.IsValid)
            {
                User u = new User();
                u.username = user.username;
                u.email = user.email;
                u.password = Helper.MyEncrypt(user.password);
                ViewBag.user = u;

                db.AddUser(u);
                return RedirectToAction("Index", "Dashboard");
            }
            return RedirectToAction("Signup", "MyAccount");
        }
    }
}