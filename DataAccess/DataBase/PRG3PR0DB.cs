using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DataAccess.DataBase
{
    public partial class PRG3PR0DB : DbContext
    {
        public PRG3PR0DB()
            : base("name=PRG3PR0DB")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public virtual DbSet<Autores> Autores { get; set; }
        public virtual DbSet<Libros> Libros { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Autores>()
                .HasMany(e => e.Libros)
                .WithOptional(e => e.Autores)
                .HasForeignKey(e => e.IdAutor);
        }
    }
}
