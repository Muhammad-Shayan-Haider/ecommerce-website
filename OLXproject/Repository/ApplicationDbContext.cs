using Microsoft.AspNet.Identity.EntityFramework;
using Models;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public string City { get; set; }
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Order> Order { get; set; }


        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Models.RoleViewModel> RoleViewModels { get; set; }

        public System.Data.Entity.DbSet<Models.Item> Items { get; set; }

        public System.Data.Entity.DbSet<Models.Category> Categories { get; set; }

        public DbSet<Review> Review { get; set; }
        public DbSet<Rating> Rating { get; set; }

        public DbSet<WishList> WishList { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<SecondHandItems> SecondHandItems { get; set; }

    }
}
