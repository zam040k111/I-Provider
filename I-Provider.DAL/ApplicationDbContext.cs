using I_Provider.DAL.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace I_Provider.DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<TariffPlan> TariffPlans { get; set; }
        public DbSet<TariffType> TariffTypes { get; set; }
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false) { }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
