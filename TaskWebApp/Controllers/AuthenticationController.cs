using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskWcf;
using TaskWebApp.Models;

namespace TaskWebApp.Controllers
{
    public class AuthenticationController : Controller
    {
        private static ITaskService srv = WcfServiceHelper.ServiceBootstrap.TaskWcfService;
        private static NLog.Logger _log = NLog.LogManager.GetCurrentClassLogger();

        // GET: Authentication
        public ActionResult Index()
        {
            return View("Authentication");
        }

        public ActionResult UserAuthentication(string login, string password)
        {
            var userUid = srv.UserAuthentication(login, password);

            if (userUid.Code == CodeResult.Success && userUid.Data != Guid.Empty)
                return Json("IsAuthenticated", JsonRequestBehavior.AllowGet);

            return Json("IsNotAuthenticated", JsonRequestBehavior.AllowGet); ;
        }
    }
}