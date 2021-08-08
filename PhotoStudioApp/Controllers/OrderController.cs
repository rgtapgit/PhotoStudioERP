using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhotoStudioApp.Models;

namespace PhotoStudioApp.Controllers
{
    public class OrderController : Controller
    {
        public PhotoStudioDbContext _db=new PhotoStudioDbContext();
        // GET: Order
        public ActionResult Index()
        {
            if (Session["LoggedId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("login", "admin");
            }
        }

        public ActionResult List()
        {
            var loggiedid = Convert.ToInt32(Session["LoggedId"]);
            var orderList = (from order1 in _db.Orders
                              join customer1 in _db.Customers on order1.customerId equals customer1.customerId
                              where order1.studioId==loggiedid
                              select new OrderViewModel
                              {
                                  orderId = order1.orderId,
                                  customerId=order1.customerId,
                                  OrderNo=order1.OrderNo,
                                  orderDate=order1.orderDate,
                                  OrderDetails=order1.OrderDetails,
                                  customerName=customer1.customerName,
                                  Contact=customer1.Contact,
                                  Address=customer1.Address,
                                  emailId=customer1.emailId,
                                  totalCost=order1.totalCost,
                                  studioId= loggiedid
                              }).ToList();
            return View(orderList);
        }

        public JsonResult getCustomerInfo( int id)
        {
            var customer = _db.Customers.SingleOrDefault(m =>m.customerId==id);
            if (customer != null)
            {
                return Json(customer, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult getOrderByCustId(int id)
        {
            var order = _db.Orders.Where(m => m.customerId == id).ToList();
            if (order != null)
            {
                return View(order);
            }
            else
            {
                return View();
            }

        }


        public ActionResult PlaceOrder()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PlaceOrder(OrderViewModel obj)
        {
            //ViewBag.Customer = new SelectList(_db.Customers.ToList(), "customerId", "customerId");
            var loggiedid = Convert.ToInt32(Session["LoggedId"]);
            Order _order = new Order()
            {
                orderId = obj.orderId,
                customerId = obj.customerId,
                OrderNo = obj.OrderNo,
                OrderDetails = obj.OrderDetails,
                orderDate = obj.orderDate,
                totalCost = obj.totalCost,
                studioId = loggiedid
            };
            _db.Orders.Add(_order);
            _db.SaveChanges();
            Customer _customer = new Customer()
            {
                customerId=obj.customerId,
                customerName = obj.customerName,
                Contact = obj.Contact,
                Address = obj.Address,
                emailId = obj.emailId,
                studioId=loggiedid
            };
            _db.Customers.Add(_customer);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            ViewBag.Customer = new SelectList(_db.Customers.ToList(), "customerId", "customerId");

            var record = _db.Orders.SingleOrDefault(m => m.orderId == id);
           
            if (record==null)
            {
                return RedirectToAction("Index");
            }
                return View(record);

        }

        [HttpPost]
        public ActionResult Edit(Order obj)
        {
            ViewBag.Customer = new SelectList(_db.Customers.ToList(), "customerId", "customerId");

            var record = _db.Orders.SingleOrDefault(m => m.orderId == obj.orderId);

            if (record != null)
            {
                record.customerId = obj.customerId;
                record.orderId = obj.orderId;
                record.OrderNo = obj.OrderNo;
                record.orderDate = obj.orderDate;
                record.OrderDetails = obj.OrderDetails;
                record.studioId = obj.studioId;
                record.totalCost = obj.totalCost;
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        //public JsonResult getOrderbyCustomerId(int id)
        //{
        //    var order = _db.Orders.SingleOrDefault(m => m.customerId == id);
        //    if (order != null)
        //    {
        //        return Json(order, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json("", JsonRequestBehavior.AllowGet);
        //    }

        //}

        public ActionResult Delete(int id)
        {
            var recordOrder = _db.Orders.SingleOrDefault(m => m.orderId == id);
            _db.Orders.Remove(recordOrder);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        //To download list of customer in excel
        public ActionResult DuedateExportList()
        {
            var _Daily = _db.Orders.ToList();

            if (_Daily.Count != 0)
            {
                var _Dailyquery = _Daily.Select(daily => new
                {
                    Name = daily.OrderNo,
                    Contact = daily.orderDate,
                    Address = daily.OrderDetails,
                    EmailId = daily.totalCost,
                }).ToList();

                System.Web.UI.WebControls.GridView gv = new System.Web.UI.WebControls.GridView();

                gv.DataSource = _Dailyquery;
                gv.DataBind();


                gv.HeaderRow.Cells[0].Text = "Order No.";
                gv.HeaderRow.Cells[1].Text = "Date";
                gv.HeaderRow.Cells[2].Text = "Order Details";
                gv.HeaderRow.Cells[3].Text = "Total Cost";

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=OrderExcel.xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                System.IO.StringWriter sw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
                return RedirectToAction("DueDate");
            }
            else
            {
                return RedirectToAction("DueDate");
            }
        }

        public ActionResult Report()
        {
            ViewBag.MobileNo = new SelectList(_db.Customers.ToList(), "customerId", "Contact");
            return View();
        }

        public JsonResult getCustomerByMobNo(string mob)
        {
            var customer = _db.Customers.SingleOrDefault(m => m.Contact == mob);
            if (customer != null)
            {
                return Json(customer, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }

        }
    }
}