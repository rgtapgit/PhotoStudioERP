using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhotoStudioApp.Models;
using System.IO;

namespace PhotoStudioApp.Controllers
{
    public class StudioController : Controller
    {
        public PhotoStudioDbContext _db = new PhotoStudioDbContext();
        // GET: PhotoStudio
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
            var _studioList = _db.Studios.ToList();
            return View(_studioList);
        }
        public ActionResult NewStudio()
        {
            return View();
        }
        [HttpPost]
        public ActionResult NewStudio(Studio obj, HttpPostedFileBase file2)
        {
            try
            {
                string sFileSavePath = Server.MapPath("~/UserProfilePhoto");

                string sfilename = Path.GetFileName(file2.FileName);
                var uniqueName = DateTime.Now.ToShortDateString() + sfilename;      //dynamic uniqueName for uploaded photos
                string spath = sFileSavePath + "//" + uniqueName;
                file2.SaveAs(spath);
                Studio _studio = new Studio()
                {
                    studioId=obj.studioId,
                    studioName=obj.studioName,
                    Password = obj.Password,
                    ProfilePhoto= uniqueName
                };                   
                _db.Studios.Add(_studio);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, error = ex.Message });
            }
        }

        //Adding validation on upload a image
        public ActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Upload(Studio obj, HttpPostedFileBase file2)
        {
            int permittedSizeInBytes = 40000;//4mb

            string permittedType = "image/jpeg,image/gif";

            if (Request.Files.Count > 0)
            {
                //var file2 = Request.Files[0];

                if (file2 != null && file2.ContentLength > 0)
                {

                    if (file2.ContentLength > permittedSizeInBytes)
                    {
                        ViewBag.Message = "File cannot be more than 4MB";
                    }
                    else
                    {
                        if (!permittedType.Split(",".ToCharArray()).Contains(file2.ContentType))
                        {
                            ViewBag.Message = "Invalid file type";
                        }
                        else
                        {
                            var fileName = Path.GetFileName(file2.FileName);
                            var path = Path.Combine(Server.MapPath("~/UserProfilePhoto"), fileName);
                            file2.SaveAs(path);
                            ViewBag.Message = "File uploaded successfully";
                            Studio _studio = new Studio()
                            {
                                studioId = obj.studioId,
                                studioName = obj.studioName,
                                Password = obj.Password,
                                ProfilePhoto = fileName
                            };
                            _db.Studios.Add(_studio);
                            _db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                    }
                }
                else
                    ViewBag.Message = "File does not have any content";
            }
            ViewBag.Message = "No files uploaded";


            return View();
        }

        public ActionResult Edit(int id)
        {
            var record = _db.Studios.SingleOrDefault(m => m.studioId == id);
            return View(record);
        }
        [HttpPost]
        public ActionResult Edit(Studio obj)
        {
            var record = _db.Studios.SingleOrDefault(m => m.studioId == obj.studioId);
            record.studioId = obj.studioId;
            record.studioName = obj.studioName;
            record.Password = obj.Password;
            record.ProfilePhoto = obj.ProfilePhoto;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var record = _db.Studios.Single(m => m.studioId == id);
            _db.Studios.Remove(record);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        //To download list of Payment in excel
        public ActionResult DuedateExportList()
        {
            var _Daily = _db.Studios.ToList();

            if (_Daily.Count != 0)
            {
                var _Dailyquery = _Daily.Select(daily => new
                {
                    studioId = daily.studioId,
                    studioName = daily.studioName,
                    Password = daily.Password,
                    ProfilePhoto = daily.ProfilePhoto,
                }).ToList();

                System.Web.UI.WebControls.GridView gv = new System.Web.UI.WebControls.GridView();

                gv.DataSource = _Dailyquery;
                gv.DataBind();


                gv.HeaderRow.Cells[0].Text = "Studio Id";
                gv.HeaderRow.Cells[1].Text = "studio Name";
                gv.HeaderRow.Cells[2].Text = "Password";
                gv.HeaderRow.Cells[3].Text = "Profile Photo";

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=StudioExcel.xls");
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