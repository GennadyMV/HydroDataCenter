using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HydroDataCenterWeb.Controllers
{
    public class ChartController : Controller
    {
        //
        // GET: /Chart/Level

        public ActionResult Level(int SiteID, int SiteCode, int SiteExtID, int SiteTypeID, string SiteName)
        {
            ViewBag.SiteID = SiteID;
            ViewBag.SiteName = SiteName;
            ViewBag.SiteCode = SiteCode;

            try
            {
                var theHydro = new HydroDataCenterEntity.HydroService.HydroServiceClient();

                DateTime dateEnd = DateTime.UtcNow;
                DateTime dateBgn = dateEnd.AddDays(-5);

                ViewBag.Min = 10999;
                ViewBag.Max = 0;

                ViewBag.SeriesAGK = "";
                var ListResult = theHydro.GetDataValues(SiteExtID, dateBgn, dateEnd, 2, null, null, null);
                ViewBag.CurrentLevelAGK = -999;
                if (ListResult != null)
                {
                    foreach (var item in ListResult)
                    {
                        string series = String.Format("[Date.UTC({0}, {1}, {2}, {3}, 0, 0 ), {4}],", item.DateUTC.Year, item.DateUTC.Month-1, item.DateUTC.Day, item.DateUTC.Hour, item.Value.ToString().Replace(',','.'));
                        ViewBag.SeriesAGK += series;
                        if (ViewBag.Min > (int) item.Value)
                        {
                            ViewBag.Min = (int)item.Value;
                        }

                        if (ViewBag.Max < (int)item.Value)
                        {
                            ViewBag.Max = (int)item.Value;
                        }
                        ViewBag.CurrentLevelAGK = ListResult.Last().Value;
                    }
                }
                                               

                ViewBag.SeriesHydroPost = "";                
                var theHydroPost = HydroDataCenterEntity.Models.Site.GetByCode(SiteCode, 2 /*ГП*/);
                ViewBag.HydroPost = theHydroPost;
                ListResult = theHydro.GetDataValues(theHydroPost.ExtID, dateBgn, dateEnd, 2, null, null, null);
                ViewBag.CurrentLevelHydroPost = -999;
                if (ListResult != null)
                {
                    foreach (var item in ListResult)
                    {
                        string series = String.Format("[Date.UTC({0}, {1}, {2}, {3}, 0, 0 ), {4}],", item.DateUTC.Year, item.DateUTC.Month-1, item.DateUTC.Day, item.DateUTC.Hour, item.Value.ToString().Replace(',','.'));
                        ViewBag.SeriesHydroPost += series;

                        if (ViewBag.Min > (int)item.Value)
                        {
                            ViewBag.Min = (int)item.Value;
                        }

                        if (ViewBag.Max < (int)item.Value)
                        {
                            ViewBag.Max = (int)item.Value;
                        }

                    }

                    ViewBag.CurrentLevelHydroPost = ListResult.Last().Value; 
                }

                

                if (ViewBag.SeriesAGK == "" && ViewBag.SeriesHydroSeries == "")
                {
                    return Content("Нет данных");
                }

                return View();

            }
            catch(Exception ex)
            {
                string err = ex.Message;
                return Content(err);
            }
            
        }

    }
}
