using CleanArchitecture.Application.Dto;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Infrastructure.Services;
using CleanArchitecture.WebApi.OptionsSetup;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

builder.Services.ConfigureOptions<SwaggerOptionsSetup>();

builder.Services.AddDbContext<CleanArchitectureDbContext>(option =>
option.UseSqlServer("name=ConnectionStrings:Default")
);

builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<CleanArchitectureDbContext>();

var installers = typeof(Program).Assembly.ExportedTypes
    .Where(x => typeof(IServiceInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
    .Select(Activator.CreateInstance).Cast<IServiceInstaller>().ToList();

installers.ForEach(x => x.Install(builder.Services, builder.Configuration));

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<EmployeeDto>();

builder.Services.AddAutoMapper(typeof(EmployeeDto));
builder.Services.AddScoped<IEmployeeRepositry, EmployeeRepositry>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("http://example.com") .AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("corsapp");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
public partial class program { }
#pragma warning restore CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.