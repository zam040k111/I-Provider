using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace I_Provider.DAL.Entities
{
    public class TariffPlan
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string ShortDesc { get; set; }
        public double Price { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime DiscWillEnd { get; set; }
        public virtual ICollection<TariffType> Types { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public TariffPlan()
        {
            Types = new List<TariffType>();
            Customers = new List<Customer>();
        }
    }
}
