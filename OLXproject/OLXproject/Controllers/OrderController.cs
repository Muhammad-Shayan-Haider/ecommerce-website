using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;
using Repository;
using OLXproject.CustomFilters;
using System.IO;
using Microsoft.AspNet.Identity;
using System.Data.SqlClient;

namespace OLXproject.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        private ApplicationDbContext ctx;

        public OrderController()
        {
            ctx = new ApplicationDbContext();
        }
        [Authorize]
        public ActionResult AddtoCart()
        {
            OrderDAL od = new OrderDAL();
            string userId = User.Identity.GetUserId();
            if(Session["Order"] == null)
            {
                Session["Order"] = od.GenerateOrder(User.Identity.GetUserId());
            }
            Session["Order"] = od.HandleOrder((int)Session["ItemId"],userId,(int)Session["Order"]);
            TempData["bill"] = od.getBill((int)Session["Order"]);
            return RedirectToAction("HomeIndex", "BrowseItem");
        }

        public ActionResult DecreaseQuantity(int id)
        {
            OrderDAL od = new OrderDAL();
            string userId = User.Identity.GetUserId();
            od.DecreaseQuantity(id,(int)Session["Order"]);
            TempData["bill"] = od.getBill((int)Session["Order"]);
            return RedirectToAction("CheckCart");
        }

        public ActionResult IncreaseQuantity(int id)
        {
            OrderDAL od = new OrderDAL();
            string userId = User.Identity.GetUserId();
            od.IncreaseQuantity(id, (int)Session["Order"]);
            TempData["bill"] = od.getBill((int)Session["Order"]);
            return RedirectToAction("CheckCart");
        }

        public ActionResult DeleteItem(int id)
        {
            int oId = (int)Session["Order"];
            OrderDAL od = new OrderDAL();
            od.DeleteItem(id, oId);
            TempData["Bill"] = od.getBill(oId);
            return RedirectToAction("CheckCart");
        }

        public ActionResult CheckCart()
        {
            ViewBag.cId = new SelectList(ctx.Categories, "categoryID", "name");
            OrderDAL od = new OrderDAL();
            int oId = (int)Session["Order"];
            List<OrderDetail> items = od.CartDetails(oId);
            return View(items.ToList<OrderDetail>());
        }

        public ActionResult ConfirmOrder()
        {
            return View();
        }
    }
}