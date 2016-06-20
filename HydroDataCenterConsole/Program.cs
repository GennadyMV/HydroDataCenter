using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydroDataCenterConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            SupportUpdate();
        //    SupportSitesUpdate();
            SupportControl();
            Console.ReadLine();
        }

        static void SupportControl() 
        {           
            try
            {
                HydroDataCenterEntity.Models.Site.SupportControlAgk();
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

        static void SupportUpdate()
        {
            try
            {
                HydroDataCenterEntity.Common.NHibernateHelper.UpdateSchema();
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

        
        static void SupportSitesUpdate()
        {
            try
            {
                HydroDataCenterEntity.Models.Site.UpdateSites();
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
    }
}
