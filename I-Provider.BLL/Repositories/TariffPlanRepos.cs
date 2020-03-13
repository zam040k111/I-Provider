using I_Provider.DAL.Entities;
using I_Provider.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using I_Provider.DAL;
using I_Provider.BLL.Services;

namespace I_Provider.BLL.Repositories
{
    public class TariffPlanRepos : IRepository<TariffPlan>
    {
        private ApplicationDbContext db;
        private PrintService prtService;
        public TariffPlanRepos(ApplicationDbContext context)
        {
            db = context;
            db.TariffPlans.Include(t => t.Types).Load();
            db.TariffPlans.Include(t => t.Customers).Load();
            prtService = new PrintService();
        }
        public void Add(TariffPlan item)
        {
            db.TariffPlans.Add(item);
        }

        public void Delete(int id)
        {
            db.TariffPlans.Remove(GetById(id));
        }

        public IEnumerable<TariffPlan> GetAll()
        {
            return db.TariffPlans;
        }

        public TariffPlan GetById(int id)
        {
            try
            {
                return db.TariffPlans.First(i => i.Id == id);
            }
            catch (Exception)
            {
                throw new Exception("Not found");
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(TariffPlan item)
        {
            try
            {
                db.Entry(item).State = EntityState.Modified;
            }
            catch (Exception)
            {
                throw new Exception("Not found");
            }
        }
        /// <summary>
        /// Change property "isDelited" in database
        /// </summary>
        /// <param name="id">Tariff plan Id</param>
        /// <param name="value">true of false</param>
        public void SetIsDeleted(int id, bool value)
        {
            GetById(id).IsDeleted = value;
        }
        public void TariffsToPDF() { prtService.TariffPlansToPDF(this); }
    }
}
