using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace fcgl.Models
{
    public class CitiesModels
    {
        [Key]
        public int id { set; get; }
        [Required]
        public string cityid { get; set; }
        [Required]
        public string city { get; set; }
        [Required]
        public string provinceid { get; set; }
    }
}