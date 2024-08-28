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


        modelBuilder.Entity<Movie>().Property(x => x.Title).HasMaxLength(150);
        modelBuilder.Entity<Movie>().Property(x => x.PictureRoute).IsUnicode();

        
        modelBuilder.Entity<Comment>().Property(x => x.Content).HasMaxLength(500);

        modelBuilder.Entity<GenderMovie>().HasKey(x => new {x.GenderId, x.MovieId});

        modelBuilder.Entity<ActorMovie>().HasKey(x => new {x.ActorId, x.MovieId});

    }


    public DbSet<Gender> Genders { get; set; }

    public DbSet<Actor> Actors { get; set; }

    public DbSet<Movie> Movies { get; set; }

    public DbSet<Comment> Comments { get; set; }

    public DbSet<GenderMovie> GenderMovies { get; set; }

    public DbSet<ActorMovie> ActorMovies { get; set; }
    
}