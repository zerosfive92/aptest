using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using APTEST.Models;

namespace APTEST.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        protected APTESTEntities db = new APTESTEntities();
        protected RoleManager<IdentityRole> RoleManager { get; private set; }

        protected UserManager<ApplicationUser> UserManager { get; private set; }

        private ApplicationDbContext sdb = new ApplicationDbContext();

        public string MaDonVi = "SAdmin";
        public string TenDonVi = "SUP Admin";
        public string MaPhongBan = "SAdmin";
        public string UType;
        public bool? IsPhong = false;
        public int? MaHuyen;
        public string DonViCapTren = "";
        public string UserGroup;
        public string Email = "";
        public string NamHoc = "";
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            //MaTruong = "LQDON";
            base.Initialize(requestContext);

            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var checkUser = db.AspNetUsers.Where(p => p.UserName == requestContext.HttpContext.User.Identity.Name).FirstOrDefault();
                UType = checkUser.UType;
                if (checkUser != null && UType != "SUP") //SUP,AD,BBGH,GV,HS
                {
                    MaDonVi = checkUser.MaDonVi;
                    MaPhongBan = checkUser.PhongBan;
                    var checkloai = db.DM_DonVi.Where(x => x.MaDonVi == MaDonVi).FirstOrDefault();
                    MaHuyen = checkloai.MaHuyen;
                    TenDonVi = checkloai.TenDonVi;
                    DonViCapTren = checkloai.IDCapTren;
                    UserGroup = checkUser.UserGroup;
                    Email = checkUser.Email;
                    //IsPhong = checkloai.Phong;
                    // NamHoc = db.DM_NamHoc.Where(p => p.MaTruong == MaDonVi && p.HienTai == true).Select(p => p.NamHoc).FirstOrDefault();
                    NamHoc = "NH";
                }
            }
        }
        public BaseController()
        {
            RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(sdb));
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(sdb));
        }
        [HttpPost]
        public ActionResult DeleteFile(string code, string idvb, string madonvi)
        {
            var find = db.VB_TT_File.Where(p => p.MaDonVi == madonvi && p.ID == code && p.IDVB == idvb).FirstOrDefault();

            if (find != null)
            {

                var fileSave = Server.MapPath(find.DuongDan);

                if (System.IO.File.Exists(fileSave))
                {
                    System.IO.File.Delete(fileSave);
                }

                db.VB_TT_File.Remove(find);
                db.SaveChanges();

                return Json(new { error = 0, msg = "File không tìm thấy" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { error = 1, msg = "File không tìm thấy" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DownLoadFileDinhKem(string code, string idvb, string madonvi, string loai)
        {
            var find = db.VB_TT_File.Where(p => p.MaDonVi == madonvi && p.ID == code && p.IDVB == idvb && p.Loai == loai).FirstOrDefault();

            if (find != null)
            {

                var fileSave = Server.MapPath(find.DuongDan);

                if (System.IO.File.Exists(fileSave))
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(fileSave);
                    string fileName = find.TenFile;
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                }


                return Json(new { error = 0, msg = "File không tìm thấy" }, JsonRequestBehavior.AllowGet);
            }

            return Content("Không tải được");
        }
        public ActionResult getHinhAnh(string manv)
        {
            var data = db.VB_TT_File.Where(p => p.MaDonVi == MaDonVi && p.IDVB == manv && p.Loai == "hinhanh").FirstOrDefault();
            return Json(new ResultInfo() { error = 0, data = data }, JsonRequestBehavior.AllowGet);
        }
    }
}