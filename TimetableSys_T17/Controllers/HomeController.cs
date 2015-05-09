using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimetableSys_T17.Models;

namespace TimetableSys_T17.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {

            //var login = "poo";//new HomeModel { deptName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase((String)TempData["deptLogin"])};

            return View();
        }
	}
}