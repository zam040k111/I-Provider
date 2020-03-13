using I_Provider.DAL.Entities;
using I_Provider.BLL.Identity;
using I_Provider.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace I_Provider.Controllers
{
    [AllowAnonymous]
    public class CartController : Controller
    {
        private IUnitOfWork data;
        public CartController(IUnitOfWork unitOfWork) { data = unitOfWork; }
        public ActionResult Cart()
        {
            return View();
        }
        [HttpGet]
        public int AddToCart(int? id)
        {
            TariffPlan tp;
            try
            {
                tp = data.TariffPlanRepos.GetById((int)id);
            }
            catch (Exception)
            {
                throw new Exception();
            }
            if (Session["cart"] == null)
            {
                List<TariffPlan> cart = new List<TariffPlan>();
                cart.Add(tp);
                Session["cart"] = cart;
                return 1;
            }
            else
            {
                List<TariffPlan> cart = (List<TariffPlan>)Session["cart"];
                if (!cart.Contains(tp))
                    cart.Add(tp);
                Session["cart"] = cart;
                return cart.Count;
            }
        }
        [HttpGet]
        public ActionResult RemoveFromCart(int? id)
        {
            TariffPlan tp;
            try
            {
                tp = data.TariffPlanRepos.GetById((int)id);
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
            if (Session["cart"] != null)
            {
                List<TariffPlan> cart = (List<TariffPlan>)Session["cart"];
                cart.Remove(tp);
                Session["cart"] = cart;
                return RedirectToAction("Cart", "Cart");
            }
            else
            {
                return HttpNotFound();
            }

        }
    }
}