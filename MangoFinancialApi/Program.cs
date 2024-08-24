using System.Net.NetworkInformation;
using MangoDomain.EntititesTest;
using MangoFinancialApi.Data;
using MangoFinancialApi.Enpoints;
using MangoFinancialApi.Repository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);
var origenPermited = builder.Configuration.GetValue<string>("origenPermited")!;

    // Add services to the container.

    //Enable the use of the database
    builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

    //Enable the auto mapper
    builder.Services.AddAutoMapper(typeof(Program));


    //End of services area


    var app = builder.Build();
    //Start area of middleware configuration


    if(builder.Environment.IsDevelopment()){}


    //Enable the use of swagger
    app.UseSwagger();
    app.UseSwaggerUI(); //Enable the use of swagger UI
   
    
    //Enable CORS
    app.UseCors();


    //Enable the use of CACHE
    app.UseOutputCache();
    

    //Habilito en endpoint libre acceso
    app.MapGet("/", [EnableCors(policyName: "free")]() => "!Hello World!");


    //Enable the class of Endpoints
    app.MapGroup("/genders").MapGenders();

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




