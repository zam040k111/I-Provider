using I_Provider.BLL.Identity;
using I_Provider.BLL.Repositories;
using I_Provider.DAL.Entities;
using System;
using System.Threading.Tasks;

namespace I_Provider.BLL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationUserManager UserManager { get; }
        ApplicationRoleManager RoleManager { get; }
        CustomerRepos CustomerRepos { get; }
        TariffPlanRepos TariffPlanRepos { get; }
        TariffTypeRepos TariffTypeRepos { get; }
        Task CommitAsync();
        void Commit();
    }
}
