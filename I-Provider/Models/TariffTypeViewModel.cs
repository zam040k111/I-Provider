using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace I_Provider.Models
{
    public class TariffTypeViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Длина строки должна быть от 6 до 50 символов")]
        [Display(Name = "Название")]
        public string TypeName { get; set; }
        public int? NumberChannels { get; set; }
        public int? Speed { get; set; }

        [Required]
        [Range(10, 3000, ErrorMessage = "Цена может быть от 10 грн до 3000 грн")]
        [Display(Name = "Цена")]
        public double DiscountPrice { get; set; }
        public int? NumberHD { get; set; }
    }
}