using I_Provider.DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace I_Provider.BLL.Identity
{
    public class ApplicationRoleManager : RoleManager<IdentityRole>
    {
        public ApplicationRoleManager(RoleStore<IdentityRole> store)
                    : base(store)
        { }
        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options,
                                         IOwinContext context)
        {
            return new ApplicationRoleManager(new
                    RoleStore<IdentityRole>(context.Get<ApplicationDbContext>()));
        }
    }
}
