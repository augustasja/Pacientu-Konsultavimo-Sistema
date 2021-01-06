using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PacientuKonsultavimoSistema.ViewModels
{
    // Toki view matys pacientas apie pasirinktą gydytoją
    public class GydytojasListViewModel
    {
        [DisplayName("Vardas")]
        public string vardas { get; set; }
        [DisplayName("Pavardė")]
        public string pavarde { get; set; }
        [DisplayName("Specialybė")]
        public string specialybe { get; set; }
        [DisplayName("Ligoninė")]
        public string istaiga { get; set; }
        [DisplayName("Miestas/Rajonas")]
        public string gyvenviete { get; set; }
    }
}
