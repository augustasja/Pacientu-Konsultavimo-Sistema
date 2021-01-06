using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace PacientuKonsultavimoSistema.Models
{
    public class Pacientas
    {
        [StringLength(32)]
        public string vardas { get; set; }
        [StringLength(32)]
        public string pavarde { get; set; }
        [StringLength(32)]
        public string ePastas { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime metai { get; set; }
        [Key]
        public string asmKodas { get; set; }
        [StringLength(14)]
        public string telNr { get; set; }
        [StringLength(14)]
        public string lytis { get; set; }
        [StringLength(50)]
        public string gyvenviete { get; set; }
        [StringLength(32)]
        public string salis { get; set; }
        public string UserId { get; set; }

        public ICollection<Gydytojas> Gydytojai { get; set; }

    }
}
