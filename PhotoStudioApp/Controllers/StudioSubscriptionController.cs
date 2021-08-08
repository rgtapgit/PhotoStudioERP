using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhotoStudioApp.Models;

namespace PhotoStudioApp.Controllers
{
    public class StudioSubscriptionController : Controller
    {
        public PhotoStudioDbContext _db = new PhotoStudioDbContext();
        // GET: StudioLogin
        public ActionResult Index()
        {
            if (Session["Logged_Id"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("SignIn", "admin");
            }
        }
        public ActionResult List()
        {
            return View();
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(StudioSubscription obj)
        {
            StudioSubscription _studioSub = new StudioSubscription()
            {
                StudioSubscriptionId = obj.StudioSubscriptionId,
                subscriptionDate = obj.subscriptionDate,
                subscriptionPeriod = obj.subscriptionPeriod,
                subscriptionAmount = obj.subscriptionAmount
            };
            _db.StudioSubscriptions.Add(_studioSub);
            _db.SaveChanges();
            return RedirectToAction("Add");
        }
    }
}