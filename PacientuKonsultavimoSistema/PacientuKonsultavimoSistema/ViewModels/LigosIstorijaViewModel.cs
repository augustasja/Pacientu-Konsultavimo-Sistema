using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PacientuKonsultavimoSistema.Models
{
    public class LigosIstorijaViewModel
    {
        [Required]
        public string Liga { get; set; }
        [Required]
        public string LigosAprasas { get; set; }

    }
}
