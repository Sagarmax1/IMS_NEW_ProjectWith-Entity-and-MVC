using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using IMS_NEW.Models;

namespace IMS_NEW.Controllers
{
    public class AssetController : Controller
    {
        IMSEntities Ims = new IMSEntities();
        // GET: Asset
        public ActionResult AssetMasterList()
        {
            return View(Ims.tbl_AssetMaster.ToList());
        }

        public ActionResult Edit(int AssetId)
        {
            var row = Ims.tbl_AssetMaster.Where(a => a.AssetId == AssetId).FirstOrDefault();
            return View(row);
        }

        [HttpPost]
        public ActionResult Edit(tbl_AssetMaster user)
        {
            if (ModelState.IsValid == true)
            {
                //Ims.Entry(user).State = EntityState.Modified;
                //int a = Ims.SaveChanges();
                //if (a > 0)
                //{
                //    TempData["UpdateMesage"] = " <script>alert('Asset Update SuccessFull')</Script>";
                //    return RedirectToAction("AssetMasterList");
                //}
                //else
                //{
                //    TempData["UpdateMessage"] = " <script>alert('Asset Not Update')</Script>";
                //}



                using (IMSEntities obj = new IMSEntities())
                {
                    tbl_AssetMaster asset = (from m in obj.tbl_AssetMaster
                                             where m.AssetId == user.AssetId
                                             select m).FirstOrDefault();
                  
                    asset.UserName = user.UserName;
                    asset.LocationId = user.LocationId;
                    asset.Ram = user.Ram;
                    asset.WarrantyStatus = user.WarrantyStatus;
                    asset.WindowKey = user.WindowKey;

                    int e = obj.SaveChanges();
                    if (e > 0)
                    {
                        TempData["UpdateMesage"] = " <script>alert('Asset Update SuccessFull')</Script>";
                        return RedirectToAction("AssetMasterList");
                    }
                    else
                    {
                        TempData["UpdateMessage"] = " <script>alert('Asset Not Update')</Script>";
                    }

                }


            }
            //  return View("AssetMasterList");
                return RedirectToAction("AssetMasterList");

        }


        public ActionResult Delete(int AssetId)
        {
            if (AssetId > 0)
            {
                var Delete = Ims.tbl_AssetMaster.Where(a => a.AssetId == AssetId).FirstOrDefault();
                if (Delete != null)
                {
                    Ims.Entry(Delete).State = EntityState.Deleted;
                   
                    int a = Ims.SaveChanges();
                    if (a > 0)
                    {
                        TempData["DeleteMesage"] = " <script>alert('Are You Sure You Want To Delete !')</Script>";
                       
                        return RedirectToAction("AssetMasterList");
                    }
                    else
                    {
                        TempData["DeleteMesage"] = " <script>alert('Not Deleted')</Script>";
                    }
                }

            }
            return View("AssetMasterList");
        }



        //[HttpPost]
        // public ActionResult Clear()
        // {
        //     return View("AssetMaster");
        // }

        [HttpGet]
        public ActionResult AssetMaster()
        {
            if (Session["UserName"] == null)   // This Syntax For Restriction to go without login on AssetMaster Page
            {
                return RedirectToAction("Login", "Admin");
            }

            tbl_AssetMaster asset = new tbl_AssetMaster();
            using (IMSEntities db = new IMSEntities())
            {
                // asset.AssetTypeCollection = db.tbl_AssetType.ToList<tbl_AssetType>();
            }
            // ViewBag.AssetTypeId = new SelectList(Ims.tbl_AssetType, "AssetTypeId", "AssetTypeName");


            return View();
        }

