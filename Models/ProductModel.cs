using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyFitness.Models
{
    public class ProductModel
    {
        [Required]
        public string PDuration { get; set; }
        [Required]
        public string PPrice { get; set; }
        [Required]
        public string PDesc { get; set; }

    }
}