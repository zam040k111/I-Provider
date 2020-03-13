using I_Provider.DAL.Entities;
using I_Provider.BLL.Identity;
using I_Provider.BLL.Interfaces;
using I_Provider.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace I_Provider.WEB.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    public class AdminController : Controller
    {
        private IUnitOfWork data;
        private ApplicationSignInManager _signInManager;
        public AdminController(IUnitOfWork unitOfWork) { data = unitOfWork; }

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
            return RedirectToAction("Customers", "Admin");
        }

        [HttpGet]
        public ActionResult Customers()
        {
            return View(data.CustomerRepos);
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult AddNewCustomer()
        {
            return View();
        }
        /// <summary>
        /// Register new customer
        /// </summary>
        /// <param name="model">Model for registration</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddNewCustomer(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, PhoneNumber = model.PhoneNumber };
                var result = await data.UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (model.Role == null)
                    {
                        data.UserManager.AddToRole(user.Id, "Customer");
                        data.CustomerRepos.Add(new Customer() { UserId = user, IsBlocked = true, Address = model.Address });
                    }
                    else
                    {
                        if(model.Role == "Admin") data.UserManager.AddToRole(user.Id, "Admin");
                        else if(model.Role == "Manager")data.UserManager.AddToRole(user.Id, "Manager");
                    }
                    data.Commit();
                    if (User.IsInRole("Admin") || User.IsInRole("Manager"))
                        return RedirectToAction("Customers", "Admin");
                    else return RedirectToAction("ChangeTariffPlan", "Customer");
                }
                AddErrors(result);
            }

            return View(model);
        }
        /// <summary>
        /// Change property "isBlocked" in database
        /// </summary>
        /// <param name="id">Customer Id</param>
        /// <param name="value">true or false</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SetIsBlocked(int? id, bool value)
        {
            try
            {
                bool res = data.CustomerRepos.SetIsBlocked((int)id, value);
                if (!res) return HttpNotFound();
                data.Commit();
                return RedirectToAction("Customers", "Admin");
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
        }
        [HttpGet]
        public ActionResult AddTariffPlan(int? id)
        {
            try
            {
                ViewBag.TariffTypes = data.TariffTypeRepos.GetAll();
                ViewBag.UserId = id;
                if (id != null)
                    ViewBag.Customer = data.CustomerRepos.GetById((int)id);
                return View();
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
        }
        /// <summary>
        /// Add tariff plan
        /// </summary>
        /// <param name="tariffPlanViewModel">Tariff plan model</param>
        /// <param name="userId">Customer Id (valid null)</param>
        /// <param name="Types">Array of tariff types Id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddTariffPlan(TariffPlanViewModel tariffPlanViewModel, int? userId, int[] Types)
        {
            try
            {
                if (Types == null) ModelState.AddModelError("DiscWillEnd", "Укажите хотябы один тип тарифа");
                if (ModelState.IsValid)
                {
                    TariffPlan ob = new TariffPlan();
                    ob.Description = tariffPlanViewModel.Description;
                    ob.ShortDesc = tariffPlanViewModel.ShortDesc;
                    ob.DateTime = DateTime.Now;
                    if (tariffPlanViewModel.DiscWillEnd == null)
                        ob.DiscWillEnd = DateTime.Now;
                    else
                        ob.DiscWillEnd = (DateTime)tariffPlanViewModel.DiscWillEnd;
                    for (int i = 0; i < Types.Length; i++)
                        ob.Types.Add(data.TariffTypeRepos.GetById(Types[i]));
                    double fullPrice = 0;
                    foreach (var type in ob.Types)
                        fullPrice += type.DiscountPrice;
                    if (ob.Types.Count > 1)
                    {
                        if (ob.Types.Count == 2)
                            ob.Price = (fullPrice / 100) * 90;
                        else ob.Price = (fullPrice / 100) * 85;
                    }
                    else ob.Price = Math.Round(fullPrice, 1);
                    if (userId != null)
                        ob.Customers.Add(data.CustomerRepos.GetById((int)userId));
                    data.TariffPlanRepos.Add(ob);
                    data.Commit();
                    return RedirectToAction("TariffPlans", "Admin");
                }
                ViewBag.TariffTypes = data.TariffTypeRepos.GetAll();
                ViewBag.UserId = userId;
                if (userId != null)
                    ViewBag.Customer = data.CustomerRepos.GetById((int)userId);
                return View(tariffPlanViewModel);
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
        }
        [HttpGet]
        public ActionResult AddTariffType()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddTariffType(TariffTypeViewModel tariffTypeViewModel)
        {
            try
            {
                if ((tariffTypeViewModel.NumberHD == null ? 0: tariffTypeViewModel.NumberHD) > 
                    (tariffTypeViewModel.NumberChannels == null ? 0: tariffTypeViewModel.NumberChannels))
                    ModelState.AddModelError("NumberHD", "Количество HD каналов не может быть больше обычных каналов");
                if (ModelState.IsValid)
                {
                    var ob = new TariffType();
                    ob.DiscountPrice = tariffTypeViewModel.DiscountPrice;
                    ob.NumberChannels = tariffTypeViewModel.NumberChannels == null ? 0 : (int)tariffTypeViewModel.NumberChannels;
                    ob.NumberHD = tariffTypeViewModel.NumberHD == null ? 0 : (int)tariffTypeViewModel.NumberHD;
                    ob.Speed = tariffTypeViewModel.Speed == null ? 0 : (int)tariffTypeViewModel.Speed;
                    ob.TypeName = tariffTypeViewModel.TypeName;
                    data.TariffTypeRepos.Add(ob);
                    data.Commit();
                    return RedirectToAction("TariffTypes", "Admin");
                }
                return View(tariffTypeViewModel);
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
        }
        [HttpGet]
        public ActionResult HideDeletedPlEvent(bool value)
        {
            Session["hideDeletePlan"] = value;
            return RedirectToAction("TariffPlans", "Admin");
        }
        [HttpGet]
        public ActionResult HideDeletedTpEvent(bool value)
        {
            Session["hideDeleteType"] = value;
            return RedirectToAction("TariffTypes", "Admin");
        }
        [HttpGet]
        public ActionResult TariffPlans()
        {
            ViewBag.TariffPlans = data.TariffPlanRepos.GetAll().Where(n => n.IsDeleted == false);
            if (Session["hideDeletePlan"] == null)
                Session["hideDeletePlan"] = true;
            else if ((bool)Session["hideDeletePlan"] == false)
                ViewBag.TariffPlans = data.TariffPlanRepos.GetAll();
            return View();
        }
        [HttpGet]
        public ActionResult TariffTypes()
        {
            ViewBag.TariffTypes = data.TariffTypeRepos.GetAll().Where(n => n.IsDeleted == false);
            if (Session["hideDeleteType"] == null)
                Session["hideDeleteType"] = true;
            else if ((bool)Session["hideDeleteType"] == false)
                ViewBag.TariffTypes = data.TariffTypeRepos.GetAll();
            return View();
        }
        [HttpGet]
        public ActionResult EditTariffPlan(int? id)
        {
            try
            {
                ViewBag.TariffTypes = data.TariffTypeRepos.GetAll();
                TariffPlan model = new TariffPlan();
                if (id != null)
                {
                    ViewBag.Id = id;
                    model = data.TariffPlanRepos.GetById((int)id);
                    ViewBag.PlanTypes = model.Types;
                }
                return View(new TariffPlanViewModel()
                {
                    Description = model.Description,
                    ShortDesc = model.ShortDesc,
                    DiscWillEnd = model.DiscWillEnd
                });
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
        }
        /// <summary>
        /// Edit tariff plan
        /// </summary>
        /// <param name="tariffPlanViewModel">Tariff plan model</param>
        /// <param name="id">Tariff plan Id</param>
        /// <param name="Types">Array of tariff types Id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditTariffPlan(TariffPlanViewModel tariffPlanViewModel, int? id, int[] Types)
        {
            try
            {
                if (Types == null) ModelState.AddModelError("DiscWillEnd", "Укажите хотябы один тип тарифа");
                if (ModelState.IsValid)
                {
                    TariffPlan ob = data.TariffPlanRepos.GetById((int)id);
                    ob.Description = tariffPlanViewModel.Description;
                    ob.ShortDesc = tariffPlanViewModel.ShortDesc;
                    ob.DateTime = DateTime.Now;
                    if (tariffPlanViewModel.DiscWillEnd == null)
                        ob.DiscWillEnd = DateTime.Now;
                    else
                        ob.DiscWillEnd = (DateTime)tariffPlanViewModel.DiscWillEnd;
                    ob.Types = new List<TariffType>();
                    for (int i = 0; i < Types.Length; i++)
                        ob.Types.Add(data.TariffTypeRepos.GetById(Types[i]));
                    double fullPrice = 0;
                    foreach (var type in ob.Types)
                        fullPrice += type.DiscountPrice;
                    if (ob.Types.Count > 1)
                    {
                        if (ob.Types.Count == 2)
                            ob.Price = (fullPrice / 100) * 90;
                        else ob.Price = (fullPrice / 100) * 85;
                    }
                    else ob.Price = Math.Round(fullPrice,1);
                    data.TariffPlanRepos.Update(ob);
                    data.Commit();
                    return RedirectToAction("TariffPlans", "Admin");
                }
                ViewBag.TariffTypes = data.TariffTypeRepos.GetAll();
                ViewBag.Id = id;
                ViewBag.PlanTypes = data.TariffPlanRepos.GetById((int)id).Types;
                return View(tariffPlanViewModel);
}
            catch (Exception)
            {
                return HttpNotFound();
            }
        }
        [HttpGet]
        public ActionResult EditTariffType(int? id)
        {
            try
            {
                TariffType model;
                if (id != null)
                    model = data.TariffTypeRepos.GetById((int)id);
                else return HttpNotFound();
                ViewBag.Id = id;
                return View(
                    new TariffTypeViewModel()
                    {
                        TypeName = model.TypeName,
                        NumberChannels = model.NumberChannels,
                        NumberHD = model.NumberHD,
                        Speed = model.Speed,
                        DiscountPrice = model.DiscountPrice
                    }
                );
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
        }
        /// <summary>
        /// Edit tariff type
        /// </summary>
        /// <param name="tariffTypeViewModel">Tariff type model</param>
        /// <param name="id">Tariff type Id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditTariffType(TariffTypeViewModel tariffTypeViewModel, int? id)
        {
            try
            {
                if ((tariffTypeViewModel.NumberHD == null ? 0 : tariffTypeViewModel.NumberHD) >
                    (tariffTypeViewModel.NumberChannels == null ? 0 : tariffTypeViewModel.NumberChannels))
                    ModelState.AddModelError("NumberHD", "Количество HD каналов не может быть больше обычных каналов");
                if (ModelState.IsValid)
                {
                    var ob = data.TariffTypeRepos.GetById((int)id);
                    ob.DiscountPrice = tariffTypeViewModel.DiscountPrice;
                    ob.NumberChannels = tariffTypeViewModel.NumberChannels == null ? 0 : (int)tariffTypeViewModel.NumberChannels;
                    ob.NumberHD = tariffTypeViewModel.NumberHD == null ? 0 : (int)tariffTypeViewModel.NumberHD;
                    ob.Speed = tariffTypeViewModel.Speed == null ? 0 : (int)tariffTypeViewModel.Speed;
                    ob.TypeName = tariffTypeViewModel.TypeName;
                    data.TariffTypeRepos.Update(ob);
                    data.Commit();
                    return RedirectToAction("TariffTypes", "Admin");
                }
                ViewBag.Id = id;
                return View(tariffTypeViewModel);
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
        }
        /// <summary>
        /// Change property "isDeleted" in database
        /// </summary>
        /// <param name="id">Tariff plan Id</param>
        /// <param name="value">true of false</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SetIsDeletedPlan(int? id, bool value)
        {
            try
            {
                data.TariffPlanRepos.SetIsDeleted((int)id, value);
                data.Commit();
                return RedirectToAction("TariffPlans", "Admin");
            }
            catch (Exception e) { return HttpNotFound(e.Message); }
        }
        /// <summary>
        /// Irrevocable removal
        /// </summary>
        /// <param name="id">Tariff plan Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeletePlan(int? id)
        {
            try
            {
                data.TariffPlanRepos.Delete((int)id);
                data.Commit();
                return RedirectToAction("TariffPlans", "Admin");
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
        }
        [HttpGet]
        public ActionResult SetIsDeletedType(int? id, bool value)
        {
            try
            {
                data.TariffTypeRepos.SetIsDeleted((int)id, value);
                data.Commit();
                return RedirectToAction("TariffTypes", "Admin");
            }
            catch (Exception e) { return HttpNotFound(e.Message); }
        }
        /// <summary>
        /// Irrevocable removal
        /// </summary>
        /// <param name="id">Tariff type Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteType(int? id)
        {
            try
            {
                data.TariffTypeRepos.Delete((int)id);
                data.Commit();
                return RedirectToAction("TariffTypes", "Admin");
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        /// <summary>
        /// Withdrawing a monthly payment from all users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ChargeOffAllCustomer()
        {
            data.CustomerRepos.ChargeOffAllCustomer();
            data.Commit();
            return RedirectToAction("Customers", "Admin");
        }
        /// <summary>
        /// Replenish account by Admin or Manager
        /// </summary>
        /// <param name=customer id></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ReplenishAccount(int? id)
        {
            if (id == null) return HttpNotFound();
            ViewBag.Id = id;
            return View();
        }

        [HttpPost]
        public ActionResult ReplenishAccount(ReplenishAdminViewModel model, int? id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    data.CustomerRepos.ReplenishmentAccount(data.CustomerRepos.GetById((int)id), model.Amount);
                    data.Commit();
                    return RedirectToAction("Customers", "Admin");
                }
                catch (Exception)
                {
                    return HttpNotFound();
                }
            }
            else
            {
                ViewBag.Id = id;
                return View(model);
            }
        }
    }
}