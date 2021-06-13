using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Order
    {
        [Key]
        public int orderId { get; set; }
        public DateTime orderDate { get; set; }

        public int total { get; set; }

        public string uId { get; set; }
        [ForeignKey("uId")]
        public virtual ApplicationUser ApplicationUser
        {
            get; set;
        }

        public virtual ICollection<OrderDetail> orderDetail { get; set; }

        public string DeliveryAddress { get; set; }
    }
}
