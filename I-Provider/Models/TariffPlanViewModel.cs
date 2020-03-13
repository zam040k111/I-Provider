using I_Provider.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace I_Provider.Models
{
    public class TariffPlanViewModel
    {
        [Required]
        [StringLength(5000, MinimumLength = 10, ErrorMessage = "Длина строки должна быть от 10 до 5000 символов")]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "Длина строки должна быть от 10 до 200 символов")]
        [Display(Name = "Краткое описане")]
        public string ShortDesc { get; set; }
        public DateTime? DiscWillEnd { get; set; }
    }
}