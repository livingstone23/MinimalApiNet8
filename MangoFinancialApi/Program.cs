using MangoDomain.EntititesTest;
using Microsoft.AspNetCore.Cors;



var builder = WebApplication.CreateBuilder(args);
var origenPermited = builder.Configuration.GetValue<string>("origenPermited")!;

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

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
        options.AddPolicy("free",configuration =>
        {
            configuration
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });

    //Enable the use of CACHE, here activate the cache for all the endpoints
    builder.Services.AddOutputCache();

    //End of services area


    var app = builder.Build();
    //Start area of middleware configuration


    //Enable CORS
    app.UseCors();


    //Enable the use of CACHE
    app.UseOutputCache();
    

    //Habilito en endpoint libre acceso
    app.MapGet("/", [EnableCors(policyName: "free")]() => "!Hello World!");


    app.MapGet("/genders",() => 
    {
        var genders = new List<Gender>
        {
            new Gender {Id = 1, Name = "Action"},
            new Gender {Id = 2, Name = "Comedy"},
            new Gender {Id = 3, Name = "Horror"}
        };

        return genders;
    }).CacheOutput(c => c.Expire(TimeSpan.FromHours(1)));
        


    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();


    




//End area of middleware configuration
app.Run();


