using System.ComponentModel.DataAnnotations;

namespace LesoftWuye2.Web.Models.Account
{
    public class LoginViewModel
    {
        public string TenancyName { get; set; }

        [Required]
        [Display(Name = "账号")]
        public string UsernameOrEmailAddress { get; set; }

        [Required]
        [Display(Name = "密码")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}












