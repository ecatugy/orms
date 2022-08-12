using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Orms.Domain.DTOs;
using Orms.Domain.Interfaces;
using Orms.Domain.Interfaces.DataBase;
using Orms.Persistence.Connections;
using Orms.Persistence.Contexts;
using Orms.Persistence.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
              options
              .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
              .EnableSensitiveDataLogging(true)
              );


builder.Services.AddScoped<IApplicationDbContext>(provider =>
{
    return provider.GetService<ApplicationDbContext>() ?? throw new ArgumentException("Null provider");
});


//Dependecy injection
builder.Services.AddScoped<IApplicationWriteDbConnection, ApplicationWriteDbConnection>();
builder.Services.AddScoped<IApplicationReadDbConnection, ApplicationReadDbConnection>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IPostRepository, PostRepository>();
builder.Services.AddScoped<IValidator<PostDto>, PostDtolValidator>();
builder.Services.AddScoped<IValidator<PostDtoRepost>, PostDtoRepostValidator>();
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddControllers();



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

app.Run();



