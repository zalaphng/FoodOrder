using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FoodOrder.Models
{
    public class UserInformationModels
    {
        public string UserId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email!")]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Không thể để trống")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Không thể để trống")]
        public string PhoneNumber { get; set; }
    }
}