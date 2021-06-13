using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace OLXproject.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly IUserRepository _user;
        private readonly ApplicationDbContext ctx = new ApplicationDbContext();

        public AdminController()
        {

        }

        public AdminController(ApplicationUserManager userManager, IUserRepository user)
        {
            this._user = user;
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Admin
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return View();
            }

            return RedirectToAction("Index", "Home");
        }


        public ActionResult getUsers()
        {
            List<ApplicationUser> list = new List<ApplicationUser>();

            if (UserManager.Users != null)
            {
                foreach (var user in UserManager.Users.ToList())
                    if (!(UserManager.IsInRole(user.Id, "Admin"))) //printing the non-admin users
                    {
                        list.Add(user);
                    }

                return View(list.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        public ActionResult Delete(String id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = _user.GetById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(String id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = _user.GetById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            var userRoles = UserManager.GetRoles(id);
            if (userRoles.Count() > 0)
            {
                foreach (var item in userRoles.ToList())
                {
                    // item should be the name of the role
                    var result = UserManager.RemoveFromRole(user.Id, item);
                }
            }

            UserManager.Delete(user);
            //UserManager.UpdateSecurityStamp(user.Id);

            return RedirectToAction("Index", "Admin");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ctx.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}