        [HttpPost]
        public ActionResult AssetMaster(tbl_AssetMaster user)
        {
            try
            {
                if (ModelState.IsValid == true)
                {

                    tbl_AssetMaster add = new tbl_AssetMaster();
                    //add.AssetTypeId = user.AssetTypeId.Value;
                   // add.LocationId = user.LocationId.Value;
                     add.AssetTypeId = 1;
                     add.LocationId = 1;
                    add.UserName = user.UserName;
                    add.SerialNo = user.SerialNo;
                    //  add.AssetsCode = user.AssetsCode;
                    add.Ram = user.Ram;
                    add.PurchaseDate = user.PurchaseDate;
                    add.WarrantyStatus = user.WarrantyStatus;
                    add.WindowKey = user.WindowKey;
                    add.Model = user.Model.ToString();
                    // add.CreatedBy = Convert.ToInt32(Session["UserId"].ToString());
                    add.CreatedBy = 1;

                    add.CreatedOn = DateTime.Now;
                    // add.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
                    add.UpdatedBy = 1;
                    add.UpdatedOn = DateTime.Now;
                    Ims.tbl_AssetMaster.Add(add);
                    Ims.SaveChanges();
                    ViewBag.Message = "Asset Added Successfully";
                }
                ModelState.Clear();
                return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        public ActionResult AssetTransaction()
        {
            if (Session["UserName"] == null)   // This Syntax For Restriction to go without login on AssetMaster Page
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }

        [HttpPost]
        public ActionResult AssetTransaction(tbl_AssetUserMaping user)
        {
            try
            {
                if (ModelState.IsValid == true)
                {
                    tbl_AssetUserMaping add = new tbl_AssetUserMaping();
                    add.AssetsCode = user.AssetsCode;
                    add.AssetId = user.AssetId.Value;
                    add.UserLocationid = user.UserLocationid;
                    // add.AssetId = 1;
                    add.UserName = user.UserName;
                    add.Emailid = user.Emailid;
                   
                    add.AssignDate = user.AssignDate;
                    add.TransferDate = user.TransferDate;
                    add.AssetLocationId = user.AssetLocationId;
                    // add.CreatedBy = Convert.ToInt32(Session["UserId"].ToString());
                    // add.CreatedBy = 1;
                    add.CreatedBy = user.CreatedBy;
                    add.CreatedOn = DateTime.Now;
                    // add.UpdatedBy = Convert.ToInt32(Session["UserId"].ToString());
                    add.UpdatedBy = 1;
                    add.UpdatedOn = DateTime.Now;
                    Ims.tbl_AssetUserMaping.Add(add);
                    Ims.SaveChanges();
                    ViewBag.Message = "Asset Set For User Successfully";

                }
                ModelState.Clear();
                return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult AssetTransactionList()
        {
            return View(Ims.tbl_AssetUserMaping.ToList());
        }



        //new Coding For Edit Case//
    
        //    [HttpGet]
        //  public ActionResult AssetTrEdit(int id)
        //{
        //    using (IMSEntities obj = new IMSEntities())
        //    {
        //        tbl_AssetUserMaping asset = (from m in obj.tbl_AssetUserMaping
        //                                     where m.AssetUserMapplingid == id
        //                                     select m).FirstOrDefault();

        //        return View(asset);
        //    }
        //}

        //[HttpPost]
        //public ActionResult AssetTrEdit(tbl_AssetUserMaping objasset)
        //{
        //    using (IMSEntities obj = new IMSEntities())
        //    {
        //        tbl_AssetUserMaping asset = (from m in obj.tbl_AssetUserMaping
        //                                     where m.AssetUserMapplingid == objasset.AssetUserMapplingid
        //                                     select m).FirstOrDefault();
        //        asset.AssetsCode = objasset.AssetsCode;
        //        asset.UserName = objasset.UserName;
        //        obj.SaveChanges();
                
        //    }
        //    return RedirectToAction("AssetTransactionList");
        //}



        //new Coding For Edit Case//

        public ActionResult EdtForAsTranscationList(int AssetUserMapplingid)
        {
            var row = Ims.tbl_AssetUserMaping.Where(a => a.AssetUserMapplingid == AssetUserMapplingid).FirstOrDefault();
            return View();

        }

        [HttpPost]
        public ActionResult EdtForAsTranscationList(tbl_AssetUserMaping user)
        {
            if (ModelState.IsValid == true)
            {
                Ims.Entry(user).State = EntityState.Modified;
                int a = Ims.SaveChanges();
                if (a > 0)
                {
                    TempData["UpdateMesage"] = " <script>alert('Asset Update SuccessFull')</Script>";
                    return RedirectToAction("AssetTransactionList");
                }
                else
                {
                    TempData["UpdateMessage"] = " <script>alert('Asset Not Update')</Script>";
                }
            }
            return View("AssetTransactionList");
        }

        public ActionResult DLTForAsTranscationList(int AssetUserMapplingid)
        {
            if (AssetUserMapplingid > 0)
            {
                var Delete = Ims.tbl_AssetUserMaping.Where(a => a.AssetUserMapplingid == AssetUserMapplingid).FirstOrDefault();
                if (Delete != null)
                {
                    Ims.Entry(Delete).State = EntityState.Deleted;

                    int a = Ims.SaveChanges();
                    if (a > 0)
                    {
                        TempData["DeleteMesage"] = " <script>alert('Are You Sure You Want To Delete !')</Script>";

                        return RedirectToAction("AssetTransactionList");
                    }
                    else
                    {
                        TempData["DeleteMesage"] = " <script>alert('Not Deleted')</Script>";
                    }
                }

            }
            return View("AssetTransactionList");
        }

    }
}