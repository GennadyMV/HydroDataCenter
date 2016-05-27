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
