using MangoDomain.EntititesTest;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;



namespace MangoFinancialApi.Data;



/// <summary>
/// Habilitamos paquete Microsoft.AspNetCore.Identity.EntityFrameworkCore para utilizar la
/// IdentityDbContext
/// </summary>
public class ApplicationDbContext : IdentityDbContext //DbContext  
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


        //It enable to personalize the name of the table Identity
        //First installa the package Microsoft.AspNetCore.Identity.EntityFrameworkCore
        modelBuilder.Entity<IdentityUser>().ToTable("Users");
        modelBuilder.Entity<IdentityRole>().ToTable("Rols");
        modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RolsClaims");
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UsersClaims");
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UsersLogins");
        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UsersRols");
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UsersTokens");
        



    }


    public DbSet<Gender> Genders { get; set; }

    public DbSet<Actor> Actors { get; set; }

    public DbSet<Movie> Movies { get; set; }

    public DbSet<Comment> Comments { get; set; }

    public DbSet<GenderMovie> GenderMovies { get; set; }

    public DbSet<ActorMovie> ActorMovies { get; set; }
    
}