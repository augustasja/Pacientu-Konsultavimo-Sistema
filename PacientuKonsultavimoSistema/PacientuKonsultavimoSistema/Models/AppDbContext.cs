using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PacientuKonsultavimoSistema.Models
{
    public class AppDbContext : Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext<ApplicationUser>
    {
        //
        public AppDbContext(DbContextOptions<AppDbContext> options)
               : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Message>()
                .HasOne<ApplicationUser>(a => a.Sender)
                .WithMany(d => d.Messages)
                .HasForeignKey(d => d.UserID);
        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<Pacientas> Pacientai{ get; set; }
        public DbSet<Gydytojas> Gydytojai { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Grafikas> Grafikas { get; set; }
        public DbSet<LigosIstorija> LigosIstorija { get; set; }


    }
}
