using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace fcgl.Models
{
    public class InfoModels
    {
        [Key]
        public int id { get; set; }
        [Required]
        [DisplayName("网站名称")]
        public string webName { get; set; }
        [Required]
        [DisplayName("设计者")]
        public string designer { get; set; }
    }
}