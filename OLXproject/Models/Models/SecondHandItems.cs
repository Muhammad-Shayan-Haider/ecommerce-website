using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Models.Models
{
    public class SecondHandItems
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
        [Display(Name = "Seller's Address")]
        public string location { get; set; }

        public string ReasonForSelling { get; set; }

        public string Condition { get; set; }

        public int cId { get; set; }
        [ForeignKey("cId")]
        public virtual Category Category { get; set; }

        public string uId { get; set; }
        [ForeignKey("uId")]
        public virtual ApplicationUser ApplicationUser
        {
            get; set;
        }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; } //cant map this to database



    }
}
