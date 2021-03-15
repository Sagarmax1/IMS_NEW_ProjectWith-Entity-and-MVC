using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using IMS_NEW.Models;
using System.IO;

namespace IMS_NEW.Controllers
{
    public class AdminController : Controller
    {
        IMSEntities Ims = new IMSEntities();
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["UserName"] == null)   // This Syntax For Restriction to go without login on AssetMaster Page
            {
                return RedirectToAction("Login", "Admin");
            }
            return View(Ims.tbl_UserRegistration.ToList());
        }


        // Export to excel code 



        // Export to excel

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(tbl_UserRegistration user)
        {
            try
            {
                if (ModelState.IsValid == true)
                {
                    //####### 1st Method for Insert Record In Database without extra column value #######//

                    //Ims.tbl_UserRegistration.Add(user); 
                    //Ims.SaveChanges();                   
                    //ViewBag.Message = "Insert SuccessFull";

                    // ################################################################# //

                    //##### 2nd Method for Insert Record In database with Extra Column Value ######//

                    tbl_UserRegistration add = new tbl_UserRegistration();
                    add.UserName = user.UserName;
                    add.Email = user.Email;
                    add.Password = user.Password;
                    add.CreatedOn = DateTime.Now;
                    Ims.tbl_UserRegistration.Add(add);
                    Ims.SaveChanges();
                    ViewBag.Message = "Insert SuccessFull Go to Login Page";

                    //###########################################################//

                }
                ModelState.Clear();
                // return RedirectToAction("Login", "Admin");
                return View();


            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(tbl_UserRegistration user)
        {
            try
            {
                if (ModelState.IsValid == true)
                {
                    var data = Ims.tbl_UserRegistration.Where(a => a.UserName == user.UserName && a.Password == user.Password).FirstOrDefault();
                    if (data == null)
                    {
                        ViewBag.ErrorMessage = "Login Fail Please Enter Correct Information !";
                        return View();
                    }
                    else
                    {
                        Session["UserName"] = user.UserName;
                        // Session["UserId"] = user.Id;
                        return RedirectToAction("AssetMaster", "Asset");
                    }
                }
                ModelState.Clear();
                return View();

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Login", "Admin");
        }


        public ActionResult Edit(int id)
        {
            var row = Ims.tbl_UserRegistration.Where(a => a.Id == id).FirstOrDefault();
            return View(row);
        }

        [HttpPost]
        public ActionResult Edit(tbl_UserRegistration user)
        {
            if (ModelState.IsValid == true)
            {
                Ims.Entry(user).State = EntityState.Modified;
                //tbl_UserRegistration add = new tbl_UserRegistration();
                //add.UserName = user.UserName;
                //add.Email = user.Email;
                //add.Password = user.Password;
                //add.CreatedOn = DateTime.Now;
                // Ims.tbl_UserRegistration.Add(user);
                int a = Ims.SaveChanges();
                if (a > 0)
                {
                    TempData["UpdateMesage"] = " <script>alert('User Update SuccessFull')</Script>";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["UpdateMessage"] = " <script>alert('User Not Update')</Script>";
                }
            }
            return View("Index");
        }

        public ActionResult Delete(int id)
        {
            if (id > 0)
            {
                var Delete = Ims.tbl_UserRegistration.Where(a => a.Id == id).FirstOrDefault();
                if (Delete != null)
                {
                    Ims.Entry(Delete).State = EntityState.Deleted;
                    int a = Ims.SaveChanges();
                    if (a > 0)
                    {
                        TempData["DeleteMesage"] = " <script>alert('Delete SuccessFull')</Script>";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["DeleteMesage"] = " <script>alert('Not Deleted')</Script>";
                    }
                }

            }
            return View("Index");
        }

        [HttpGet]
        public ActionResult FileUpload()
        {
            return View();
        }

        [HttpPost]
        [ActionName("FileUpload")]
        public JsonResult FileUpload_Post()
        {
            bool flag = true;
            string responseMessage = string.Empty;

            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase file = Request.Files[0];

                
                if (file != null && file.ContentLength > 0 && (Path.GetExtension(file.FileName).ToLower() == ".xlsx" || Path.GetExtension(file.FileName).ToLower() == ".xls"))
                {
                    try
                    {
                        string fileName = Path.GetFileName(file.FileName);
                        string filePath = Path.Combine(Server.MapPath("~/UploadFiles"), fileName);
                        file.SaveAs(filePath);

                        flag = true;
                        responseMessage = "Upload Successful.";
                    }
                    catch (Exception ex)
                    {
                        flag = false;
                        responseMessage = "Upload Failed with error: " + ex.Message;
                    }
                }
                else
                {
                    flag = false;
                    responseMessage = "File is invalid.";
                }
            }
            else
            {
                flag = false;
                responseMessage = "File Upload has no file.";
            }


            return Json(new { success = flag, responseMessage = responseMessage }, JsonRequestBehavior.AllowGet);
        }



    }
}