using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace PacientuKonsultavimoSistema.Models
{
    public class LigosIstorija
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(450)]
        public string PacientasId { get; set; }

        [Required]
        [StringLength(450)]
        public string GydytojasId { get; set; }
        
        [Required]
        [StringLength(450)]
        public string GydSpecialybe { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Data { get; set; }
        
        [Required]
        [StringLength(450)]
        public string Liga { get; set; }

        [Required]
        public string LigosAprasas { get; set; }
        
        [Required]
        [StringLength(50)]
        public string GydVardas { get; set; }
        
        [Required]
        [StringLength(50)]
        public string GydPavarde { get; set; }


        public LigosIstorija()
        {

        }

    }
}
