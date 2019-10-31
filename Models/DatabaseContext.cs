using Microsoft.EntityFrameworkCore;

namespace WeddingPlanner.Models
{
    public class DatabaseContext : DbContext
    {
        // base() calls the parent class' constructor
        // passing the "options" parameter along
        public DatabaseContext(DbContextOptions options) : base(options) { }
        // This property is refers to all the data in the table as a List of Objects.
        // The *name must match the table(case insensitive).
        public DbSet<User> Users {get;set;}
        public DbSet<Wedding> Weddings {get;set;}
        public DbSet<Relation> Relations { get; set; }
        // This Class will likely have DbSet properties for every table in your database
    }
}