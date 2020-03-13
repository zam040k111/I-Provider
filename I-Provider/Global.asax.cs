using Ninject;
using Ninject.Modules;
using I_Provider.Util;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Ninject.Web.Mvc;
using System;
using I_Provider.BLL.Logger;
using I_Provider.BLL.Repositories;
using I_Provider.BLL.Logger.Entities;

namespace I_Provider
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // внедрение зависимостей
            NinjectModule registrations = new NinjectRegistrations();
            var kernel = new StandardKernel(registrations);
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }
        /// <summary>
        /// Logger
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            if (Request.RequestContext.RouteData.Values["controller"] == null)
                return;

            LogRepository logger = new LogRepository(new LoggerContext());
            string Controller = (Request.RequestContext.RouteData.Values["controller"] != null ? Request.RequestContext.RouteData.Values["controller"].ToString() : "PanelSpecifications");
            string Action = (Request.RequestContext.RouteData.Values["action"] != null ? Request.RequestContext.RouteData.Values["action"].ToString() : "Index");
            int counter = 0;
            string IP = Request.UserHostAddress;
            string UserName = User.Identity.Name;

            // full route data
            foreach (var item in Request.RequestContext.RouteData.Values)
            {
                if (item.Key.Trim().ToLower() != "controller"
                    && item.Key.Trim().ToLower() != "action")
                {
                    logger.Add(new Log()
                    {
                        Time = DateTime.Now,
                        Controller = Controller,
                        Action = Action,
                        IP = IP,
                        UserName = UserName,
                        Number = counter,
                        Field = item.Key ?? "",
                        Value = Convert.ToString(item.Value) ?? ""
                    }
                    );
                    counter++;
                }
            }

            // Request Query String
            foreach (string key in Request.QueryString.Keys)
            {
                string Value = Convert.ToString(Request.QueryString[key]);
                logger.Add(new Log()
                {
                    Time = DateTime.Now,
                    Controller = Controller,
                    Action = Action,
                    IP = IP,
                    UserName = UserName,
                    Number = counter,
                    Field = key ?? "",
                    Value = Value ?? ""
                }
                );
                counter++;
            }

            // Request Form Values
            foreach (string key in Request.Form.Keys)
            {
                string Value = Convert.ToString(Request.Form[key]);
                if (key == "Password" || key == "ConfirmPassword") Value = "********";
                logger.Add(new Log()
                {
                    Time = DateTime.Now,
                    Controller = Controller,
                    Action = Action,
                    IP = IP,
                    UserName = UserName,
                    Number = counter,
                    Field = key ?? "",
                    Value = Value ?? ""
                }
                );
                counter++;
            }
            logger.Commit();
        }
    }
}
