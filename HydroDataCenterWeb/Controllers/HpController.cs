using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HydroDataCenterWeb.Controllers
{
    public class HpController : Controller
    {
        //
        // GET: /Hp/

        public ActionResult Index()
        {
            List<HydroDataCenterEntity.Models.Site> theSiteList = HydroDataCenterEntity.Models.Site.GetAllHydroPost();
            return View(theSiteList);
        }

    }
}
