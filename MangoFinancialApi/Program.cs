using System.Net.NetworkInformation;
using FluentValidation;
using MangoDomain.EntititesTest;
using MangoFinancialApi.Data;
using MangoFinancialApi.Enpoints;
using MangoFinancialApi.Migrations;
using MangoFinancialApi.Repository;
using MangoFinancialApi.Services;
using MangoFinancialApi.Utility;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);
var origenPermited = builder.Configuration.GetValue<string>("origenPermited")!;

    // Add services to the container.

    //Enable the use of the database
    builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


    //Enable the use of identity in the context, with Library IdentityCore Microsoft.AspNetCore.Authentication.JwtBearer 8.0.8 
    builder.Services.AddIdentityCore<IdentityUser>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

    //Enable UserManager y Identity
    //Reprensentation of the user of the application
    //Enable the use of the user manager
    builder.Services.AddScoped<UserManager<IdentityUser>>();
    //Enable the use of the sign in manager, for the login
    builder.Services.AddScoped<SignInManager<IdentityUser>>();


    //Habilitamos CORS
    builder.Services.AddCors(options =>
        {
            
            options.AddDefaultPolicy(configuration =>
            {
                configuration.WithOrigins(origenPermited)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });

            //Add another politics
            options.AddPolicy("free", configuration =>
            {
                configuration
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

    //Enable the use of CACHE, here activate the cache for all the endpoints
    builder.Services.AddOutputCache();

    
    builder.Services.AddEndpointsApiExplorer();


    //Enable the swagger
    builder.Services.AddSwaggerGen();


    //Add the services of the repository
    builder.Services.AddScoped<IRepositoryGender, RepositoryGender>();
    builder.Services.AddScoped<IRepositoryActor, RepositoryActor>();
    builder.Services.AddScoped<IRepositoryMovie, RepositoryMovie>();
    builder.Services.AddScoped<IRepositoryComment, RepositoryComment>();
    //builder.Services.AddScoped<IStoreFiles, StoreFilesAzure>();
    builder.Services.AddScoped<IStoreFiles, StoreFilesLocal>();
    builder.Services.AddTransient<IUserServices, UserServices>();
    
    builder.Services.AddHttpContextAccessor(); //Enable the use of the http context, for using StoreFilesLocal and PaginationDTO

    //Enable the auto mapper
    builder.Services.AddAutoMapper(typeof(Program));

    //Enable of fluent validation
    builder.Services.AddValidatorsFromAssemblyContaining<Program>();
    

    //Enable the use of the exception handler, in this case we use the problem details. #PD1
    builder.Services.AddProblemDetails();



    //Enable the security in endpoints #SEC1
    builder.Services.AddAuthentication().AddJwtBearer(opt =>    
    {

    //Enable to get the claims of the token in the controller         
    opt.MapInboundClaims = false;                //Install package Microsoft.AspNetCore.Authentication.JwtBearer 8.0.8 
    
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,                                                 //Validate the issuer(emisor)
        ValidateAudience = false,                                               //Validate the audience
        ValidateLifetime = true,                                                //Validate the lifetime of the token
        ValidateIssuerSigningKey = true,                                        //Validate the key of the issuer
        IssuerSigningKey = Keys.GetKey(builder.Configuration).First(),        //Enable the use of one unique key
        //IssuerSigningKeys = Keys.GetAllKeys(builder.Configuration),             //Enable the use of all keys
        ClockSkew = TimeSpan.Zero   
        };                                            //The time of the clock
    });

    //builder.Services.AddAuthorization();

    //Enable a policy for the authorization
    builder.Services.AddAuthorization(opt =>
    {
        //Require the claim isadmin
        opt.AddPolicy("isadmin", policy => policy
                .RequireClaim("isadmin"));
    }


    );


    //End of services area


    var app = builder.Build();
    //Start area of middleware configuration


    if(builder.Environment.IsDevelopment()){}


    //Enable the use of swagger
    app.UseSwagger();
    app.UseSwaggerUI(); //Enable the use of swagger UI
   

    //Enable the use of the exception handler . #PD2
    app.UseExceptionHandler();
    //Enable the use of status code pages   . #PD3
    app.UseStatusCodePages(); 


    app.UseStaticFiles(); //Enable the use of static files, for using StoreFilesLocal
    
    //Enable CORS
    app.UseCors();


    //Enable the use of CACHE
    app.UseOutputCache();


    //Enable the security in endpoints #SEC2
    app.UseAuthorization();
    
    

    //Habilito en endpoint libre acceso
    app.MapGet("/", [EnableCors(policyName: "free")]() => "!Hello World!");


    //Enable the class of Endpoints
    app.MapGroup("/genders").MapGenders();
    app.MapGroup("/actors").MapActors();
    app.MapGroup("/movies").MapMovies();
    app.MapGroup("/movies/{movieId:int}/comments").MapComments();
    app.MapGroup("/users").MapUsers();


    /*

    //From expression lamda to method
    
    //var endopointGenders = app.MapGroup("/genders");

    endopointGenders.MapGet("/", GetAllGenders).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("genders-get"));
    endopointGenders.MapGet("/{id:int}",GetById);
    endopointGenders.MapPost("/", CreateGender);    
    endopointGenders.MapPut("/{id:int}", UpdateGender);
    endopointGenders.MapDelete("/{id:int}",DeleteGender);
    

    endopointGenders.MapGet("/",async(IRepositoryGender repository) => 
    {
        return await repository.GetAll();
    }).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("genders-get"));


    endopointGenders.MapGet("/{id:int}",async(IRepositoryGender repository, int id) => 
    {
        var gender = await repository.GetById(id);
        if (gender is null)
        {
            return Results.NotFound();
        }
        return Results.Ok(gender); 
    });

    //Method to create gender
    
    endopointGenders.MapPost("/", async(Gender gender, IRepositoryGender repository, IOutputCacheStore outputCacheStore) => 
    {
        var id = await repository.Create(gender);

        //Clear the cache
        await outputCacheStore.EvictByTagAsync("genders-get",default);

        return Results.Created($"/genders/{id}",gender);
    });
    
    //Metodo para actualizar 
    
    endopointGenders.MapPut("/{id:int}", async(int id,Gender gender, IRepositoryGender repository, IOutputCacheStore outputCacheStore) => 
    {
        var exist = await repository.Exist(id);

        if (!exist)
        {
            return Results.NotFound();
        }

        await repository.Update(gender);

         //Clear the cache
        await outputCacheStore.EvictByTagAsync("genders-get",default);

        return Results.NoContent();

    });
    
    //Borrar un registro
    
    endopointGenders.MapDelete("/{id:int}", async(int id, IRepositoryGender repository, IOutputCacheStore outputCacheStore) => 
    {
        var exist = await repository.Exist(id);

        if (!exist)
        {
            return Results.NotFound();
        }

        await repository.Delete(id);

         //Clear the cache
        await outputCacheStore.EvictByTagAsync("genders-get",default);

        return Results.NoContent();

    });
    */
    

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();



//End area of middleware configuration
app.Run();




