using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhotoStudioApp.Models;
using System.Net;

namespace PhotoStudioApp.Controllers
{
    public class PaymentController : Controller
    {
        public PhotoStudioDbContext _db = new PhotoStudioDbContext();
        // GET: Payment
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
            var _paymentList = (from payment1 in _db.Payments
                                join order1 in _db.Orders on payment1.orderId equals order1.orderId
                                where payment1.studioId== loggiedid
                                select new PaymentViewModel
                                {
                                    paymentId=payment1.paymentId,
                                    orderId=order1.orderId,
                                    balanceAmount=payment1.balanceAmount,
                                    OrderNo=order1.OrderNo,
                                    downPayment=payment1.downPayment,
                                    paymentDate=payment1.paymentDate,
                                    paymentDetail=payment1.paymentDetail,
                                    payMode=payment1.payMode,
                                    receiptNo=payment1.receiptNo,
                                    totalCost=order1.totalCost,
                                    studioId= loggiedid
                                }).ToList();
            return View(_paymentList);
        }
        public JsonResult getOrderDetailByOrderNo(int ordNo)
        {
            var order = _db.Orders.SingleOrDefault(m => m.OrderNo == ordNo);
            if (order != null)
            {
                return Json(order, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult MakePayment()
        {
            ViewBag.OrderNo = new SelectList(_db.Orders.ToList(), "OrderNo", "OrderNo");
            return View();
        }
        [HttpPost]
        public ActionResult MakePayment(PaymentViewModel obj)
        {
            ViewBag.OrderNo = new SelectList(_db.Orders.ToList(), "OrderNo", "OrderNo");
            var loggiedid = Convert.ToInt32(Session["LoggedId"]);
                                          
            Payment _payment = new Payment()
            {
                orderId=obj.orderId,
                paymentId=obj.paymentId,
                paymentDetail=obj.paymentDetail,
                balanceAmount=obj.balanceAmount,
                downPayment=obj.downPayment,
                paymentDate=obj.paymentDate,
                payMode=obj.payMode,
                receiptNo=obj.receiptNo,
                studioId= loggiedid
            };
            _db.Payments.Add(_payment);
            _db.SaveChanges();

            Order _order = new Order()
            {
                customerId = obj.customerId,
                orderId = obj.orderId,
                OrderNo = obj.OrderNo,
                totalCost = obj.totalCost,
                studioId = loggiedid
            };
            _db.Orders.Add(_order);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        //GET Edit
        public ActionResult Edit(int id)
        {
            ViewBag.OrderNo = new SelectList(_db.Orders.ToList(), "OrderNo", "OrderNo");

            var record = _db.Payments.SingleOrDefault(m => m.paymentId == id);
            return View(record);
        }
        [HttpPost]
        public ActionResult Edit(Payment obj)
        {
            ViewBag.OrderNo = new SelectList(_db.Orders.ToList(), "OrderNo", "OrderNo");

            var record = _db.Payments.SingleOrDefault(m => m.paymentId == obj.paymentId);
            if (record != null)
            {
                record.paymentId = obj.paymentId;
                record.orderId = obj.orderId;
                record.studioId = obj.studioId;
                record.balanceAmount = obj.balanceAmount;
                record.downPayment = obj.downPayment;
                record.paymentDate = obj.paymentDate;
                record.paymentDetail = obj.paymentDetail;
                record.payMode = obj.payMode;
                record.receiptNo = obj.receiptNo;
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var record = _db.Payments.SingleOrDefault(m => m.paymentId == id);
            _db.Payments.Remove(record);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        //To download list of Payment in excel
        public ActionResult DuedateExportList()
        {
            var _Daily = _db.Payments.ToList();
            //var _Daily1 = _db.Orders.ToList();

            if (_Daily.Count != 0)
            {
                var _Dailyquery = _Daily.Select(daily => new
                {
                    receiptNo = daily.receiptNo,
                    paymentDate = daily.paymentDate,
                    paymentDetail = daily.paymentDetail,
                    payMode = daily.payMode,
                    DownPayment = daily.downPayment,
                    balanceAmount = daily.balanceAmount,
                    
                }).ToList();


                //var _Dailyquery1 = _Daily1.Select(daily1 => new
                //{
                //    OrderNo = daily1.OrderNo,
                //    paymentDetail = daily1.totalCost,

                //}).ToList();

                System.Web.UI.WebControls.GridView gv = new System.Web.UI.WebControls.GridView();

                gv.DataSource = _Dailyquery;
                gv.DataBind();


                gv.HeaderRow.Cells[0].Text = "Receipt No.";
                gv.HeaderRow.Cells[1].Text = "Date";
                //gv.HeaderRow.Cells[2].Text = "Order No.";
                gv.HeaderRow.Cells[2].Text = "Payment Details";
                gv.HeaderRow.Cells[3].Text = "Payment Mode";
                //gv.HeaderRow.Cells[5].Text = "Total Cost";
                gv.HeaderRow.Cells[4].Text = "Down Payment";
                gv.HeaderRow.Cells[5].Text = "Balance Amount";

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=PaymentExcel.xls");
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
    }
}