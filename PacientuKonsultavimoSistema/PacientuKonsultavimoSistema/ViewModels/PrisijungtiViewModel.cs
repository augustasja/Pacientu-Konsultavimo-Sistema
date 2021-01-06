using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PacientuKonsultavimoSistema.Models
{
    public class PrisijungtiViewModel
    {
        [Required]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Display(Name = "Prisiminti mane")]
        public bool prisimintiMane { get; set; }

    }
}
