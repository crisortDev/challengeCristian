using Api.Endpoints;                     
using Challenge.Api;                 
using Challenge.Api.Middlewares;        
using Challenge.Core.Abstraction;
using Challenge.Core.Abstraction.Services;
using Challenge.Core.Services;          
using Challenge.Infra;                
using Core.Abstraction.Services;
using Core.Mapping;
using Infra.Events;
using Infra.Repositories;
using Microsoft.EntityFrameworkCore;                      

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var sqliteConnection = builder.Configuration.GetConnectionString("Sqlite")
                      ?? "Data Source=challenge.db";

builder.Services.AddInfrastructure(sqliteConnection);

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
builder.Services.AddLogging(); 

// Services de dominio/aplicación (viven en Core)
builder.Services.AddScoped<IUserService, Core.Services.UserService>();   
builder.Services.AddScoped<ITaskService, TaskService>();                
builder.Services.AddScoped<IEventRepository, EventRepository>();  
builder.Services.AddScoped<IEventDispatcher, EventDispatcher>();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Infra.Data.AppDbContext>();
    db.Database.Migrate(); 
}

// Middleware de excepciones
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapUsersEndpoints();   
app.MapTasksEndpoints(); 

app.Run();
