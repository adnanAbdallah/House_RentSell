
using WebAPI.Data;
using WebAPI.Data.Repo;
using  Microsoft.EntityFrameworkCore;
using WebAPI.Interfaces;

var builder = WebApplication.CreateBuilder(args);
// Configure your context and use SQL Server as the server.
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
));
//for repository
builder.Services.AddScoped<ICityRepository , CityRepository>();
// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();
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
app.UseAuthorization();
app.MapControllers();
app.Run();
