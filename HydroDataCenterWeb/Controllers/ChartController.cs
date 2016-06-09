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
                        if (item.Value < -900)
                        {
                            continue;
                        }
                        
                        item.Value += Popravka(SiteCode, item.DateUTC);

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

        private int Popravka(int SiteCode, DateTime popravkaDate)
        {
            int res = 0;
            switch (SiteCode)
            {
                case 05063: // Биробиджан
                    if (popravkaDate > new DateTime(2016, 05, 28))
                    {
                        res = -13;
                    }
                    break;

                case 05024: // Комсомольск
                    if (popravkaDate > new DateTime(2016, 05, 09))
                    {
                        res = -60;
                    }
                    break;

                case 05019: // Троийкое
                    if (popravkaDate > new DateTime(2016, 05, 18))
                    {
                        res = -20;
                    }
                    break;
            }
            return res;
        }

        public ActionResult AGK(int SiteCode)
        {
            var theSite = HydroDataCenterEntity.Models.Site.GetByCode(SiteCode, 6);

            if (theSite == null)
            {
                return Content("АГК не найден " + SiteCode.ToString());
            }

            ViewBag.SiteID = theSite.ID;
            ViewBag.SiteName = theSite.Name;
            ViewBag.SiteCode = SiteCode;
            int SiteExtID = theSite.ExtID;

            try
            {
                var theHydro = new HydroDataCenterEntity.HydroService.HydroServiceClient();

                DateTime dateEnd = DateTime.UtcNow;
                DateTime dateBgn = dateEnd.AddDays(-30);

                ViewBag.Min = 10999;
                ViewBag.Max = 0;

                ViewBag.SeriesAGK = "";
                var ListResult = theHydro.GetDataValues(SiteExtID, dateBgn, dateEnd, 2, null, null, null);
                ViewBag.CurrentLevelAGK = -999;
                if (ListResult != null)
                {
                    foreach (var item in ListResult)
                    {
                        if (item.Value < -900)
                        {
                            continue;
                        }

                        item.Value += Popravka(SiteCode, item.DateUTC);

                        string series = String.Format("[Date.UTC({0}, {1}, {2}, {3}, 0, 0 ), {4}],", item.DateUTC.Year, item.DateUTC.Month - 1, item.DateUTC.Day, item.DateUTC.Hour, item.Value.ToString().Replace(',', '.'));
                        ViewBag.SeriesAGK += series;
                        if (ViewBag.Min > (int)item.Value)
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
                        string series = String.Format("[Date.UTC({0}, {1}, {2}, {3}, 0, 0 ), {4}],", item.DateUTC.Year, item.DateUTC.Month - 1, item.DateUTC.Day, item.DateUTC.Hour, item.Value.ToString().Replace(',', '.'));
                        ViewBag.SeriesHydroPost += series;

                        if (ViewBag.Min > (int)item.Value)
                        {
                            ViewBag.Min = (int)item.Value;
                        }

                        if (ViewBag.Max < (int)item.Value)
                        {
                            ViewBag.Max = (int)item.Value;
                        }

                        ViewBag.Max += 10;
                        ViewBag.Max -= 10;
                    }

                    ViewBag.CurrentLevelHydroPost = ListResult.Last().Value;
                }



                if (ViewBag.SeriesAGK == "" && ViewBag.SeriesHydroSeries == "")
                {
                    return Content("Нет данных");
                }

                return View();

            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return Content(err);
            }
        }


        public ActionResult HydroPost(int SiteCode)
        {
            var theSite = HydroDataCenterEntity.Models.Site.GetByCode(SiteCode, 2);

            if (theSite == null)
            {
                return Content("Гидрологический пост не найден " + SiteCode.ToString());
            }

            ViewBag.SiteID = theSite.ID;
            ViewBag.SiteName = theSite.Name;
            ViewBag.SiteCode = SiteCode;
            int SiteExtID = theSite.ExtID;

            try
            {
                var theHydro = new HydroDataCenterEntity.HydroService.HydroServiceClient();

                DateTime dateEnd = DateTime.UtcNow;
                DateTime dateBgn = dateEnd.AddDays(-30);
                
               
                ViewBag.SeriesHydroPost = "";
                var theHydroPost = HydroDataCenterEntity.Models.Site.GetByCode(SiteCode, 2 /*ГП*/);
                ViewBag.HydroPost = theHydroPost;
                var ListResult = theHydro.GetDataValues(theHydroPost.ExtID, dateBgn, dateEnd, 2, null, null, null);
                ViewBag.CurrentLevelHydroPost = -999;
                if (ListResult != null)
                {
                    foreach (var item in ListResult)
                    {
                        string series = String.Format("[Date.UTC({0}, {1}, {2}, {3}, 0, 0 ), {4}],", item.DateUTC.Year, item.DateUTC.Month - 1, item.DateUTC.Day, item.DateUTC.Hour, item.Value.ToString().Replace(',', '.'));
                        ViewBag.SeriesHydroPost += series;

                    }

                    ViewBag.CurrentLevelHydroPost = ListResult.Last().Value;
                }

                if (ViewBag.SeriesHydroSeries == "")
                {
                    return Content("Нет данных");
                }

                foreach (var criteria in theHydro.GetSiteCriterias(theSite.ExtID))
                {
                    switch (criteria.Type.Id)
                    {
                        case 1:
                            string series = String.Format("[Date.UTC({0}, {1}, {2}, {3}, 0, 0 ), {4}],", dateBgn.Year, dateBgn.Month - 1, dateBgn.Day, dateBgn.Hour, criteria.BeginValue.ToString().Replace(',', '.'));
                            ViewBag.SeriesCriteria01 += series;
                            series = String.Format("[Date.UTC({0}, {1}, {2}, {3}, 0, 0 ), {4}],", dateEnd.Year, dateEnd.Month - 1, dateEnd.Day, dateEnd.Hour, criteria.BeginValue.ToString().Replace(',', '.'));
                            ViewBag.SeriesCriteria01 += series;
                            ViewBag.SeriesCriteriaName01 += criteria.Type.Comment;
                            break;

                        case 2:
                            series = String.Format("[Date.UTC({0}, {1}, {2}, {3}, 0, 0 ), {4}],", dateBgn.Year, dateBgn.Month - 1, dateBgn.Day, dateBgn.Hour, criteria.BeginValue.ToString().Replace(',', '.'));
                            ViewBag.SeriesCriteria02 += series;
                            series = String.Format("[Date.UTC({0}, {1}, {2}, {3}, 0, 0 ), {4}],", dateEnd.Year, dateEnd.Month - 1, dateEnd.Day, dateEnd.Hour, criteria.BeginValue.ToString().Replace(',', '.'));
                            ViewBag.SeriesCriteria02 += series;
                            ViewBag.SeriesCriteriaName02 += criteria.Type.Comment;
                            break;

                        case 3:
                            series = String.Format("[Date.UTC({0}, {1}, {2}, {3}, 0, 0 ), {4}],", dateBgn.Year, dateBgn.Month - 1, dateBgn.Day, dateBgn.Hour, criteria.BeginValue.ToString().Replace(',', '.'));
                            ViewBag.SeriesCriteria03 += series;
                            series = String.Format("[Date.UTC({0}, {1}, {2}, {3}, 0, 0 ), {4}],", dateEnd.Year, dateEnd.Month - 1, dateEnd.Day, dateEnd.Hour, criteria.BeginValue.ToString().Replace(',', '.'));
                            ViewBag.SeriesCriteria03 += series;
                            ViewBag.SeriesCriteriaName03 += criteria.Type.Comment;
                            break;
                    }                    
                }                
                
                float forecast00 = 238;
                float forecast01 = 217;
                float forecast02 = 228;
                float forecast03 = 223;
                float forecast04 = 215;
                float forecast05 = 215;
                string series01 = String.Format("[Date.UTC({0}, {1}, {2}, {3}, 0, 0 ), {4}],", dateEnd.Year, dateEnd.Month - 1, dateEnd.AddDays(-1).Day, ListResult.Last().DateUTC.Hour, forecast00.ToString().Replace(',', '.'));
                ViewBag.SeriesForecast += series01;
                series01 = String.Format("[Date.UTC({0}, {1}, {2}, {3}, 0, 0 ), {4}],", dateEnd.AddDays(1).Year, dateEnd.AddDays(1).Month - 1, dateEnd.AddDays(1).Day, dateEnd.AddDays(1).Hour, forecast01.ToString().Replace(',', '.'));
                ViewBag.SeriesForecast += series01;
                series01 = String.Format("[Date.UTC({0}, {1}, {2}, {3}, 0, 0 ), {4}],", dateEnd.AddDays(2).Year, dateEnd.AddDays(2).Month - 1, dateEnd.AddDays(2).Day, dateEnd.AddDays(2).Hour, forecast02.ToString().Replace(',', '.'));
                ViewBag.SeriesForecast += series01;
                series01 = String.Format("[Date.UTC({0}, {1}, {2}, {3}, 0, 0 ), {4}],", dateEnd.AddDays(1).Year, dateEnd.AddDays(3).Month - 1, dateEnd.AddDays(3).Day, dateEnd.AddDays(3).Hour, forecast03.ToString().Replace(',', '.'));
                ViewBag.SeriesForecast += series01;
                series01 = String.Format("[Date.UTC({0}, {1}, {2}, {3}, 0, 0 ), {4}],", dateEnd.AddDays(4).Year, dateEnd.AddDays(4).Month - 1, dateEnd.AddDays(4).Day, dateEnd.AddDays(4).Hour, forecast04.ToString().Replace(',', '.'));
                ViewBag.SeriesForecast += series01;
                series01 = String.Format("[Date.UTC({0}, {1}, {2}, {3}, 0, 0 ), {4}],", dateEnd.AddDays(5).Year, dateEnd.AddDays(5).Month - 1, dateEnd.AddDays(5).Day, dateEnd.AddDays(5).Hour, forecast05.ToString().Replace(',', '.'));

                return View();

            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return Content(err);
            }

        }

    }
}
