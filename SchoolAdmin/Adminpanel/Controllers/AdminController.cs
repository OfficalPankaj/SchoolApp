using Adminpanel.BusinessLayer;
using Adminpanel.DataAccess;
using Adminpanel.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Adminpanel.Controllers
{
    public class AdminController : Controller
    {
        DB_SchoolMgmtContext db = new DB_SchoolMgmtContext();
        BannerOperations objBanerOperation = new BannerOperations();
        AboutUsOperations objAboutUsOperation = new AboutUsOperations();
        CourseOperations objCourseOperation = new CourseOperations();
        GalleryOperations objGalleryOperation = new GalleryOperations();

        // GET: Admin
        public ActionResult Index()
        {
            if (Session["UserName"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Signin");
            }
        }

        public ActionResult Signin()
        {
            ViewBag.ErrorMsg = "";

            return View();
        }
        [HttpPost]
        public ActionResult Signin(TblAdminLogin model)
        {
            ViewBag.ErrorMsg = "";
            if (model.UserName.Trim() != "" && model.UserPassword.Trim() != "")
            {
                var obj = db.TblAdminLogin.Where(a => a.UserName.Equals(model.UserName.Trim()) && a.UserPassword.Equals(model.UserPassword.Trim())).FirstOrDefault();
                if (obj != null)
                {
                    Session["UserId"] = obj.Id.ToString();
                    Session["UserName"] = obj.UserName.ToString();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorMsg = "Invalid UserId Or Password !";
                    return View();

                }

            }
            return View();
        }

        public ActionResult Signup()
        {

            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();

            //  FormsAuthentication.SignOut();
            return RedirectToAction("Signin");

        }
        [HttpGet]
        public async Task<ActionResult> BannerMaster(int? id)
        {
            if (Session["UserName"] != null)
            {
                ViewBag.txtBannerHeading = "";
                ViewBag.BannerFilePath = "";
                ViewBag.BannerFileName = "";
                ViewBag.Id = 0;
                ViewBag.Status = "0";
                ViewBag.btnText = "SUBMIT";
                ViewBag.BannerHeading = "Add Banner :";
                if (id > 0)
                {
                    var BannerDetails = db.TblBannerMaster.Find(id);
                    ViewBag.txtBannerHeading = BannerDetails.BannerHeading;
                    ViewBag.BannerFilePath = BannerDetails.BannerPath;
                    ViewBag.BannerFileName = BannerDetails.BannerImage;
                    ViewBag.Id = BannerDetails.Id;
                    ViewBag.Status = BannerDetails.IsActive == true ? "1" : "2";
                    ViewBag.btnText = "UPDATE";
                    ViewBag.BannerHeading = "Update Banner :";

                }
                var bannerDetails = await objBanerOperation.GetLocationMaster();
                return View(bannerDetails);
            }
            else
            {
                return RedirectToAction("Signin", "Admin");
            }

        }

        [HttpPost]
        [Route("/Admin/BannerMaster")]
        public async Task<ActionResult> BannerMaster(TblBannerMaster model, HttpPostedFileBase BannerFile, string Status)
        {
            if (Session["UserName"] != null)
            {
                int AddedByid = Convert.ToInt32(Session["UserId"].ToString());

                if (model.Id > 0)
                {
                    var data = db.TblBannerMaster.Find(model.Id);
                    if (BannerFile != null && BannerFile.FileName != "")
                    {

                        string directorypath = ConfigurationManager.AppSettings["FileUploadBaseDirectory"];
                        string folderPath = Path.Combine(directorypath, "Uploads", "BannerImages");
                        string folderfilepath = folderPath + "//" + data.BannerPath;
                        if (Directory.Exists(folderPath))
                        {
                            if (System.IO.File.Exists(folderfilepath))
                            {
                                System.IO.File.Delete(folderfilepath);
                            }
                        }

                        string UploadedFilePath = objBanerOperation.UploadBannerImage(BannerFile, "BannerImages");
                        if (data != null)
                        {
                            data.BannerPath = UploadedFilePath;
                            data.BannerImage = BannerFile.FileName;
                            data.BannerHeading = model.BannerHeading;
                            data.IsActive = Status == "1" ? true : false;
                            db.SaveChanges();
                        }
                    }
                    else
                    {

                        var existingEntity = db.TblBannerMaster.Find(model.Id);
                        if (existingEntity != null)
                        {
                            existingEntity.BannerHeading = model.BannerHeading;
                            existingEntity.IsActive = Status == "1" ? true : false;
                            db.SaveChanges();
                        }
                    }
                }
                else
                {
                    string UploadedFilePath = objBanerOperation.UploadBannerImage(BannerFile, "BannerImages");
                    var newRecord = new TblBannerMaster
                    {
                        BannerPath = UploadedFilePath,
                        BannerImage = BannerFile.FileName,
                        BannerHeading = model.BannerHeading,
                        IsActive = Status == "1" ? true : false,
                        IsDelete = false,
                        AddedBy = AddedByid,
                        AddedOn = DateTime.Now,

                    };
                    db.TblBannerMaster.Add(newRecord);
                    db.SaveChanges();
                }

                return RedirectToAction("BannerMaster");
            }
            else
            {
                return RedirectToAction("Signin", "Admin");
            }

        }

        public ActionResult DeleteBanner(int id)
        {
            var data = db.TblBannerMaster.Find(id);
            if (data != null)
            {
                if (data.BannerPath != "")
                {
                    string directorypath = ConfigurationManager.AppSettings["FileUploadBaseDirectory"];
                    string folderPath = Path.Combine(directorypath, "Uploads", "BannerImages");
                    string folderfilepath = folderPath + "//" + data.BannerPath;
                    if (Directory.Exists(folderPath))
                    {
                        if (System.IO.File.Exists(folderfilepath))
                        {
                            System.IO.File.Delete(folderfilepath);
                        }
                    }
                }
                db.TblBannerMaster.Remove(data);
                db.SaveChanges();
            }

            return RedirectToAction("BannerMaster");
        }

        public async Task<ActionResult> AboutUsMaster(int? id)
        {
            if (Session["UserName"] != null)
            {
                ViewBag.txtAboutUsHeading = "";
                ViewBag.txtAboutUsDescreption = "";
                ViewBag.AboutUsFilePath = "";
                ViewBag.AboutUsFileName = "";
                ViewBag.Id = 0;
                ViewBag.Status = "0";
                ViewBag.btnText = "SUBMIT";
                ViewBag.AboutUsHeading = "Add About Us :";
                if (id > 0)
                {
                    var AboutUsDetails = db.TblAboutUsMaster.Find(id);
                    ViewBag.txtAboutUsHeading = AboutUsDetails.AboutUsHeading;
                    ViewBag.txtAboutUsDescreption = AboutUsDetails.Descreption;
                    ViewBag.AboutUsFilePath = AboutUsDetails.AboutsFilePath;
                    ViewBag.AboutUsFileName = AboutUsDetails.AboutsFileName;
                    ViewBag.Id = AboutUsDetails.Id;
                    ViewBag.Status = AboutUsDetails.IsActive == true ? "1" : "2";
                    ViewBag.btnText = "UPDATE";
                    ViewBag.AboutUsHeading = "Update About Us :";

                }
                var bannerDetails = await objAboutUsOperation.GetAboutUsMaster();
                return View(bannerDetails);
            }
            else
            {
                return RedirectToAction("Signin", "Admin");
            }
        }

        [HttpPost]
        [Route("/Admin/AboutUsMaster")]
        public async Task<ActionResult> AboutUsMaster(TblAboutUsMaster model, HttpPostedFileBase AboutUsFile, string Status)
        {
            if (Session["UserName"] != null)
            {
                int AddedByid = Convert.ToInt32(Session["UserId"].ToString());

                if (model.Id > 0)
                {
                    var data = db.TblAboutUsMaster.Find(model.Id);
                    if (AboutUsFile != null && AboutUsFile.FileName != "")
                    {

                        string directorypath = ConfigurationManager.AppSettings["FileUploadBaseDirectory"];
                        string folderPath = Path.Combine(directorypath, "Uploads", "AboutUsImages");
                        string folderfilepath = folderPath + "//" + data.AboutsFilePath;
                        if (Directory.Exists(folderPath))
                        {
                            if (System.IO.File.Exists(folderfilepath))
                            {
                                System.IO.File.Delete(folderfilepath);
                            }
                        }

                        string UploadedFilePath = objAboutUsOperation.UploadAboutUsImage(AboutUsFile, "AboutUsImages");
                        if (data != null)
                        {
                            data.AboutsFilePath = UploadedFilePath;
                            data.AboutsFileName = AboutUsFile.FileName;
                            data.AboutUsHeading = model.AboutUsHeading;
                            data.Descreption = model.Descreption;
                            data.IsActive = Status == "1" ? true : false;
                            data.ModifiedBy = AddedByid;
                            data.ModifiedOn = DateTime.Now;
                            db.SaveChanges();
                        }
                    }
                    else
                    {

                        var existingEntity = db.TblAboutUsMaster.Find(model.Id);
                        if (existingEntity != null)
                        {
                            existingEntity.AboutUsHeading = model.AboutUsHeading;
                            existingEntity.Descreption = model.Descreption;
                            existingEntity.IsActive = Status == "1" ? true : false;
                            existingEntity.ModifiedBy = AddedByid;
                            existingEntity.ModifiedOn = DateTime.Now;
                            db.SaveChanges();
                        }
                    }
                }
                else
                {
                    string UploadedFilePath = objAboutUsOperation.UploadAboutUsImage(AboutUsFile, "AboutUsImages");
                    var newRecord = new TblAboutUsMaster
                    {
                        AboutsFilePath = UploadedFilePath,
                        AboutsFileName = AboutUsFile.FileName,
                        AboutUsHeading = model.AboutUsHeading,
                        Descreption = model.Descreption,
                        IsActive = Status == "1" ? true : false,
                        IsDelete = false,
                        AddedBy = AddedByid,
                        AddedOn = DateTime.Now,

                    };
                    db.TblAboutUsMaster.Add(newRecord);
                    db.SaveChanges();
                }

                return RedirectToAction("AboutUsMaster");
            }
            else
            {
                return RedirectToAction("Signin", "Admin");
            }

        }

        public ActionResult DeleteAboutUsMaster(int id)
        {
            var data = db.TblAboutUsMaster.Find(id);
            if (data != null)
            {
                if (data.AboutsFilePath != "")
                {
                    string directorypath = ConfigurationManager.AppSettings["FileUploadBaseDirectory"];
                    string folderPath = Path.Combine(directorypath, "Uploads", "AboutUsImages");
                    string folderfilepath = folderPath + "//" + data.AboutsFilePath;
                    if (Directory.Exists(folderPath))
                    {
                        if (System.IO.File.Exists(folderfilepath))
                        {
                            System.IO.File.Delete(folderfilepath);
                        }
                    }
                }
                db.TblAboutUsMaster.Remove(data);
                db.SaveChanges();
            }

            return RedirectToAction("AboutUsMaster");
        }
        [HttpGet]
        public async Task<ActionResult> CourseMaster(int? id)
        {
            if (Session["UserName"] != null)
            {
                ViewBag.txtCourseName = "";
                ViewBag.txtCoursePunchLine = "";
                ViewBag.CourseFilePath = "";
                ViewBag.CourseFileName = "";
                ViewBag.txtDescreption = "";
                ViewBag.Id = 0;
                ViewBag.Status = "0";
                ViewBag.btnText = "SUBMIT";
                ViewBag.CourseHeading = "Add Course :";
                if (id > 0)
                {
                    var CourseDetail = db.TblCourseMaster.Find(id);
                    ViewBag.txtCourseName = CourseDetail.CourseName;
                    ViewBag.txtCoursePunchLine = CourseDetail.CoursePunchLine;
                    ViewBag.CourseFilePath = CourseDetail.CourseFilePath;
                    ViewBag.CourseFileName = CourseDetail.CourseFileName;
                    ViewBag.txtDescreption = CourseDetail.Descreption;
                    ViewBag.Id = CourseDetail.Id;
                    ViewBag.Status = CourseDetail.IsActive == true ? "1" : "2";
                    ViewBag.btnText = "UPDATE";
                    ViewBag.BannerHeading = "Update Course :";

                }
                var coursedetails = await objCourseOperation.GetCourseMaster();
                return View(coursedetails);
            }
            else
            {
                return RedirectToAction("Signin", "Admin");
            }

        }

        [HttpPost]
        [Route("/Admin/CourseMaster")]
        public async Task<ActionResult> CourseMaster(TblCourseMaster model, HttpPostedFileBase CourseFile, string Status)
        {
            if (Session["UserName"] != null)
            {
                int AddedByid = Convert.ToInt32(Session["UserId"].ToString());

                if (model.Id > 0)
                {
                    var data = db.TblCourseMaster.Find(model.Id);
                    if (CourseFile != null && CourseFile.FileName != "")
                    {

                        string directorypath = ConfigurationManager.AppSettings["FileUploadBaseDirectory"];
                        string folderPath = Path.Combine(directorypath, "Uploads", "CourseImages");
                        string folderfilepath = folderPath + "//" + data.CourseFilePath;
                        if (Directory.Exists(folderPath))
                        {
                            if (System.IO.File.Exists(folderfilepath))
                            {
                                System.IO.File.Delete(folderfilepath);
                            }
                        }

                        string UploadedFilePath = objCourseOperation.UploadCourseImage(CourseFile, "CourseImages");
                        if (data != null)
                        {
                            data.CourseFilePath = UploadedFilePath;
                            data.CourseFileName = CourseFile.FileName;
                            data.CourseName = model.CourseName;
                            data.CoursePunchLine = model.CoursePunchLine;
                            data.Descreption = model.Descreption;
                            data.IsActive = Status == "1" ? true : false;
                            data.ModifiedBy = AddedByid;
                            data.ModifiedOn = DateTime.Now;
                            db.SaveChanges();
                        }
                    }
                    else
                    {

                        var existingEntity = db.TblCourseMaster.Find(model.Id);
                        if (existingEntity != null)
                        {
                            existingEntity.CoursePunchLine = model.CoursePunchLine;
                            existingEntity.CourseName = model.CourseName;
                            existingEntity.Descreption = model.Descreption;
                            existingEntity.IsActive = Status == "1" ? true : false;
                            existingEntity.ModifiedBy = AddedByid;
                            existingEntity.ModifiedOn = DateTime.Now;
                            db.SaveChanges();
                        }
                    }
                }
                else
                {
                    string UploadedFilePath = objCourseOperation.UploadCourseImage(CourseFile, "CourseImages");
                    var newRecord = new TblCourseMaster
                    {
                        CourseFilePath = UploadedFilePath,
                        CourseFileName = CourseFile.FileName,
                        CourseName = model.CourseName,
                        CoursePunchLine = model.CoursePunchLine,

                        Descreption = model.Descreption,
                        IsActive = Status == "1" ? true : false,
                        IsDelete = false,
                        AddedBy = AddedByid,
                        AddedOn = DateTime.Now,

                    };
                    db.TblCourseMaster.Add(newRecord);
                    db.SaveChanges();
                }

                return RedirectToAction("CourseMaster");
            }
            else
            {
                return RedirectToAction("Signin", "Admin");
            }

        }
        public ActionResult DeleteCourseMaster(int id)
        {
            var data = db.TblCourseMaster.Find(id);
            if (data != null)
            {
                if (data.CourseFilePath != "")
                {
                    string directorypath = ConfigurationManager.AppSettings["FileUploadBaseDirectory"];
                    string folderPath = Path.Combine(directorypath, "Uploads", "CourseImages");
                    string folderfilepath = folderPath + "//" + data.CourseFilePath;
                    if (Directory.Exists(folderPath))
                    {
                        if (System.IO.File.Exists(folderfilepath))
                        {
                            System.IO.File.Delete(folderfilepath);
                        }
                    }
                }
                db.TblCourseMaster.Remove(data);
                db.SaveChanges();
            }

            return RedirectToAction("CourseMaster");
        }

        [HttpGet]
        public async Task<ActionResult> GalleryMaster(int? id)
        {
            if (Session["UserName"] != null)
            {
                ViewBag.txtGalleryPunchLine = "";
                ViewBag.GalleryFileName = "";
                ViewBag.GalleryFilePath = "";
                ViewBag.Id = 0;
                ViewBag.Status = "0";
                ViewBag.btnText = "SUBMIT";
                ViewBag.GalleryHeading = "Add Gallery :";
                if (id > 0)
                {
                    var GalleryDetails = db.TblGalleryMaster.Find(id);
                    ViewBag.txtGalleryPunchLine = GalleryDetails.GalleryPunchLine;
                    ViewBag.GalleryFileName = GalleryDetails.GalleryFileName;
                    ViewBag.GalleryFilePath = GalleryDetails.GalleryFilePath;
                    ViewBag.Id = GalleryDetails.Id;
                    ViewBag.Status = GalleryDetails.IsActive == true ? "1" : "2";
                    ViewBag.btnText = "UPDATE";
                    ViewBag.GalleryHeading = "Update Gallery :";

                }
                var galleryDetails = await objGalleryOperation.GetLGalleryMaster();
                return View(galleryDetails);
            }
            else
            {
                return RedirectToAction("Signin", "Admin");
            }

        }

        [HttpPost]
        [Route("/Admin/GalleryMaster")]
        public async Task<ActionResult> GalleryMaster(TblGalleryMaster model, HttpPostedFileBase GalleryFile, string Status)
        {
            if (Session["UserName"] != null)
            {
                int AddedByid = Convert.ToInt32(Session["UserId"].ToString());

                if (model.Id > 0)
                {
                    var data = db.TblGalleryMaster.Find(model.Id);
                    if (GalleryFile != null && GalleryFile.FileName != "")
                    {

                        string directorypath = ConfigurationManager.AppSettings["FileUploadBaseDirectory"];
                        string folderPath = Path.Combine(directorypath, "Uploads", "GalleryImages");
                        string folderfilepath = folderPath + "//" + data.GalleryFilePath;
                        if (Directory.Exists(folderPath))
                        {
                            if (System.IO.File.Exists(folderfilepath))
                            {
                                System.IO.File.Delete(folderfilepath);
                            }
                        }

                        string UploadedFilePath = objGalleryOperation.UploadGalleryImage(GalleryFile, "GalleryImages");
                        if (data != null)
                        {
                            data.GalleryFilePath = UploadedFilePath;
                            data.GalleryFileName = GalleryFile.FileName;
                            data.GalleryPunchLine = model.GalleryPunchLine;
                            data.IsActive = Status == "1" ? true : false;
                            db.SaveChanges();
                        }
                    }
                    else
                    {

                        var existingEntity = db.TblGalleryMaster.Find(model.Id);
                        if (existingEntity != null)
                        {
                            existingEntity.GalleryPunchLine = model.GalleryPunchLine;
                            existingEntity.IsActive = Status == "1" ? true : false;
                            db.SaveChanges();
                        }
                    }
                }
                else
                {
                    string UploadedFilePath = objGalleryOperation.UploadGalleryImage(GalleryFile, "GalleryImages");
                    var newRecord = new TblGalleryMaster
                    {
                        GalleryFilePath = UploadedFilePath,
                        GalleryFileName = GalleryFile.FileName,
                        GalleryPunchLine = model.GalleryPunchLine,
                        IsActive = Status == "1" ? true : false,
                        IsDelete = false,
                        AddedBy = AddedByid,
                        AddedOn = DateTime.Now,

                    };
                    db.TblGalleryMaster.Add(newRecord);
                    db.SaveChanges();
                }

                return RedirectToAction("GalleryMaster");
            }
            else
            {
                return RedirectToAction("Signin", "Admin");
            }

        }

        public ActionResult DeleteGallery(int id)
        {
            var data = db.TblGalleryMaster.Find(id);
            if (data != null)
            {
                if (data.GalleryFilePath != "")
                {
                    string directorypath = ConfigurationManager.AppSettings["FileUploadBaseDirectory"];
                    string folderPath = Path.Combine(directorypath, "Uploads", "GalleryImages");
                    string folderfilepath = folderPath + "//" + data.GalleryFilePath;
                    if (Directory.Exists(folderPath))
                    {
                        if (System.IO.File.Exists(folderfilepath))
                        {
                            System.IO.File.Delete(folderfilepath);
                        }
                    }
                }
                db.TblGalleryMaster.Remove(data);
                db.SaveChanges();
            }

            return RedirectToAction("GalleryMaster");
        }




    }
}