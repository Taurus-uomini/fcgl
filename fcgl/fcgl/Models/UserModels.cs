using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace fcgl.Models
{
    public class UserModels
    {
        [Key]
        public int id { get; set; }
        [Required]
        [StringLength(20)]
        [DisplayName("用户名")]
        public string username { get; set; }
        [Required]
        [DisplayName("密码")]
        public string password { get; set; }
        [Required]
        [DisplayName("手机号")]
        [RegularExpression(@"^1[3458][0-9]{9}$", ErrorMessage ="手机号格式错误！")]
        public string phone { get; set; }
        [Required]
        [DisplayName("真实姓名")]
        public string relname { get; set; }
        [Required]
        [DisplayName("身份证号")]
        [RegularExpression(@"(^[1-9]\d{5}(18|19|([23]\d))\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx]$)|(^[1-9]\d{5}\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{2}$)", ErrorMessage = "身份证号格式错误！")]
        public string idcard { get; set; }
        [Required]
        [DisplayName("详细地址")]
        public string adress { get; set; }
        [Required]
        [DisplayName("电子邮箱")]
        [RegularExpression(@"^[a-zA-Z0-9_-]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$", ErrorMessage ="电子邮箱格式错误！")]
        public string email { get; set; }
        public virtual ICollection<HousePropertyModels> houseProperty { get; set; }
        public virtual ICollection<OrderModels> Orders { get; set; }
        public virtual AreasModels area { get; set; }
    }
}