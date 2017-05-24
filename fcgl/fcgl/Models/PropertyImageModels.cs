using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace fcgl.Models
{
    public class PropertyImageModels
    {
        [Key]
        public int id { get; set; }
        public string url { get; set; }
        [Required]
        public virtual HousePropertyModels houseProperty { get; set; }
    }
}