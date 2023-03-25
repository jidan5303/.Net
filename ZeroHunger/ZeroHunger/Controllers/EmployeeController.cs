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
    public class EmployeeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        // GET: Employee
        public ActionResult OfferedList()
        {
            var name = Session["username"];
            var db = new ZeroHungerEntities5();
            var detail = (from e in db.Assigneds
                       where e.Employee.Name == name && e.Status.Equals("Confirmed")
                          select e).ToList();            
            return View(detail);
        }
        public ActionResult Proceed(int id)
        {
            var db = new ZeroHungerEntities5();
            var ad = (from a in db.Assigneds
                      where a.ID == id
                      select a).SingleOrDefault();
            ad.Status = "Proceeding";
            db.SaveChanges();

            var tr = new Track();
            tr.ReqID = ad.ID;
            tr.Status = "Proceeding";
            db.Tracks.Add(tr);
            db.SaveChanges();

            return View(ad);
        }
        public ActionResult Delivered(int id)
        {
            var db = new ZeroHungerEntities5();
            var ad = (from a in db.Assigneds
                      where a.ID == id
                      select a).SingleOrDefault();
            ad.Status = "Delivered";

            var tr = new Track();
            tr.ReqID = ad.ID;
            tr.Status = "Delivered";
            db.Tracks.Add(tr);
            db.SaveChanges();

            //var fd = (from f in db.Foods
            //          where f.FoodID==ad.FoodID
            //          select f).SingleOrDefault();
            //db.Foods.Remove(fd);
            //db.SaveChanges();



            //db.Assigneds.Remove(ad);
            //db.SaveChanges();

            TempData["msg"] = "Confirmation sent";
            return RedirectToAction("Index");
        }
    }
}