using I_Provider.DAL.Entities;
using I_Provider.BLL.Identity;
using I_Provider.BLL.Interfaces;
using I_Provider.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace I_Provider.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private IUnitOfWork data;
        private ApplicationSignInManager _signInManager;
        public CustomerController(IUnitOfWork unitOfWork) { data = unitOfWork; }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                if (_signInManager == null)
                    _signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
                return _signInManager;
            }
        }
        public ActionResult Index()
        {
            Customer cust = data.CustomerRepos.GetAll().Where(n => n.UserId.UserName == User.Identity.Name).FirstOrDefault();
            return View(cust);
        }
        /// <summary>
        /// Update Tariff plan
        /// </summary>
        /// <param name="Customer Id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ChangeTariffPlan(int? id)
        {
            try
            {
                ViewBag.Customer = data.CustomerRepos.GetAll().Where(n => n.UserId.UserName.Equals(User.Identity.Name)).FirstOrDefault();
                if (id != null) ViewBag.Customer = data.CustomerRepos.GetById((int)id);
                if (ViewBag.Customer == null) return HttpNotFound();
                ViewBag.TariffPlans = data.TariffPlanRepos.GetAll().Where(n => n.IsDeleted == false);
                return View();
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
        }
        /// <summary>
        /// Update Tariff plan
        /// </summary>
        /// <param name="tariffPlansId">Tariff plans Id</param>
        /// <param name="id">Customer Id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangeTariffPlan(int[] tariffPlansId, int? id)
        {
            try
            {
                Customer cust = data.CustomerRepos.GetAll().Where(n => n.UserId.UserName.Equals(User.Identity.Name)).FirstOrDefault();
                if (id != null) cust = data.CustomerRepos.GetById((int)id);
                if (cust == null) return HttpNotFound();
                if (tariffPlansId == null || tariffPlansId.Length < 1)
                {
                    ViewBag.Message = "Тарифный план не выбран";
                    ViewBag.Customer = cust;
                    ViewBag.TariffPlans = data.TariffPlanRepos.GetAll().Where(n => n.IsDeleted == false);
                    return View();
                }
                else
                {
                    Session["cart"] = null;
                    data.CustomerRepos.TariffPlanUpdate(cust, tariffPlansId.Select(n => data.TariffPlanRepos.GetById(n)));
                    data.Commit();
                    if (User.IsInRole("Customer"))
                        return RedirectToAction("/");
                    return RedirectToAction("Customers", "Admin");
                }
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
        }
        /// <summary>
        /// Replenish account by customer
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ReplenishAccount()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ReplenishAccount(ReplenishViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    data.CustomerRepos.ReplenishmentAccount(data.CustomerRepos.GetAll()
                        .Where(n => n.UserId.UserName.Equals(User.Identity.Name))
                        .FirstOrDefault(), model.Amount);
                    data.Commit();
                    return RedirectToAction("/");
                }
                catch (Exception)
                {
                    return HttpNotFound();
                }
            }
            else
            {
                return View(model);
            }
        }
    }
}