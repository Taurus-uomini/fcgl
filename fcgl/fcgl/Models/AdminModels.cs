using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace fcgl.Models
{
    public class AdminModels
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
        public string ip { get; set; }
        public string jurisdiction { get; set; }
    }
}