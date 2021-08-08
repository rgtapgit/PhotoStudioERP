using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhotoStudioApp.Models;

namespace PhotoStudioApp.Controllers
{
    public class AdminController : Controller
    {
        public PhotoStudioDbContext _db = new PhotoStudioDbContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        //Studio LogIn Page
        public ActionResult LogIn()
        {
            return View();
        }
        public ActionResult CheckLogin(Studio obj)
        {
            var isUserExist = _db.Studios.SingleOrDefault(m => m.studioName == obj.studioName && m.Password == obj.Password);
            if (isUserExist != null)
            {
                Session["LoggedId"] = isUserExist.studioId;
                var loggiedid = Session["LoggedId"];
                return Json(isUserExist, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Dashboard()
        {
            var loggiedid =Convert.ToInt32(Session["LoggedId"]);
            ViewBag.TotalCustomer = _db.Customers.Where(m=>m.studioId == loggiedid).Count();
            ViewBag.TotalOrder = _db.Orders.Where(m => m.studioId==loggiedid).Count();
            ViewBag.TotalAmount = Convert.ToInt32(_db.Orders.Where(m => m.studioId == loggiedid).Sum(m=> m.totalCost));
            ViewBag.BalanceAmount = _db.Payments.Where(m => m.studioId == loggiedid).Sum(m => m.balanceAmount);
            return View();
        }

        public ActionResult Admin()
        {
            var loggiedid = Convert.ToInt32(Session["LoggedId"]);
            ViewBag.UserName = _db.Studios.SingleOrDefault(m => m.studioId == loggiedid).studioName;
            return View();
        }

        //Admin SignIn Page
        public ActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CheckSignIn(string UserName, string Password)
        {
            
            if (UserName == "rahul1" && Password == "1234")
            {
                Session["Logged_Id"] = UserName;
                var loggiedid = Session["Logged_Id"];
                return RedirectToAction("index", "Studio");
            }
            else
            {
                return View("SignIn");
            }
            
        }
        
        //dataTable by jquery
        public ActionResult OrderList(string sEcho, int iDisplayStart, int iDisplayLength, DateTime? fromdate, DateTime? toDate, string Company, string Catagory, string Model, string SerialNo)

        {
            string sSearchValue = string.Empty;
            IEnumerable<Order> _Type = _db.Orders;
            sSearchValue = Request["sSearch"];
            var sortdirection = Request["sSortDir_0"];
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            orderSequence oMemberOrder = (orderSequence)sortColumnIndex;
            string sSortColumn = oMemberOrder.ToString();
            int totalRecords = 0;
            var _MemberListing = (from member in _db.Orders
                                  where (sSearchValue == "" || member.OrderDetails.Contains(sSearchValue))
                                  select member).OrderByDescending(c => c.OrderNo).AsEnumerable().Select((member, index) => new
                                  {
                                      Index = index + 1,
                                      OrderNo = member.OrderNo,
                                      OrderDetails = member.OrderDetails,
                                      orderDate = member.orderDate,
                                      totalCost = member.totalCost,
                                  }).ToList();

            if (sSearchValue != null && sSearchValue != "")
            {
                totalRecords = _MemberListing.Count();
            }
            else
            {
                totalRecords = _MemberListing.Count();
            }
            var _MemberQuery = _MemberListing.AsEnumerable().ToList().Select(member => new[]
                 {
                    member.Index.ToString(),
                    member.OrderNo.ToString(),
                    member.OrderDetails,
                    member.orderDate,
                    member.totalCost.ToString()
             }).Skip(iDisplayStart).Take(iDisplayLength);

            var _Json_Memberquery = new
            {
                sEcho = sEcho,
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = totalRecords,
                aaData = _MemberQuery
            };
            return Json(_Json_Memberquery, JsonRequestBehavior.AllowGet);
        }

        public ActionResult List()
        {
            var ordList = _db.Orders.ToList();
            return View(ordList);
        }
    }
}
