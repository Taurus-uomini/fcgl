using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace fcgl.Models
{
    public class ProvincesModels
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string provinceid { get; set; }
        [Required]
        public string province { get; set; }
    }
}