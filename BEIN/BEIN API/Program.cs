using BEIN_API.UtilityPrograms;
using BEIN_DL.Data;
using BEIN_RL.IRepositories;
using BEIN_RL.Repositories;
using BEIN_ServerSide_SL.IServerSideServices;
using BEIN_ServerSide_SL.ServerSideServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BEIN_DL.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IAccount, AccountRepo>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAdminFunctions, AdminFunctionsRepo>();
builder.Services.AddScoped<IAdminFunctionsService, AdminFunctionsService>();
builder.Services.AddScoped<ISector, SectorRepo>();
builder.Services.AddScoped<ISectorService, SectorService>();

builder.Services.AddSingleton<JwtUtility>();

builder.Services.AddDbContext<BeinDbContext>(options
    => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("BEIN API"))
);

builder.Services.AddDbContext<SignInDbContext>(options
    => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("BEIN API"))
);

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>().AddEntityFrameworkStores<SignInDbContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]!))
        };

        options.Events = new()
        {
            OnMessageReceived = context =>
            {
                context.Token = context.HttpContext.Request.Cookies["AuthToken"];
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddControllers().AddJsonOptions(options
    => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.InitializeIdentityAsync();
await app.InitializeSectorAsync(app.Services.CreateScope().ServiceProvider.GetRequiredService<IWebHostEnvironment>());

app.Run();
