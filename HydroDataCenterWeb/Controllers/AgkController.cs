using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HydroDataCenterWeb.Controllers
{
    public class AgkController : Controller
    {
        //
        // GET: /Agk/

        public ActionResult Index()
        {
            List<HydroDataCenterEntity.Models.Site> theSiteList = HydroDataCenterEntity.Models.Site.GetAllAGK();
            return View(theSiteList);
        }

    }
}
