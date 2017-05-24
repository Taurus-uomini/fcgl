using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace fcgl.Models
{
    public class OrderModels
    {
        [Key]
        public int id { get; set; }
        public int status { get; set; }
        public DateTime startTime { get; set; }
        public DateTime cancelTime { get; set; }
        public DateTime finishTime { get; set; }
        public virtual UserModels buyer { get; set; }
        public virtual UserModels seller { get; set; }
        [Required]
        public virtual HousePropertyModels houseProperty { get; set; }
    }
}