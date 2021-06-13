using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class Review
    {
        [Key]
        [Required]
        public int ReviewId { get; set; }

        [Required]
        public string text { get; set; }

        public DateTime reviewDate { get; set; }

        public string uId { get; set; }
        [ForeignKey("uId")]
        public virtual ApplicationUser ApplicationUser { get; set;}

        public int itemId { get; set; }
        [ForeignKey("itemId")]
        public virtual Item Item { get; set; }




    }
}
