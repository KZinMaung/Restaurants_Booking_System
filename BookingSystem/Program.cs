using BookingSystem.Services;
using Data.Constants;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation()
   .AddJsonOptions(options =>
   {
       options.JsonSerializerOptions.PropertyNamingPolicy = null;
   });


builder.Services.AddSession(options =>
{
    //options.Cookie.Name = "ephr";  
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(360);//You can set Time   
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
           .AddCookie(o => o.LoginPath = new PathString("/authentication"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Middleware for conditional routing
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
        {
            var userType = context.User.Claims.ToArray()[AuthDataIndex.UserType].Value;
            if (userType == UserType.Restaurant)
            {
                context.Response.Redirect("/Restaurant/Index");

            }
            else
            {
                context.Response.Redirect("/Customer/Index");
            }

        }
        else
        {
            context.Response.Redirect("/Customer/Index");
        }
    }
    else
    {
        await next();
    }
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Customer}/{action=Index}/{id?}");

app.Run();
