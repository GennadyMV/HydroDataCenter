using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydroDataCenterEntity.Models
{
    public class Site
    {
        public virtual int ID { get; set; }
        public virtual DateTime created_at { get; set; }
        public virtual DateTime updated_at { get; set; }
        public virtual string Name { get; set; }
        public virtual int Code { get; set; }
        public virtual int ExtID { get; set; }
        public virtual int TypeID { get; set; }
        public virtual string TypeName { get; set; }
        public virtual string TypeNameShort { get; set; }
        public virtual float ControlProcent { get; set; }
        public virtual void Save()
        {
            this.created_at = DateTime.Now;
            this.updated_at = DateTime.Now;
            HydroDataCenterEntity.Common.IRepository<Site> repo = new Repositories.SiteRepository();

            repo.Save(this);
        }

        public virtual void Update()
        {
            this.updated_at = DateTime.Now;
            HydroDataCenterEntity.Common.IRepository<Site> repo = new Repositories.SiteRepository();
            repo.Update(this);
        }
        public static Site GetByCode(int SiteCode, int SiteType)
        {
            Repositories.SiteRepository repo = new Repositories.SiteRepository();
            return repo.GetByCode(SiteCode, SiteType);
        }

        public static List<Site> GetAll()
        {
            HydroDataCenterEntity.Common.IRepository<Site> repo = new Repositories.SiteRepository();
            return (List<Site>)repo.GetAll();
        }
        public static List<Site> GetAllAGK()
        {
            var repo = new Repositories.SiteRepository();
            return (List<Site>)repo.GetAllAGK();
        }
        public static List<Site> GetAllHydroPost()
        {
            var repo = new Repositories.SiteRepository();
            return (List<Site>)repo.GetAllHydroPost();
        }

        static public void SupportControlAgk()
        {
            var ListAgk = Site.GetAllAGK();
            try
            {

                DateTime dateEnd = DateTime.Now;
                DateTime dateBgn = dateEnd.AddDays(-7);

                var theHydro = new HydroService.HydroServiceClient();
                // 2 - ГП
                // 6 - АГК


                foreach(var site in ListAgk)
                {
                    try
                    {

                            System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\HydroDataCenterAgkControl\\" + site.Code.ToString() + "-" + site.Name.Replace("/", "-") + ".txt");

                            string line = site.Name;

                            file.WriteLine(line);
                
                            var ListResultAgk = theHydro.GetDataValues(site.ExtID, dateBgn, dateEnd, 2, null, null, null);

                            var theHydroPost = Site.GetByCode(Convert.ToInt32(site.Code), 2 /*ГП*/);    
                            var ListResultHydroPost = theHydro.GetDataValues(theHydroPost.ExtID, dateBgn, dateEnd, 2, null, null, null);

                            int CountValue = 0;
                            int SummaControlProcent = 0;

                            foreach (var value_hydropost in ListResultHydroPost)
                            {
                                #region stat
                                if (value_hydropost.Date.Hour == 8 || value_hydropost.Date.Hour == 20)
                                {
                                    line = value_hydropost.Date.ToString();
                                    line += "\t";
                                    line += value_hydropost.Value.ToString();

                                    string agk_value = "";
                                    if (ListResultAgk == null)
                                    {
                                        agk_value = "AGK not found";
                                        
                                        line += "\t";
                                        line += agk_value;
                                        file.WriteLine(line);
                                        continue;
                                    }
                                    var value_agk_list = ListResultAgk.Where(x => x.Date.Year == value_hydropost.Date.Year && x.Date.Month == value_hydropost.Date.Month && x.Date.Day == value_hydropost.Date.Day && x.Date.Hour == value_hydropost.Date.Hour).ToList();

                                    if (value_agk_list == null || value_agk_list.Count() == 0)
                                    {
                                        agk_value = "AGK not data";
                                        
                                        line += "\t";
                                        line += agk_value;
                                        file.WriteLine(line);
                                        continue;

                                    }
                                    
                                    var value_agk = value_agk_list.Last();


                                    agk_value = value_agk.Value.ToString() ;

                                    if (value_agk != null)
                                    {
                                        CountValue++;
                                        float value_control = Math.Abs(value_hydropost.Value - value_agk.Value);
                                        int coeff = 0;
                                        if (value_control < 1)
                                        {
                                                coeff = 100;
                                                SummaControlProcent += coeff;
                                        }
                                        else
                                        {
                                            if (value_control < 3)
                                            {
                                                coeff = 75;
                                                SummaControlProcent += coeff;
                                            }
                                            else 
                                                if (value_control < 4)
                                                {
                                                    
                                                    coeff = 50;
                                                    SummaControlProcent += coeff;
                                                }
                                                else
                                                {
                                                    
                                                    coeff = 0;
                                                    SummaControlProcent += coeff;
                                                }
                                        }


                                        line += "\t";
                                        line += agk_value;
                                        
                                        line += "\t";
                                        line += value_control.ToString();

                                        line += "\t";
                                        line += coeff.ToString();
                                        file.WriteLine(line);

                                    }

                                }
                                #endregion                         
                            }

                            


                            if (CountValue == 0)
                            {
                                continue;
                            }

                            site.ControlProcent = SummaControlProcent / CountValue;

                            site.Update();

                            line = site.ControlProcent.ToString();
                            file.WriteLine("{0} / {1} = {2}", SummaControlProcent, CountValue, line);

                            file.Close();
                    }
                    catch
                    {
                        site.ControlProcent = -999;

                    }
                }


            }
            catch (Exception ex)
            {
                string err = ex.Message;
                if (ex.InnerException != null)
                {
                    err += "\n\r" + ex.InnerException.Message;
                }
                Console.WriteLine(err);
            }
        }
        static private void SupportSitesUpdateSite(HydroService.HydroServiceClient theHydro, int theType)
        {
            try
            {   
                foreach (var site in theHydro.GetSiteList(theType))
                {
                    HydroDataCenterEntity.Models.Site theSite = null;
                    int site_code = Convert.ToInt32(site.SiteCode);
                    theSite = Site.GetByCode(site_code, site.Type.Id);
                    if (theSite == null)
                    {
                        theSite = new Site();
                    }
                    theSite.Code = Convert.ToInt32(site.SiteCode);
                    theSite.Name = site.Name;
                    theSite.ExtID = site.SiteId;
                    theSite.TypeID = site.Type.Id;
                    theSite.TypeName = site.Type.Name;
                    theSite.TypeNameShort = site.Type.ShortName;
                    if (theSite.ID == 0)
                    {
                        theSite.Save();
                    }
                    else
                    {
                        theSite.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                if (ex.InnerException != null)
                {
                    err += "\n\r" + ex.InnerException.Message;
                }
                Console.WriteLine(err);
            }

        }
        public static void UpdateSites()
        {
            try
            {
                var theHydro = new HydroService.HydroServiceClient();
                // 2 - ГП
                // 6 - АГК
                Site.SupportSitesUpdateSite(theHydro, 2);
                Site.SupportSitesUpdateSite(theHydro, 6);
            }
            catch(Exception ex)
            {
                string err = ex.Message;
                if (ex.InnerException != null)
                {
                    err += "\n\r" + ex.InnerException.Message;
                }
                Console.WriteLine(err);
            }
        }
    }
}
