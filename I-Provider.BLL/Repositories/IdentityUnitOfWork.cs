using I_Provider.DAL.Entities;
using I_Provider.DAL.Identity;
using I_Provider.DAL.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace I_Provider.DAL.Repositories
{
    public class IdentityUnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext db;
        public ApplicationUserManager UserManager { get; }
        public ApplicationRoleManager RoleManager { get; }

        public IdentityUnitOfWork(ApplicationDbContext dbContext)
        {
            db = dbContext;
            UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            RoleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(db));
        }

        public void Commit()
        {
            db.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    UserManager.Dispose();
                    RoleManager.Dispose();
                }
                disposed = true;
            }
        }
    }
}
