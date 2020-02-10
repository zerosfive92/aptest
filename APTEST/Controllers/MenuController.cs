using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APTEST.Controllers
{
    public class MenuController : BaseController
    {
        // GET: Menu
        [HttpGet]
        public ActionResult AdminMenu()
        {
            if (UType == "SUP")
            {
                return PartialView("_SUPMenu");
            }
            else if (UType == "AD")
            {
                return PartialView("_ADMenu");
            }
            else if (UType == "BGH")
            {
                return PartialView("_BGHMenu");
            }
            else if (UType == "GV")
            {
                return PartialView("_GVMenu");
            }
            else if (UType == "HS")
            {
                return PartialView("_HSMenu");
            }
            else
            {
                return PartialView("_NoneView");
            }
        }
    }
}