using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TimetableSys_T17.Controllers
{
    public class LoginController : Controller
    {
        private bool validateDetails(string deptIn, string password)
        {

            bool returnVal = false;

            using (var db = new TimetableDbEntities())
            {

                var user = from userDB in db.Users from dInfoDB in db.DeptInfoes where userDB.deptID == dInfoDB.deptID && dInfoDB.deptName == deptIn select userDB.email;
                
                // search for email, i.e. username, email = uniq to display. Are doing usernames or just depts?

                if (user.FirstOrDefault() != null)
                {

                    var inputPass = from userDB in db.Users from dInfoDB in db.DeptInfoes where userDB.deptID == dInfoDB.deptID && dInfoDB.deptName == deptIn select userDB.password;

                    if (inputPass.FirstOrDefault() == password)
                    {
                        returnVal = true;
                    }
                }
            }

            return returnVal;
        }

        [HttpGet]
        public ActionResult Index()
        {

            return View();

        }

        [HttpPost] // validation - rtn deptlogin from input, validate
        public ActionResult Index(Models.LoginModel deptLogin)
        {


            if (ModelState.IsValid)
            {

                if (validateDetails(deptLogin.deptIn, deptLogin.password))
                {
                    int userId = 0;
                    using (var db = new TimetableDbEntities()) { 
                    

                       var userID = from deptTable in db.DeptInfoes where deptTable.deptName == deptLogin.deptIn select deptTable.deptID;
                       userId = userID.FirstOrDefault();
                }
                    

                    /*return RedirectToRoute(new
                    {
                        Controller = "Home",
                        action = "Index",
                        userId = userId,
                        userName = deptLogin.deptIn

                    });*/

                    TempData["usrId"] = userId;
                    TempData["deptLogin"] = deptLogin.deptIn;
                    return RedirectToAction("Index", "Home"); ;

                }
                else
                {
                    ModelState.AddModelError("", "Serious bullshit is going on, you've input the wrong credentials.");
                }
            }
            return View();
        }
	}
}