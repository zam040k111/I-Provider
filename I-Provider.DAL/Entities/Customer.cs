using System.Collections.Generic;

namespace I_Provider.DAL.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public virtual ApplicationUser UserId { get; set; }
        public double Amount { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsBlocked { get; set; }
        public string Address { get; set; }
        public virtual ICollection<TariffPlan> Tariffs { get; set; }
        public Customer()
        {
            Tariffs = new List<TariffPlan>();
        }
    }
}
