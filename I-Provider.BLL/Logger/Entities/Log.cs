using I_Provider.BLL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace I_Provider.BLL.Logger.Entities
{
    public class Log
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string UserName { get; set; }
        public string IP { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public int Number { get; set; }
        public string Field { get; set; }
        public string Value { get; set; }
    }
}
