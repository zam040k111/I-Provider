using I_Provider.BLL.Interfaces;
using I_Provider.BLL.Repositories;
using I_Provider.DAL.Entities;

namespace I_Provider.BLL.Services
{
    public class AccountService : IAccountService
    {
        private CustomerRepos repos;
        public AccountService(CustomerRepos repository)
        {
            repos = repository;
            //Event subscription
            repos.ReplenishmentEvent += ReplenishmentHandler;
            repos.TariffPlanUpdateEvent += TariffPlanUpdateHandler;
            repos.IsBlockedEvent += IsBlockedHandler;
        }
        /// <summary>
        /// Replenishment event handler
        /// </summary>
        /// <param name="customer">Customer instance</param>
        public void ReplenishmentHandler(Customer customer)
        {
            if (customer.IsBlocked)
            {
                double paymentAmount = 0;
                foreach (var tr in customer.Tariffs)
                    paymentAmount += tr.Price;
                if (customer.Tariffs.Count > 1) 
                {
                    if (customer.Tariffs.Count < 3) paymentAmount = (paymentAmount / 100) * 90;
                    else paymentAmount = (paymentAmount / 100) * 85;
                }
                if (customer.Amount > paymentAmount) 
                { 
                    customer.Amount -= paymentAmount;
                    customer.IsBlocked = false;
                }
            }
        }
        /// <summary>
        /// Tariff plan change event handler
        /// </summary>
        /// <param name="customer">Customer instance</param>
        public void TariffPlanUpdateHandler(Customer customer)
        {
            double paymentAmount = 0;
            foreach (var tr in customer.Tariffs)
                paymentAmount += tr.Price;
            if (customer.Tariffs.Count > 1)
            {
                if (customer.Tariffs.Count < 3) paymentAmount = (paymentAmount / 100) * 90;
                else paymentAmount = (paymentAmount / 100) * 85;
            }
            if (customer.Amount > paymentAmount)
            {
                customer.Amount -= paymentAmount;
                customer.IsBlocked = false;
            }
            else customer.IsBlocked = true;
        }
        /// <summary>
        /// User lock event handler
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="value">true or false</param>
        /// <returns></returns>
        public bool IsBlockedHandler(Customer customer, bool value)
        {
            if (!value)
            {
                double paymentAmount = 0;
                foreach (var tr in customer.Tariffs)
                    paymentAmount += tr.Price;
                if (customer.Tariffs.Count > 1)
                {
                    if (customer.Tariffs.Count < 3) paymentAmount = (paymentAmount / 100) * 90;
                    else paymentAmount = (paymentAmount / 100) * 85;
                }
                if (customer.Amount > paymentAmount)
                {
                    customer.Amount -= paymentAmount;
                    customer.IsBlocked = false;
                    return true;
                }
                else return false;
            }
            else { customer.IsBlocked = true; return true; }
        }
        /// <summary>
         /// Monthly payment write-off event handler
         /// </summary>
        public void ChargeOffAllCustomer()
        {
            var customers = repos.GetAll();
            foreach (var customer in customers)
            {
                if (!customer.IsBlocked)
                {
                    double paymentAmount = 0;
                    foreach (var tr in customer.Tariffs)
                        paymentAmount += tr.Price;
                    if (customer.Tariffs.Count > 1)
                    {
                        if (customer.Tariffs.Count < 3) paymentAmount = (paymentAmount / 100) * 90;
                        else paymentAmount = (paymentAmount / 100) * 85;
                    }
                    if (customer.Amount > paymentAmount)
                        customer.Amount -= paymentAmount;
                    else customer.IsBlocked = true;
                }
            }
            repos.Save();
        }
    }
}
