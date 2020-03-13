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
    public class CustomerRepos : IRepository<Customer>
    {
        private IAccountService service;
        public delegate void AccountHandler(Customer subject);
        public delegate bool AccountBlockHandler(Customer subject, bool value);
        public event AccountHandler ReplenishmentEvent;
        public event AccountHandler TariffPlanUpdateEvent;
        public event AccountBlockHandler IsBlockedEvent;
        private ApplicationDbContext db;
        public CustomerRepos(ApplicationDbContext context)
        {
            db = context;
            db.Customers.Include(t => t.Tariffs).Load();
            db.Customers.Include(t => t.UserId).Load();
            service = new AccountService(this);
        }
        /// <summary>
        /// Deposit with an event invoke
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="amount"></param>
        public void ReplenishmentAccount(Customer customer, double amount)
        {
            customer.Amount += amount;
            Update(customer);
            ReplenishmentEvent?.Invoke(customer);
        }
        /// <summary>
        /// Tariff plan update with an event invoke
        /// </summary>
        /// <param name="customer">The instance want to change</param>
        /// <param name="newTariffPlans">Collection of tariff plans</param>
        public void TariffPlanUpdate(Customer customer, IEnumerable<TariffPlan> newTariffPlans)
        {
            customer.Tariffs = newTariffPlans.ToList();
            Update(customer);
            TariffPlanUpdateEvent?.Invoke(customer);
        }
        public void Add(Customer item)
        {
            db.Customers.Add(item);
        }

        public void Delete(int id)
        {
            foreach (var item in db.Customers)
                if (item.Id == id)
                {
                    db.Customers.Remove(item);
                    break;
                }
        }

        public IEnumerable<Customer> GetAll()
        {
            return db.Customers;
        }

        public Customer GetById(int id)
        {
            try
            {
                return db.Customers.First(i => i.Id == id);
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

        public void Update(Customer item)
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
        /// Change property "isBlocked" in database
        /// </summary>
        /// <param name="id">Customer Id</param>
        /// <param name="value">true or false</param>
        /// <returns></returns>
        public bool SetIsBlocked(int id, bool value)
        {
            return (bool)IsBlockedEvent?.Invoke(GetById(id), value);
        }
        public void ChargeOffAllCustomer() { service.ChargeOffAllCustomer(); }
    }
}
