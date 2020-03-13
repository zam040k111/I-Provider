using I_Provider.DAL;
using I_Provider.BLL.Interfaces;
using Ninject.Modules;
using I_Provider.BLL;
using I_Provider.BLL.Logger;
using I_Provider.BLL.Repositories;

namespace I_Provider.Util
{
    /// <summary>
    /// Dependency Injection
    /// </summary>
    public class NinjectRegistrations : NinjectModule
    {
        public override void Load()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Bind<IUnitOfWork>().To<UnitOfWork>().InSingletonScope()
                .WithConstructorArgument("context", db);
            
        }
        
    }
}