using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace EsercizioSettimana11Marzo.Models
{
    public partial class ModelDbContext : DbContext
    {
        public ModelDbContext()
            : base("name=ModelDbContext")
        {
        }

        public virtual DbSet<Articolo> Articoloes { get; set; }
        public virtual DbSet<DettaglioOrdine> DettaglioOrdines { get; set; }
        public virtual DbSet<Ordine> Ordines { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<Utente> Utentes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Articolo>()
                .Property(e => e.Prezzo)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Articolo>()
                .HasMany(e => e.DettaglioOrdines)
                .WithRequired(e => e.Articolo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ordine>()
                .Property(e => e.TotaleOrdine)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Ordine>()
                .HasMany(e => e.DettaglioOrdines)
                .WithRequired(e => e.Ordine)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Utente>()
                .HasMany(e => e.Ordines)
                .WithRequired(e => e.Utente)
                .WillCascadeOnDelete(false);
        }
    }
}
