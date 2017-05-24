using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace fcgl.Models
{
    public class AreasModels
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string areaid { get; set; }
        [Required]
        public string area { get; set; }
        [Required]
        public string cityid { get; set; }
        public virtual ICollection<HousePropertyModels> houseProperty { get; set; }
        public virtual ICollection<UserModels> user { get; set; }
    }
}