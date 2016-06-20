using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HydroDataCenterEntity.Common;
using NHibernate;
using NHibernate.Criterion;

namespace HydroDataCenterEntity.Repositories
{
    public class SiteRepository : IRepository<HydroDataCenterEntity.Models.Site>
    {
        #region IRepository<Site> Members

        void IRepository<HydroDataCenterEntity.Models.Site>.Save(HydroDataCenterEntity.Models.Site entity)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(entity);
                    transaction.Commit();
                }
            }
        }

        void IRepository<HydroDataCenterEntity.Models.Site>.Update(HydroDataCenterEntity.Models.Site entity)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(entity);
                    transaction.Commit();
                }
            }
        }

        void IRepository<HydroDataCenterEntity.Models.Site>.Delete(HydroDataCenterEntity.Models.Site entity)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete(entity);
                    transaction.Commit();
                }
            }
        }

        HydroDataCenterEntity.Models.Site IRepository<HydroDataCenterEntity.Models.Site>.GetById(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.CreateCriteria<HydroDataCenterEntity.Models.Site>().Add(Restrictions.Eq("ID", id)).UniqueResult<HydroDataCenterEntity.Models.Site>();
        }

        public HydroDataCenterEntity.Models.Site GetByCode(int Code, int Type)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.CreateCriteria<HydroDataCenterEntity.Models.Site>().Add(Restrictions.Eq("Code", Code)).Add(Restrictions.Eq("TypeID", Type)).
                    UniqueResult<HydroDataCenterEntity.Models.Site>();
        }

        IList<HydroDataCenterEntity.Models.Site> IRepository<HydroDataCenterEntity.Models.Site>.GetAll()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(HydroDataCenterEntity.Models.Site));
                criteria.AddOrder(Order.Asc("Code"));
                return criteria.List<HydroDataCenterEntity.Models.Site>();
            }
        }

        public List<HydroDataCenterEntity.Models.Site> GetAllAGK()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(HydroDataCenterEntity.Models.Site));
                criteria.AddOrder(Order.Asc("Code"));
                criteria.Add(Restrictions.Eq("TypeID", 6));
                return (List<HydroDataCenterEntity.Models.Site>)criteria.List<HydroDataCenterEntity.Models.Site>();
            }
        }
        public List<HydroDataCenterEntity.Models.Site> GetAllHydroPost()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(HydroDataCenterEntity.Models.Site));
                criteria.AddOrder(Order.Asc("Code"));
                criteria.Add(Restrictions.Eq("TypeID", 2));
                return (List<HydroDataCenterEntity.Models.Site>)criteria.List<HydroDataCenterEntity.Models.Site>();
            }
        }

        #endregion
    }
}
