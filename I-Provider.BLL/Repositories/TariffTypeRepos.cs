using I_Provider.DAL.Entities;
using I_Provider.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using I_Provider.DAL;

namespace I_Provider.BLL.Repositories
{
    public class TariffTypeRepos : IRepository<TariffType>
    {
        private ApplicationDbContext db;
        public TariffTypeRepos(ApplicationDbContext context)
        {
            db = context;
            db.TariffTypes.Include(t => t.Plans).Load();
        }
        public void Add(TariffType item)
        {
            db.TariffTypes.Add(item);
        }

        public void Delete(int id)
        {
            db.TariffTypes.Remove(GetById(id));
        }

        public IEnumerable<TariffType> GetAll()
        {
            return db.TariffTypes;
        }

        public TariffType GetById(int id)
        {
            try
            {
                return db.TariffTypes.First(i => i.Id == id);
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

        public void Update(TariffType item)
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
        /// <param name="id">Tariff type Id</param>
        /// <param name="value">true of false</param>
        public void SetIsDeleted(int id, bool value)
        {
            GetById(id).IsDeleted = value;
        }
    }
}
