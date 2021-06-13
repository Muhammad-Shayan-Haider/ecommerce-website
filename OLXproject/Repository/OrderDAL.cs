using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using Models;
using Repository;
using System.IO;
using Microsoft.AspNet.Identity;
using System.Data.SqlClient;

namespace Repository
{
    public class OrderDAL
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();
        public ApplicationDbContext GetDbContext()
        {
            return ctx;
        }

        public int HandleOrder(int IID, string User, int OrID)
        {
            int itemId = IID;
            string userId = User;
            int oId = OrID;
            try
            {
                ctx.Database.SqlQuery<int>("select quantity from OrderDetails where OId = " + oId + " and ItId = " + itemId).First();
                IncreaseQuantity(IID, OrID);
            }
            catch
            {
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.ItId = itemId;
                orderDetail.OId = oId;
                orderDetail.quantity = 1;   //Quantity still needs to be configured
                ctx.OrderDetails.Add(orderDetail);
                ctx.SaveChanges();
                var itemPrice = ctx.Database.SqlQuery<double>("Select price from Items where itemID = @ItId", new SqlParameter("@ItId", itemId)).First();
                ctx.Database.ExecuteSqlCommand("update Orders set total = total + @itp where orderId = @oId",
                    new SqlParameter("@itp", itemPrice),
                    new SqlParameter("@oId", oId));
            }
            return oId;
        }

        public int getBill(int OrID)
        {
            int bill = ctx.Database.SqlQuery<int>("select total from Orders where orderId = " + OrID).First(); //new SqlParameter("@oid", oId)
            return bill;
        }

        public void DecreaseQuantity(int IID, int OrID)
        {
            int oId = OrID;
            OrderDetail orderDetail = new OrderDetail();
            var itemPrice = ctx.Database.SqlQuery<double>("Select price from Items where itemID = " + IID).First();
            ctx.Database.ExecuteSqlCommand("update Orders set total = total - @itp where orderId = @oId", new SqlParameter("@itp", itemPrice), new SqlParameter("@oId", oId));
            ctx.Database.ExecuteSqlCommand("update OrderDetails set quantity = quantity - 1 where OId = " + oId + " and ItId = " + IID);
        }

        public void IncreaseQuantity(int IID, int OrID)
        {
            int oId = OrID;
            OrderDetail orderDetail = new OrderDetail();
            var itemPrice = ctx.Database.SqlQuery<double>("Select price from Items where itemID = " + IID).First();
            ctx.Database.ExecuteSqlCommand("update Orders set total = total + @itp where orderId = @oId", new SqlParameter("@itp", itemPrice), new SqlParameter("@oId", oId));
            ctx.Database.ExecuteSqlCommand("update OrderDetails set quantity = quantity + 1 where OId = " + oId + " and ItId = " + IID);
        }

        public void DeleteItem(int IID, int OrID)
        {
            var total = ctx.Database.SqlQuery<double>("select OrderDetails.quantity * Items.price from OrderDetails join Items on OrderDetails.ItId = Items.itemID where Items.itemID = @itId and OrderDetails.OId = @oId", new SqlParameter("@itId", IID), new SqlParameter("@oId", OrID)).First();
            ctx.Database.ExecuteSqlCommand("delete from orderdetails where itid = @itid and oId = @oid", new SqlParameter("@itid", IID), new SqlParameter("@oid", OrID));
            ctx.Database.ExecuteSqlCommand("update Orders set total = total - @total where orderId = @oid", new SqlParameter("@total", total), new SqlParameter("@oid", OrID));
        }
        public int GenerateOrder(string User)
        {
            Order newOrder = new Order();
            newOrder.orderDate = DateTime.Now;
            newOrder.total = 0;
            newOrder.uId = User;
            newOrder.DeliveryAddress = null;
            ctx.Order.Add(newOrder);
            ctx.SaveChanges();  //Order Generated

            int orderId = ctx.Database.SqlQuery<int>("Select OrderId from Orders where uId = @uId and orderDate = @orderDate", new SqlParameter("@uId", newOrder.uId), new SqlParameter("@orderDate", newOrder.orderDate)).First();
            return orderId;
        }

        public List<OrderDetail> CartDetails(int oId)
        {
            return ctx.OrderDetails.Include(o => o.Item).Include(o => o.Order).Where(s => s.OId == oId).ToList<OrderDetail>();
        }
    }
}
