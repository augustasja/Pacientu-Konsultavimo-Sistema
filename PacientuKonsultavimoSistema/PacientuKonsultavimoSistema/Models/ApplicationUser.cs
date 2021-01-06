using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PacientuKonsultavimoSistema.Models
{
    // sioje klase pridedame papildomus laukus vartotojo registracijai
    public class ApplicationUser: IdentityUser
    {

        public ApplicationUser()
        {
            Messages = new HashSet<Message>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Year { get; set; }
        public string Sex { get; set; }
        public string LivingPlace { get; set; }
        public string Country { get; set; }
        public string Speciality { get; set; }
        public string Institution { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}
