using I_Provider.BLL.Logger.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace I_Provider.BLL.Logger
{
    public class LoggerContext : DbContext
    {
        public DbSet<Log> Logs { get; set; }
        public LoggerContext()
            : base("LogDb") { }
    }
}
