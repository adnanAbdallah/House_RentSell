
using WebAPI.Data;
using WebAPI.Data.Repo;
using  Microsoft.EntityFrameworkCore;
using WebAPI.Interfaces;
using WebAPI.Services;
using AutoMapper;
using WebAPI.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
//* Configure your context and use SQL Server as the server.
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
));
//*for repository
// builder.Services.AddScoped<ICityRepository , CityRepository>();
//* to map the content of the http 
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
//* unit of work of all the repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//* service to remove image from the cloud
builder.Services.AddScoped<IPhotoService, PhotoService>();
//*(updated) Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();
//* for to enable accessing from angular applications
builder.Services.AddCors();

var secretKey = builder.Configuration.GetSection("AppSettings:Key").Value;
var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(secretKey));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
// services.AddAuthentication("Bearer")
    .AddJwtBearer(opt => {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKey = key                        
        };
    });



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();








var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint("/swagger/v1/swagger.json","v1");
        options.RoutePrefix = string.Empty;
        options.DocumentTitle = "My Swagger";
    });
}
//to enable connecting of frontEnd 
//!position between swagger and useAuthorization is matter
app.UseCors(options =>
options
.WithOrigins("http://localhost:4200")
.AllowAnyMethod()
.AllowAnyHeader()
);
//*
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
