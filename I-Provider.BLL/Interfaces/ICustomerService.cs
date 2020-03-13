using I_Provider.BLL.Repositories;
using I_Provider.DAL.Entities;

namespace I_Provider.BLL.Interfaces
{
    interface IAccountService
    {
        /// <summary>
        /// Replenishment event handler
        /// </summary>
        /// <param name="customer">Customer instance</param>
        void ReplenishmentHandler(Customer customer);
        /// <summary>
        /// Tariff plan change event handler
        /// </summary>
        /// <param name="customer">Customer instance</param>
        void TariffPlanUpdateHandler(Customer customer);
        /// <summary>
        /// User lock event handler
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="value">true or false</param>
        /// <returns></returns>
        bool IsBlockedHandler(Customer customer, bool value);
        /// <summary>
        /// Monthly payment write-off event handler
        /// </summary>
        void ChargeOffAllCustomer();
    }
}
