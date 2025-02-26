using BEIN_Web_App.ClientSideServices;
using BEIN_Web_App.IClientSideServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<IRequestService, RequestService>();
builder.Services.AddScoped<INavigationService, NavigationService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddMemoryCache();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
    options.LoginPath = "/Account/SignInOrRegister";
    options.LogoutPath = "/Account/SignOut";
    options.AccessDeniedPath = "/Home/Error";
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBeinApi", builder =>
    {
        builder.WithOrigins("https://localhost:7012")
        .AllowCredentials()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors("AllowBeinApi");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Public}/{action=LandingPage}/{id?}");

app.Run();
