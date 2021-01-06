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
    public class Grafikas
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Start { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime End { get; set; }

        [StringLength(32)]
        public string GydytojasId { get; set; }
        [StringLength(32)]
        public string PacientasId { get; set; }
        [StringLength(32)]
        public string Laisvos { get; set; }

        public bool Statusas { get; set; }

        [StringLength(50)]
        public string PacVardas { get; set; }
        [StringLength(50)]
        public string PacPavarde { get; set; }
        [StringLength(50)]
        public string GydVardas { get; set; }
        [StringLength(50)]
        public string GydPavarde { get; set; }




        public Grafikas()
        {

        }

    }
}
