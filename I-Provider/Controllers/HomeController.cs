using I_Provider.BLL.Identity;
using I_Provider.BLL.Interfaces;
using Microsoft.AspNet.Identity.Owin;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace I_Provider.Controllers
{
    public class HomeController : Controller
    {
        private IUnitOfWork data;
        private ApplicationSignInManager _signInManager;
        public HomeController(IUnitOfWork unitOfWork) { data = unitOfWork; }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                if (_signInManager == null)
                    _signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
                return _signInManager;
            }
        }
        /// <summary>
        /// Page shows all tariff plans
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(data.TariffPlanRepos.GetAll());
        }
        /// <summary>
        /// Tariff plan sorter
        /// </summary>
        /// <returns></returns>
        public ActionResult TariffPlansView()
        {
            if (Session["sort"] == null)
                return View(data.TariffPlanRepos.GetAll());
            else 
            {
                if ((string)Session["sort"] == "price")
                    return View(data.TariffPlanRepos.GetAll().OrderBy(n => n.Price));
                return View(data.TariffPlanRepos.GetAll().OrderBy(n => n.ShortDesc));
            }
        }

        [HttpGet]
        public ActionResult SortByName()
        {
            Session["sort"] = "name";
            return RedirectToAction("TariffPlansView", "Home");
        }

        [HttpGet]
        public ActionResult SortByPrice()
        {
            Session["sort"] = "price";
            return RedirectToAction("TariffPlansView", "Home");
        }

        [HttpGet]
        public ActionResult SortByDate()
        {
            Session["sort"] = null;
            return RedirectToAction("TariffPlansView", "Home");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
        /// <summary>
        /// Create view tariff plans as pdf file
        /// </summary>
        /// <returns>PDF file</returns>
        [HttpGet]
        public FileResult GetPDF()
        {
            data.TariffPlanRepos.TariffsToPDF();
            var f = new FileStream(Server.MapPath(@"\Content\TariffsPDF\Tariff plans.pdf"), FileMode.Open);
            return File(f, "application/pdf", "Tariff plans.pdf");
        }
    }
}