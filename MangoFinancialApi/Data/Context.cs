using MangoDomain.EntititesTest;
using Microsoft.EntityFrameworkCore;



namespace MangoFinancialApi.Data;



public class ApplicationDbContext : DbContext
{


    public ApplicationDbContext(DbContextOptions options) : base(options)
    {

    }


    //Using apiflutter to perform the database
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);

        
        modelBuilder.Entity<Gender>().Property(x => x.Name).HasMaxLength(50);

        modelBuilder.Entity<Actor>().Property(x => x.Name).HasMaxLength(150);
        modelBuilder.Entity<Actor>().Property(x => x.PictureRoute).IsUnicode();
        

    }

    public DbSet<Gender> Genders { get; set; }

    public DbSet<Actor> Actors { get; set; }

}