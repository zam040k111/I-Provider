using I_Provider.BLL.Logger;
using I_Provider.BLL.Logger.Entities;
using I_Provider.BLL.Repositories;
using I_Provider.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace I_Provider.Controllers
{

    [Authorize(Roles = "Manager")]
    public class ManagerController : Controller
    {
        LogRepository logger;
        public ManagerController()
        {
            logger = new LogRepository(new LoggerContext());
        }
        /// <summary>
        /// Show Logs
        /// </summary>
        /// <param name="page">Number of page</param>
        /// <param name="pageSize">Count items in page</param>
        /// <returns></returns>
        public ActionResult Index(int page = 1, int pageSize = 100)
        {
            try
            {
                if ((page-1) * pageSize > logger.GetAll().Count() || (page - 1) * pageSize < 0) return HttpNotFound();
                IEnumerable<Log> logsPerPages = SortedBy().Skip((page - 1) * pageSize).Take(pageSize);
                PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = logger.GetAll().Count() };
                LogViewModels lvm = new LogViewModels { PageInfo = pageInfo, Logs = logsPerPages };
                return View(lvm);
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
        }
        /// <summary>
        /// Sorting Logs for private using
        /// </summary>
        /// <returns>Sorted collection</returns>
        private IEnumerable<Log> SortedBy() 
        {
            if ((string)Session?["logSort"] == "UserName") return logger.GetAll().OrderBy(n => n.UserName);
            if ((string)Session?["logSort"] == "IP") return logger.GetAll().OrderBy(n => n.IP);
            if ((string)Session?["logSort"] == "Time") return logger.GetAll().OrderBy(n => n.Time);
            if ((string)Session?["logSort"] == "Controller") return logger.GetAll().OrderBy(n => n.Controller);
            if ((string)Session?["logSort"] == "Action") return logger.GetAll().OrderBy(n => n.Action);
            if ((string)Session?["logSort"] == "Field") return logger.GetAll().OrderBy(n => n.Field);
            if ((string)Session?["logSort"] == "Value") return logger.GetAll().OrderBy(n => n.Value);
            if ((string)Session?["logSort"] == "Number") return logger.GetAll().OrderBy(n => n.Number);
            return logger.GetAll();
        }
        public ActionResult SortBy(string sortBy) { Session["logSort"] = sortBy; return RedirectToAction("Index", "Manager"); }
    }
}