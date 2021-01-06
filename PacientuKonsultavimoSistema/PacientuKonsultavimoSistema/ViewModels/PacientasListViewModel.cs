using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PacientuKonsultavimoSistema.ViewModels
{
    // Toki view matys gydytojas apie pasirinkta pacientą
    public class PacientasListViewModel
    {
        [DisplayName("Vardas")]
        public string vardas { get; set; }
        [DisplayName("Pavardė")]
        public string pavarde { get; set; }
        [DisplayName("El. Paštas")]
        public string ePastas { get; set; }
        [DisplayName("Gimimo metai")]
        public DateTime metai { get; set; }
        [DisplayName("Tel. Nr.")]
        public string telNr { get; set; }
        [DisplayName("Lytis")]
        public string lytis { get; set; }
        [DisplayName("Gyvenvietė")]
        public string gyvenviete { get; set; }
        [DisplayName("Šalis")]
        public string salis { get; set; }
        // Pakoreguoti arba sukurti nauja view kuriame pacientas mato registracijos laika ir savo priskirta gydytoja bus naudojama fk_gydytojas
    }
}
