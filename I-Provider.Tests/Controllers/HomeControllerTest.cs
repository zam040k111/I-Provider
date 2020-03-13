using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using I_Provider;
using I_Provider.Controllers;
using I_Provider.BLL.Interfaces;
using I_Provider.BLL;
using I_Provider.DAL;

namespace I_Provider.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        IUnitOfWork data = new UnitOfWork(new ApplicationDbContext());
        
        [TestMethod]
        public void Contact()
        {
           HomeController controller = new HomeController(data);

           ViewResult result = controller.Contact() as ViewResult;

           Assert.IsNotNull(result);
        }
    }
}
