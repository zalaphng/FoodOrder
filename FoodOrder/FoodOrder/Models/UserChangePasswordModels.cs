using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FoodOrder.Models
{
    public class UserChangePasswordModels
    {
        public string UserId { get; set; }

        [StringLength(50)]
        [EmailAddress]
        [Required(ErrorMessage = "Vui lòng nhập Email!")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu hiện tại!")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long", MinimumLength = 4)]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới!")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long", MinimumLength = 4)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Mật khẩu không trùng khớp!")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu không trùng khớp!")]
        public string ConfirmNewPassword { get; set; }
    }
}