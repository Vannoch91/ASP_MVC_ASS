using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCASS.Context;
using MVCASS.Models;
using System.Web.SessionState;
using System.Text;
using System.Net;
using System.Data.Entity;
using PagedList;
using PagedList.Mvc;
using System.Data;



namespace MVCASS.Controllers
{
    [SessionAuthorize]
    public class ArticleController : Controller
    {
        // GET: Article
        ArticleContext dbContext = new ArticleContext();
        public ActionResult IndexNA()
        {
            return View(dbContext.Articles.ToList());
           
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include ="title,description")] Articles article)
        {
            if (ModelState.IsValid)
            {
                /* If you want to use Raw SQL Query Pls check this comment code...
                 * Articles art = new Articles();
                art.title = article.title;
                art.description = article.description;
                art.author = Int32.Parse(Session["UID"].ToString());
                art.created_at= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                dbContext.AddNewArticle(art);
                ViewBag.article = art;
                */

                article.author = Int32.Parse(Session["UID"].ToString());
                article.created_at = DateTime.Now;
                dbContext.Articles.Add(article);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
           
            }
            return View(article);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            Articles a = dbContext.Articles.Find(id);
            dbContext.Articles.Remove(a);
            dbContext.SaveChanges();
            return RedirectToAction("Index", "Article");
        }
        public ActionResult Details(int id)
        {
            var obj = dbContext.Articles.Where(a => a.id == id).FirstOrDefault();
            if (obj == null)
            {
                return new HttpNotFoundResult();
            }
            ViewBag.id = id;
            return View(obj);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Articles obj = dbContext.Articles.Find(id);
            if (obj == null)
            {
                return HttpNotFound();
            }
            return View(obj);
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var objToUpdate = dbContext.Articles.Find(id);
            objToUpdate.author = Int32.Parse(Session["UID"].ToString());
            if (TryUpdateModel(objToUpdate, "",
               new string[] { "title", "description","author" }))
            {
                try
                {
                    dbContext.SaveChanges();

                    return RedirectToAction("Index","Article");
                }
                catch (Exception ex)
                {
                    
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(objToUpdate);
        }


        public ActionResult Index(string sortOn, string orderBy,
                        string pSortOn, string keyword, int? page)
        {
            int recordsPerPage = 10;
            if (!page.HasValue)
            {
                page = 1; // set initial page value
                if (string.IsNullOrWhiteSpace(orderBy) || orderBy.Equals("asc"))
                {
                    orderBy = "desc";
                }
                else
                {
                    orderBy = "asc";
                }
            }

            // override the sort order if the previous sort order and current
            //request sort order is different
            if (!string.IsNullOrWhiteSpace(sortOn) && !sortOn.Equals(pSortOn,
                    StringComparison.CurrentCultureIgnoreCase))
            {
                orderBy = "asc";
            }

            ViewBag.OrderBy = orderBy;
            ViewBag.SortOn = sortOn;
            ViewBag.Keyword = keyword;

            var list = dbContext.Articles.AsQueryable();

            switch (sortOn)
            {
                case "title":
                    if (orderBy.Equals("desc"))
                    {
                        list = list.OrderByDescending(a => a.title);
                    }
                    else
                    {
                        list = list.OrderBy(a => a.title);
                    }
                    break;
                case "description":
                    if (orderBy.Equals("desc"))
                    {
                        list = list.OrderByDescending(a => a.description);
                    }
                    else
                    {
                        list = list.OrderBy(a => a.description);
                    }
                    break;
                case "author":
                    if (orderBy.Equals("desc"))
                    {
                        list = list.OrderByDescending(a => a.author);
                    }
                    else
                    {
                        list = list.OrderBy(a => a.author);
                    }
                    break;
                default:
                    list = list.OrderBy(a => a.id);
                    break;
            }
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                list = list.Where(a => a.title.Contains(keyword) || a.description.Contains(keyword));
            }
            var finalList = list.ToPagedList(page.Value, recordsPerPage);
      
            return View(finalList);
        }


        public JsonResult getAllArticle()
        {
            var article = dbContext.Articles.AsQueryable();
            var startDate = Request.QueryString["sDate"];
            var endDate = Request.QueryString["eDate"];
            if (!string.IsNullOrWhiteSpace(startDate) && !string.IsNullOrWhiteSpace(endDate))
            {
                DateTime sDate = Convert.ToDateTime(startDate);
                DateTime eDate = Convert.ToDateTime(endDate);
                Console.WriteLine(sDate);
                article = article.Where(a => a.created_at >= sDate && a.created_at <= eDate);
            }
           
            var jsonData = Json(article, JsonRequestBehavior.AllowGet);
            return jsonData;
        }

    
    }
}