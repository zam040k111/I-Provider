using I_Provider.BLL.Interfaces;
using System.Web.Mvc;

namespace I_Provider.WEB.Controllers
{
    public class ErrorController : Controller
    {
        IUnitOfWork data;
        public ErrorController(IUnitOfWork unitOfWork)
        {
            data = unitOfWork;
        }
        public ActionResult NotFound(string statusDescription)
        {
            Response.StatusDescription = statusDescription;
            Response.StatusCode = 404;
            return View();
        }
        public ActionResult Forbidden(int id)
        {
            Response.StatusCode = 403;
            return View();
        }
    }
}