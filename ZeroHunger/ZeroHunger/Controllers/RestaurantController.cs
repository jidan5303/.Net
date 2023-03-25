using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeroHunger.CustomValidation;
using ZeroHunger.EF;

namespace ZeroHunger.Controllers
{
    [Logged]
    public class RestaurantController : Controller
    {
        // GET: Restaurant
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Add() 
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(Food Model)
        {
            var db = new ZeroHungerEntities5();
            db.Foods.Add(Model);
            db.SaveChanges();
            return RedirectToAction("Details");
        }
        public ActionResult Details()
        {
            var name = Session["Rname"];
            var db = new ZeroHungerEntities5();
            var detail = (from e in db.Restaurants
                          where e.RestaurantName == name.ToString()
                          select e).SingleOrDefault();
            var fd = (from e in db.Foods
                      where e.RID == detail.RestaurantID
                      select e).ToList();

            return View(fd);
        }
    }
}