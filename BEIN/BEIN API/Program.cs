using BEIN_API.UtilityPrograms;
using BEIN_DL.Data;
using BEIN_RL.IRepositories;
using BEIN_RL.Repositories;
using BEIN_ServerSide_SL.IServerSideServices;
using BEIN_ServerSide_SL.ServerSideServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IAccount, AccountRepo>();
builder.Services.AddScoped<IAccountService, AccountService>();

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

builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

await app.InitializeIdentityAsync();

app.Run();
