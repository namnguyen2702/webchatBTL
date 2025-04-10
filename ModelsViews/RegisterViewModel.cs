using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using webchatBTL.ModelsViews;

namespace webchatBTL.ModelsViews
{
    public class RegisterViewModel
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Họ tên!")]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Email!")]
        [MaxLength(100)]
        [DataType(DataType.EmailAddress)]
        //[Remote(action:"ValidateEmail", controller: "Account")]
        public string Email { get; set; }

        [MaxLength(11)]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "Vui lòng số điện thoại!")]
        //[Remote(action: "ValidatePhone", controller: "Account")]
        public string Phone { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu!")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải tối thiểu 6 ký tự")]
        public string Password { get; set; }

        [MinLength(6, ErrorMessage = "Mật khẩu phải tối thiểu 6 ký tự")]
        [Display(Name = "Nhập lại mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khảu xác nhận không trùng khớp")]
        public string ConfirmPassword { get; set; }

    }
}
