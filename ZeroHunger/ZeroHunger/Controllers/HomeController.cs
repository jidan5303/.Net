using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeroHunger.CustomValidation;
using ZeroHunger.EF;
using ZeroHunger.Models;

namespace ZeroHunger.Controllers
{
    [Logged]
    public class HomeController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(Login Model)
        {
            if (ModelState.IsValid)
            {
                ZeroHungerEntities5 db = new ZeroHungerEntities5();
                var user = (from u in db.Users
                            where u.UserName.Equals(Model.Username)
                            && u.Password.Equals(Model.Password)
                            select u).SingleOrDefault();
                if (user != null && user.Type.Equals("User"))
                {
                    Session["Rname"] = Model.Username;
                    Session["user"] = user;
                    var returnUrl = Request["ReturnUrl"];
                    if (returnUrl != null)
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Restaurant");
                }
                else if (user != null && user.Type.Equals("Admin"))
                {
                    Session["user"] = user;
                    var returnUrl = Request["ReturnUrl"];
                    if (returnUrl != null)
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("About", "Home");
                }
                else if (user != null && user.Type.Equals("Employee"))
                {
                    Session["user"] = user;
                    Session["username"] = Model.Username;
                    var returnUrl = Request["ReturnUrl"];
                    if (returnUrl != null)
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Employee");
                }
                TempData["msg"] = "Username or Password is Invalid";
            }
            return View(Model);
        }
        public ActionResult RestaurantDetail()
        {
            ZeroHungerEntities5 db = new ZeroHungerEntities5();
            return View(db.Restaurants.ToList());
        }

        public ActionResult FoodDetail(int id)
        {
            var db = new ZeroHungerEntities5();
            var detail = (from f in db.Foods
                          where f.RID == id
                          select f).ToList();
            return View(detail);
        }
        public ActionResult AddCollection(int id)
        {
            ZeroHungerEntities5 db = new ZeroHungerEntities5();
            List<Food> collection = null;
            if (Session["collection"] == null)
            {
                collection = new List<Food>();
            }
            else
            {
                collection = (List<Food>)Session["collection"];
            }
            var fd = db.Foods.Find(id);
            collection.Add(new Food() {
                FoodID = fd.FoodID,
                Name = fd.Name,
                RID = fd.RID,
                Qty = fd.Qty,
                ExpDate = fd.ExpDate,
            });
            Session["collection"] = collection;
            TempData["msg"] = "Successfully added to collection";
            return RedirectToAction("About");
        }
        public ActionResult Collection()
        {
            var collection = (List<Food>)Session["collection"];
            if (collection != null)
            {
                return View(collection);
            }
            TempData["msg"] = "Collection empty";
            return RedirectToAction("About");
        }
        public ActionResult AssignEmp()
        {
            var db = new ZeroHungerEntities5();
            return View(db.Employees.ToList());
        }
        public ActionResult ConfirmCollection(int id)
        {
            var cart = (List<Food>)Session["collection"];
            var db = new ZeroHungerEntities5();
            foreach (var f in cart)
            {
                var ad = new Assigned();
                ad.EmpID = id;
                ad.FoodID = f.FoodID;
                ad.AssignDate = DateTime.Now;
                ad.Status = "Confirmed";
                db.Assigneds.Add(ad);

                var tr = new Track();
                tr.ReqID = ad.ID;
                tr.Status = "Confirmed";
                db.Tracks.Add(tr);
            }
            db.SaveChanges();
            Session["collection"] = null;
            TempData["msg"] = "Food Collection Confirmed Successfully";
            return RedirectToAction("About");
        }
        public ActionResult ConfirmedCollection()
        {
            var db = new ZeroHungerEntities5();
            var detail = (from p in db.Assigneds
                          where p.Status.Equals("Confirmed") || p.Status.Equals("Proceeding")
                          select p).ToList();
            return View(detail);
        }
        [Admin]
        public ActionResult Tracker()
        {
            var db = new ZeroHungerEntities5(); 
            return View(db.Tracks.ToList());
        }
        public ActionResult About()
        {
            return View();
        }
        
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            return RedirectToAction("Login");
        }
    }
}