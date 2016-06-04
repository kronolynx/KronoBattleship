using KronoBattleship.Datalayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KronoBattleship.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "BattleShip";
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Chat");
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}