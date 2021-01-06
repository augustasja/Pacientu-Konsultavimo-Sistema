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
    public class Gydytojas
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

        [StringLength(32)]
        public string salis { get; set; }
        [StringLength(32)]
        public string specialybe { get; set; }
        [StringLength(32)]
        public string istaiga { get; set; }

        public string UserId { get; set; }

        public ICollection<Pacientas> Pacientai { get; set; }

        public Gydytojas()
        {

        }

    }
}
