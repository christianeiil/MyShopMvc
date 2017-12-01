using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShopMVC.Models
{
    public class UsersModel
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "User Type")]
        [Required]

        public int TypeID { get; set; }

        public List<SelectListItem> UserTypes {get; set;}

        public string UserType { get; set; }

        [Display(Name = "Email Address")]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string FirstName { get; set; }
        [Display(Name = "FirstName")]
        [Required]
        [MaxLength(80)]

        public string LastName { get; set; }
        [Display(Name = "LastName")]
        [Required]
        [MaxLength(80)]

        public string Street { get; set; }
        [Display(Name = "Street")]
        [Required]
        [MaxLength(50)]

        public string Municipality{ get; set; }
        [Display(Name = "Municipality")]
        [Required]
        [MaxLength(50)]

     
        public string City { get; set; }
        [Display(Name = "City")]
        [Required]
        [MaxLength(50)]


        public string Phone { get; set; }
        [Display(Name = "Phone")]
        [Required]
        [MaxLength(12)]
        [DataType(DataType.PhoneNumber)]

    
        public string Mobile { get; set; }
        [Display(Name = "Mobile")]
        [Required]
        [MaxLength(12)]
        [DataType(DataType.PhoneNumber)]

        public string Status { get; set; }
        public DateTime DateAdded { get; set; }

        public DateTime DateModified { get; set; }




    }
}