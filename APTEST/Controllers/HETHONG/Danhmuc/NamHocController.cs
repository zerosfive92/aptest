using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using APTEST.Models;
namespace APTEST.Controllers.HETHONG.Danhmuc
{
    public class NamHocController : BaseController
    {
        // GET: NamHoc
        public ActionResult Show()
        {
            return View();
        }

        [HttpGet]
        public ActionResult getNamHoc(int? page, string search = "")
        {
            int pageSize = 50;

            int pageNumber = (page ?? 1);


            var data = db.DM_NamHoc.Where(p => p.NamHoc.Contains(search) && p.MaTruong == MaDonVi).ToList();

            ResultInfo result = new ResultWithPaging()
            {
                error = 0,
                msg = "",
                page = pageNumber,
                pageSize = pageSize,
                toltalSize = data.Count(),
                data = data.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList()
            };


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult create(string NamHoc, string NienKhoa, string tuNgay, string denNgay, bool HienTai)
        {

            if (String.IsNullOrEmpty(NamHoc))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);
            // 20/12/2018
            string tngay = tuNgay.Substring(0, 2);
            string tthang = tuNgay.Substring(3, 2);
            string tnam = tuNgay.Substring(6, 4);

            string dngay = denNgay.Substring(0, 2);
            string dthang = denNgay.Substring(3, 2);
            string dnam = denNgay.Substring(6, 4);

            string sysFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;

            //format sever M/d/yyyy
            DateTime dateFrom;
            DateTime dateTo;
            if (sysFormat == "M/d/yyyy")
            {
                dateFrom = DateTime.Parse(tthang + '/' + tngay + '/' + tnam);
                dateTo = DateTime.Parse(dthang + '/' + dngay + '/' + dnam);
            }
            else
            {
                dateFrom = DateTime.Parse(tuNgay);
                dateTo = DateTime.Parse(denNgay);
            }


            var check = db.DM_NamHoc.Where(p => p.MaTruong == MaDonVi && p.NamHoc == NamHoc).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);

            //kiem tra neu  la nam hoc hien tai thi xoa nhưng cai con lai
            if (HienTai == true)
            {
                db.DM_NamHoc.Where(x => x.MaTruong == MaDonVi && x.NamHoc != NamHoc).ToList().ForEach(x =>
                {
                    x.HienTai = false;
                });
                db.SaveChanges();
            }
            var insData = new DM_NamHoc()
            {

                NamHoc = NamHoc,
                TuNgay = dateFrom,
                DenNgay = dateTo,
                MaTruong = MaDonVi,
                NienKhoa = NienKhoa,
                HienTai = HienTai
            };
            db.DM_NamHoc.Add(insData);
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = sysFormat }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult edit(DM_NamHoc namhoc, string tuNgay, string denNgay)
        {
            if (String.IsNullOrEmpty(namhoc.NamHoc))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);
            string tngay = tuNgay.Substring(0, 2);
            string tthang = tuNgay.Substring(3, 2);
            string tnam = tuNgay.Substring(6, 4);

            string dngay = denNgay.Substring(0, 2);
            string dthang = denNgay.Substring(3, 2);
            string dnam = denNgay.Substring(6, 4);

            string sysFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;

            //format sever M/d/yyyy
            DateTime dateFrom;
            DateTime dateTo;
            if (sysFormat == "M/d/yyyy")
            {
                dateFrom = DateTime.Parse(tthang + '/' + tngay + '/' + tnam);
                dateTo = DateTime.Parse(dthang + '/' + dngay + '/' + dnam);
            }
            else
            {
                dateFrom = DateTime.Parse(tuNgay);
                dateTo = DateTime.Parse(denNgay);
            }
            var check = db.DM_NamHoc.Where(p => p.MaTruong == MaDonVi && p.NamHoc == namhoc.NamHoc).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);


            if (namhoc.HienTai == true)
            {
                db.DM_NamHoc.Where(x => x.MaTruong == MaDonVi && x.NamHoc != namhoc.NamHoc).ToList().ForEach(x =>
                {
                    x.HienTai = false;
                });
                // db.SaveChanges();
            }
            check.NamHoc = namhoc.NamHoc;
            check.MaTruong = MaDonVi;
            namhoc.TuNgay = dateFrom;
            namhoc.DenNgay = dateTo;
            check.DenNgay = namhoc.DenNgay;
            check.HienTai = namhoc.HienTai;
            check.NienKhoa = namhoc.NamHoc;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(string namhoc)
        {
            if (String.IsNullOrEmpty(namhoc))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.DM_NamHoc.Where(p => p.MaTruong == MaDonVi && p.NamHoc == namhoc).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);
            if (check.HienTai == true)
                return Json(new ResultInfo() { error = 1, msg = "Không thể xóa năm hiện tại" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
    }
}