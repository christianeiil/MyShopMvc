using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
 
namespace MyShopMVC.Models
{
    public class ProductsModel
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Name")]
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Display(Name = "Category")]
        [Required]
        public int CatID { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public string Category { get; set; }

        [Display(Name = "Code")]
        [Required]
        [MaxLength(20)]
        public string Code { get; set; }

        [Display(Name = "Description")]
        [Required]
        [DataType(DataType.MultilineText)]
        [MaxLength(300)]
        public string Description { get; set; }

        [Display(Name = "Image")]
        [Required]
        public string Image { get; set; }

        [Display(Name = "Price")]
        [Required]
        [DataType(DataType.Currency)]
        [Range(1.00, 10000.00, ErrorMessage = "Invalid range.")]
        public double Price { get; set; }

        [Display(Name = "Is Featured?")]
        public bool IsFeatured { get; set; }

        [Display(Name = "Available")]
        public int Available { get; set; }

        [Display(Name = "Critical")]
        [Required]
        [Range(0, 100)]
        public int Critical { get; set; }

        [Display(Name = "Maximum")]
        [Required]
        [Range(0, 100)]
        public int Max { get; set; }

        public string Status { get; set; }

        public DateTime DateAdded { get; set; }

        [DisplayFormat(NullDisplayText = "")]
        public DateTime? DateModified { get; set; }
    }

    public enum Status
    {
        Active,
        Inactive,
        Archived
    }

    public enum Featured
    {
        Yes,
        No
    }
}