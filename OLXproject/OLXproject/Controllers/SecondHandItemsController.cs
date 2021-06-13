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
using Models.Models;

namespace OLXproject.Controllers
{
    public class SecondHandItemsController : Controller
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();
        // GET: SecondHandItems
        public ActionResult SecondHandSection()
        {
            return View();
        }
        public async Task<ActionResult> Index()
        {
            var items = ctx.SecondHandItems.Include(i => i.Category).Include(j => j.ApplicationUser);
            return View(await items.ToListAsync());
        }

        public ActionResult PostItem()
        {
            ViewBag.cId = new SelectList(ctx.Categories, "categoryID", "name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PostItem(SecondHandItems item)
        {
            if (ModelState.IsValid == true)
            {
                string fileName = Path.GetFileNameWithoutExtension(item.ImageFile.FileName);
                string extension = Path.GetExtension(item.ImageFile.FileName);

                HttpPostedFileBase postedFile = item.ImageFile;

                int length = postedFile.ContentLength; // length has image size in bytes

                if (extension.ToLower() == ".jpeg" || extension.ToLower() == ".jpg" || extension.ToLower() == ".png")
                {
                    if (length <= 1000000) //1mb
                    {

                        fileName = fileName + extension;
                        item.imgPath = "~/images/" + fileName;
                        fileName = Path.Combine(Server.MapPath("~/images/"), fileName);
                        item.ImageFile.SaveAs(fileName);

                        ctx.SecondHandItems.Add(item);
                        await ctx.SaveChangesAsync();

                        ModelState.Clear();
                        return RedirectToAction("Index", "SecondHandItems");
                    }
                }
            }
            return View();

        }
    }
}