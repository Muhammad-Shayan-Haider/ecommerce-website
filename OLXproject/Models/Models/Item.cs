using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Models
{
    public class Item
    {
        [Key]
        public int itemID { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string name { get; set; }
        [Required]
        [Display(Name = "Price")]
        public double price { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string description { get; set; }

        [Display(Name = "Image Path")]
        public string imgPath { get; set; }

        public int cId { get; set; }
        [ForeignKey("cId")]
        public virtual Category Category { get; set; }

        [Required]
        [Display(Name = "Stock")]
        [Range(1, 20, ErrorMessage = " Quantity must be between 1 and 20 ")]
        public int quantity { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; } //cant map this to database



    }
}
