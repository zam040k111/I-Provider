using I_Provider.DAL.Entities;
using I_Provider.BLL.Identity;
using I_Provider.BLL.Interfaces;
using I_Provider.BLL.Repositories;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Threading.Tasks;
using I_Provider.DAL;
using I_Provider.BLL.Logger;

namespace I_Provider.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext db;

        private ApplicationUserManager userManager;
        private ApplicationRoleManager roleManager;

        private TariffPlanRepos tariffPlanRepos;
        private TariffTypeRepos tariffTypeRepos;
        private CustomerRepos customerRepos;

        private bool disposed = false;
        public UnitOfWork(ApplicationDbContext context)
        {
            db = context;
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                if(userManager == null)
                    userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
                return userManager;
            }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                if (roleManager == null)
                    roleManager = new ApplicationRoleManager(new RoleStore<IdentityRole>(db));
                return roleManager;
            }
        }

        public TariffPlanRepos TariffPlanRepos
        {
            get
            {
                if (tariffPlanRepos == null)
                    tariffPlanRepos = new TariffPlanRepos(db);
                return tariffPlanRepos;
            }
        }

        public TariffTypeRepos TariffTypeRepos
        {
            get
            {
                if (tariffTypeRepos == null)
                    tariffTypeRepos = new TariffTypeRepos(db);
                return tariffTypeRepos;
            }
        }

        public CustomerRepos CustomerRepos
        {
            get
            {
                if (customerRepos == null)
                    customerRepos = new CustomerRepos(db);
                return customerRepos;
            }
        }

        public void Commit()
        {
            db.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await db.SaveChangesAsync();
        }
        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                    db.Dispose();
                disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
