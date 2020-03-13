using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace I_Provider.DAL.Entities
{
    public class TariffType
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public int NumberChannels { get; set; }
        public int Speed { get; set; }
        public double DiscountPrice { get; set; }
        public int NumberHD { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<TariffPlan> Plans { get; set; }
        public TariffType()
        {
            Plans = new List<TariffPlan>();
        }
    }
}
