using Challenge.Core.Abstraction;
using Challenge.Core.Domain.Entities;
using Core.Abstraction.Services;
using Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories
{
    // Clase para el manejo de los eventos relacionados con las tareas.
    // Implementa la interfaz IEventRepository para realizar las operaciones CRUD sobre la entidad TaskEvent.
    public class EventRepository : IEventRepository
    {
        private readonly AppDbContext _db; // Contexto de la base de datos

        // Constructor que recibe el contexto de base de datos para inicializar el repositorio.
        public EventRepository(AppDbContext db)
        {
            _db = db;
        }

        // Método para agregar un evento (TaskEvent) a la base de datos.
        public async Task AddEventAsync(TaskEvent taskEvent, CancellationToken ct)
        {
            // Agregar el evento al DbSet de TaskEvents
            await _db.TaskEvents.AddAsync(taskEvent, ct);
            // Guardar los cambios en la base de datos
            await _db.SaveChangesAsync(ct);
        }

        // Método para obtener un TaskEvent por su ID. Retorna null si no se encuentra el evento.
        public async Task<TaskEvent?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            // Buscar el evento con el ID proporcionado utilizando el método FindAsync
            return await _db.TaskEvents.FindAsync(new object?[] { id }, ct);
        }

        // Método para listar todos los TaskEvents, con la posibilidad de aplicar una especificación.
        // Las especificaciones permiten filtrar los resultados según ciertos criterios.
        public async Task<IReadOnlyList<TaskEvent>> ListAsync(ISpecification<TaskEvent>? spec = null, CancellationToken ct = default)
        {
            var query = _db.TaskEvents.AsQueryable(); // Obtener una consulta para TaskEvents

            // Si se pasa una especificación, aplicarla al query.
            if (spec?.Criteria != null)
            {
                query = query.Where(spec.Criteria); // Filtrar los resultados según la especificación
            }

            // Ejecutar la consulta y devolver los resultados como una lista de solo lectura
            return await query.ToListAsync(ct);
        }

        // Método para actualizar un TaskEvent existente en la base de datos.
        // Este método se marca como Task.CompletedTask porque no retorna nada.
        public Task UpdateAsync(TaskEvent entity, CancellationToken ct)
        {
            _db.TaskEvents.Update(entity); // Marcar el evento para actualización
            return Task.CompletedTask; // Operación completada
        }

        // Método para eliminar un TaskEvent de la base de datos.
        public Task DeleteAsync(TaskEvent entity, CancellationToken ct)
        {
            _db.TaskEvents.Remove(entity); // Marcar el evento para eliminación
            return Task.CompletedTask; // Operación completada
        }

        // Método no implementado (por el momento) para agregar un TaskEvent. Este método lanza una excepción si se invoca.
        public Task<TaskEvent> AddAsync(TaskEvent entity, CancellationToken ct = default)
        {
            throw new NotImplementedException(); // Lanzar una excepción porque este método no está implementado
        }
    }
}
