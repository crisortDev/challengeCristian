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

var builder = WebApplication.CreateBuilder(args); // Crea una nueva instancia de WebApplication (el punto de inicio de la app)

// Configuración de Swagger para documentar la API
builder.Services.AddEndpointsApiExplorer(); // Permite la exploración de los endpoints de la API
builder.Services.AddSwaggerGen();           // Añade el servicio para generar la documentación Swagger

// Configura la conexión a la base de datos SQLite desde el archivo de configuración
var sqliteConnection = builder.Configuration.GetConnectionString("Sqlite")
                      ?? "Data Source=challenge.db";  // Si no encuentra el connection string en el archivo de configuración, utiliza uno por defecto

// Añade la infraestructura necesaria, configurando la conexión a la base de datos
builder.Services.AddInfrastructure(sqliteConnection);

// Configura AutoMapper para el mapeo entre objetos (por ejemplo, entre DTOs y entidades)
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly); // Carga los perfiles de mapeo

// Añade el servicio de logging para registrar eventos, errores, etc.
builder.Services.AddLogging();

// Configura los servicios de dominio/aplicación. Estos servicios son implementaciones de la lógica de negocio
builder.Services.AddScoped<IUserService, Core.Services.UserService>();   // Añade el servicio de usuario
builder.Services.AddScoped<ITaskService, TaskService>();                // Añade el servicio de tareas
builder.Services.AddScoped<IEventRepository, EventRepository>();        // Añade el repositorio de eventos
builder.Services.AddScoped<IEventDispatcher, EventDispatcher>();        // Añade el despachador de eventos

// Crea la aplicación web
var app = builder.Build();

// Aplica las migraciones de la base de datos al iniciar la aplicación
using (var scope = app.Services.CreateScope())  // Crea un scope para resolver dependencias
{
    var db = scope.ServiceProvider.GetRequiredService<Infra.Data.AppDbContext>(); // Obtiene el contexto de la base de datos
    db.Database.Migrate();  // Aplica las migraciones pendientes (crea la base de datos si no existe)
}

// Añade un middleware para manejar las excepciones no controladas
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configura Swagger en el entorno de desarrollo para acceder a la documentación de la API
if (app.Environment.IsDevelopment()) // Si estamos en entorno de desarrollo
{
    app.UseSwagger(); // Habilita Swagger para la documentación
    app.UseSwaggerUI(); // Habilita la interfaz de usuario de Swagger
}

// Redirige automáticamente las solicitudes HTTP a HTTPS
app.UseHttpsRedirection();

// Configura los endpoints para los recursos de la API (en este caso, usuarios y tareas)
app.MapUsersEndpoints();  // Mapea los endpoints de los usuarios
app.MapTasksEndpoints();  // Mapea los endpoints de las tareas

// Ejecuta la aplicación web (inicia el servidor)
app.Run();
