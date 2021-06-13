using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using OLXproject;
using Models;
using OLXproject.CustomFilters;

namespace WebApplication2.Controllers
{

    public class RoleController : Controller
    {
        private ApplicaitonRoleManager _roleManager;


        public RoleController()
        {
        }

        public RoleController(ApplicaitonRoleManager roleManager)
        {
            _roleManager = roleManager;
        }

        public ApplicaitonRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicaitonRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        // GET: Role
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Index()
        {
            List<RoleViewModel> list = new List<RoleViewModel>();
            if (RoleManager.Roles != null)
            {
                foreach (var role in RoleManager.Roles)
                    list.Add(new RoleViewModel(role));
                return View(list);
            } else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [CustomAuthorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> Create(RoleViewModel model)
        {
            var role = new ApplicationRole() { Name = model.Name };
            await RoleManager.CreateAsync(role);
            return RedirectToAction("Index");
        }

        [CustomAuthorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);

            return View(new RoleViewModel(role));
        }

        [CustomAuthorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> Edit(RoleViewModel model)
        {
            var role = new ApplicationRole() { Id = model.Id, Name = model.Name };
            await RoleManager.UpdateAsync(role);
            return RedirectToAction("Index");
        }

        [CustomAuthorize(Roles = "Admin")]
        public async Task<ActionResult> Details(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);

            return View(new RoleViewModel(role));

        }

        [CustomAuthorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            return View(new RoleViewModel(role));

        }

        [CustomAuthorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            await RoleManager.DeleteAsync(role);

            return RedirectToAction("Index");

        }


    }
}