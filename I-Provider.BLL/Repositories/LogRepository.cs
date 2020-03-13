using I_Provider.BLL.Logger;
using I_Provider.BLL.Logger.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace I_Provider.BLL.Repositories
{
    public class LogRepository
    {
        private LoggerContext db;
        public LogRepository(LoggerContext context)
        {
            db = context;
        }

        public void Add(Log entity)
        {
            db.Logs.Add(entity);
        }
        public IEnumerable<Log> GetAll()
        {
            return db.Logs;
        }

        public void Commit()
        {
            db.SaveChanges();
        }
    }
}
