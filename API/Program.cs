using API.Services.Auth;
using API.Services.Customer;
using API.Services.Restaurant;
using Data.Model;
using Infra.BlobStorage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.WithOrigins("https://localhost:7156"
                            )
                          .AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
                      });
});
builder.Services.AddControllersWithViews()
                         .AddJsonOptions(options =>
                         {
                             options.JsonSerializerOptions.PropertyNamingPolicy = null;
                         });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = builder.Configuration;
// Add JWT Authentication
var jwtSettings = configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["secret"]);
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});


//Db
string connectionString = configuration["ConnectionStrings"];
var contextOptions = new DbContextOptionsBuilder<BookingSystemDbCotnext>()
             .UseSqlServer(connectionString)
             .Options;

builder.Services.AddScoped(s => new BookingSystemDbCotnext(contextOptions));
builder.Services.AddScoped<IAuth>(s => new AuthBase(
           s.GetService<BookingSystemDbCotnext>(),
            s.GetService<IAzureBlobStorage>(),
           configuration
           ));

builder.Services.AddSingleton<IAzureBlobStorage, AzureBlobStorage>();

builder.Services.AddScoped<ICustomer>(s => new CustomerBase(
           s.GetService<BookingSystemDbCotnext>(),
           s.GetService<IAzureBlobStorage>(),
           configuration
           ));


builder.Services.AddScoped<IRestaurant>(s =>
{
    var dbContext = s.GetService<BookingSystemDbCotnext>();
    var azureBlobStorage = s.GetService<IAzureBlobStorage>();
    var webHostEnvironment = s.GetService<IWebHostEnvironment>();

    return new RestaurantBase(dbContext, azureBlobStorage, configuration);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();

app.Run();
