using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhotoStudioApp.Models;
using System.IO;

namespace PhotoStudioApp.Controllers
{
    public class CustomerController : Controller
    {
        public PhotoStudioDbContext _db = new PhotoStudioDbContext();
        // GET: Customer
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
            var loggiedid = Convert.ToInt32( Session["LoggedId"]);
            var _customerList = _db.Customers.Where(m => m.studioId == loggiedid).ToList();
            return View(_customerList);
        }

        public ActionResult NewCustomer()
        {
            return View();
        }
        [HttpPost]
        public ActionResult NewCustomer(Customer obj, HttpPostedFileBase file1)
        {
            
            var loggiedid = Convert.ToInt32(Session["LoggedId"]);
            try
            {
                if (!ModelState.IsValid && file1 != null)
                {
                    string sFileSavePath = Server.MapPath("~/CustomerPhotos");

                    string sfilename = Path.GetFileName(file1.FileName);
                    var uniqueName = DateTime.Now.ToShortDateString() + sfilename;      //dynamic uniqueName for uploaded photos
                    string spath = sFileSavePath + "//" + uniqueName;
                    file1.SaveAs(spath);
                    Customer _customer = new Customer()
                    {
                        Address = obj.Address,
                        ImageUrl = uniqueName,
                        Contact = obj.Contact,
                        customerId = obj.customerId,
                        customerName = obj.customerName,
                        emailId = obj.emailId,
                        studioId = loggiedid
                    };
                    _db.Customers.Add(_customer);
                    _db.SaveChanges();
                    ViewBag.error = "";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Please check validation and selected file";

                    return View(obj);
                }
                    
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, error = ex.Message });
            }
        }

        // to add new customer from PlaceOrder page
        public ActionResult AddCustomer()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddCustomer(Customer obj)
        {
            var loggiedid = Convert.ToInt32(Session["LoggedId"]);
           
                    Customer _customer = new Customer()
                    {
                        Address = obj.Address,
                        Contact = obj.Contact,
                        customerId = obj.customerId,
                        customerName = obj.customerName,
                        emailId = obj.emailId,
                        studioId = loggiedid
                    };
                    _db.Customers.Add(_customer);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var record = _db.Customers.SingleOrDefault(m => m.customerId == id);
                                   
            return View(record);
        }
        [HttpPost]
        public ActionResult Edit(Customer obj, HttpPostedFileBase file1)
        {
            var record = _db.Customers.SingleOrDefault(m => m.customerId == obj.customerId);
            var loggiedid = Convert.ToInt32(Session["LoggedId"]);
            
                if (ModelState.IsValid)
                {
                try
                {
                    // Get File Name
                    var fileName = Path.GetFileName(file1.FileName) ?? "";
                
                // If file present
                if (fileName != "")
                {
                    // Get Application folder path and combine it with file name
                    var filePath = Path.Combine(Server.MapPath("~/CustomerPhotos"), fileName);

                    // If same name of file present then delete that file first
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    // Save file
                    file1.SaveAs(filePath);

                    // Update Records
                    record.customerId = obj.customerId;
                    record.customerName = obj.customerName;
                    record.Contact = obj.Contact;
                    record.emailId = obj.emailId;
                    record.Address = obj.Address;
                    record.studioId = loggiedid;
                    record.ImageUrl = file1.FileName;
                    _db.Customers.Add(obj);
                    _db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.errorMessage = "No file Present";
                }
              }
                catch (Exception ex)
                {
                    return Json(new { Success = 0, error = ex.Message });
                }
            }
            else
            {
                return View(obj);
            }
            return View(obj);
        }

        public ActionResult Delete(int id)
        {
            var record = _db.Customers.SingleOrDefault(m => m.customerId == id);
            _db.Customers.Remove(record);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        //To download list of customer in excel
        public ActionResult DuedateExportList()
        {

            var _Daily = _db.Customers.ToList();
             
            if (_Daily.Count != 0)
            {
                var _Dailyquery = _Daily.Select(daily => new
                {
                    Name = daily.customerName,
                    Contact = daily.Contact,
                    Address = daily.Address,
                    EmailId = daily.emailId,
                    Photo = daily.ImageUrl,
                }).ToList();

                System.Web.UI.WebControls.GridView gv = new System.Web.UI.WebControls.GridView();

                gv.DataSource = _Dailyquery;
                gv.DataBind();


                gv.HeaderRow.Cells[0].Text = "Customer Name";
                gv.HeaderRow.Cells[1].Text = "Contact No.";
                gv.HeaderRow.Cells[2].Text = "Address";
                gv.HeaderRow.Cells[3].Text = "Email Id";
                gv.HeaderRow.Cells[4].Text = "Photo";

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=CustomerExcel.xls");
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