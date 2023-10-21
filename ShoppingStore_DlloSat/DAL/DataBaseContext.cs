using Microsoft.EntityFrameworkCore;
using ShoppingStore_DlloSat.DAL.Entities;

namespace ShoppingStore_DlloSat.DAL
{
    public class DataBaseContext : DbContext
    {
        //En este constructor, creo la refrencia de DbContextOptions que me sirve para configurar las opciones de la BD, como por ejemplo usar SQL Server y usar la cadena de conexión a la BD
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasIndex(c => c.Name).IsUnique(); //Yo con esta línea controlo la duplicidad de mis países
            modelBuilder.Entity<Category>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<State>().HasIndex("Name","CountryId").IsUnique(); //Compuesto Index
            modelBuilder.Entity<City>().HasIndex("Name", "StateId").IsUnique(); //Compuesto Index
        }

        //DbSet me sirve para convertir mi clase Country en una tabla de BD. El nombre de la tabla será "Countries"
        public DbSet<Country> Countries { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
    }
}
