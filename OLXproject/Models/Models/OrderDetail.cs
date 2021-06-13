using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class OrderDetail
    {
        [Key, Column(Order = 1)]
        public int OId { get; set; }
        [ForeignKey("OId")]
        public virtual Order Order { get; set; }

        [Key, Column(Order=2)]
        public int ItId { get; set; }
        [ForeignKey("ItId")]
        public virtual Item Item { get; set; }

        public int quantity { get; set; }
    }
}
