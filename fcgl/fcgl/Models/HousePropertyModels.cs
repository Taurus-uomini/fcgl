using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace fcgl.Models
{
    public class HousePropertyModels
    {
        [Key]
        [DisplayName("房产ID")]
        public int id { get; set; }
        [Required]
        [DisplayName("房产地址")]
        public string adress { get; set; }
        [Required]
        [DisplayName("房产价格")]
        public float prize { get; set; }
        [Required]
        [DisplayName("房产介绍")]
        public string detial { get; set; }
        [DisplayName("房产证正面")]
        public string propertyUrl1 { get; set; }
        [DisplayName("房产证反面")]
        public string propertyUrl2 { get; set; }
        [DisplayName("房产证关键信息页")]
        public string propertyUrl3 { get; set; }
        public int status { get; set; }
        public virtual UserModels owner { get; set; }
        public virtual AreasModels area { get; set; }
        public virtual ICollection<OrderModels> Orders { get; set; }
        public virtual ICollection<PropertyImageModels> img { get; set; }
    }
}