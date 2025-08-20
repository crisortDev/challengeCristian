using Challenge.Core.Abstraction;
using Infra.Data;
using Infra.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Challenge.Infra.Repositories
{
    // Clase genérica que implementa el patrón Repository para las entidades.
    // Esta clase es responsable de las operaciones CRUD básicas.
    public class EfRepository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _db;  // Contexto de base de datos

        // Constructor de la clase que recibe el contexto de base de datos.
        public EfRepository(AppDbContext db) => _db = db;

        // Método para obtener una entidad por su ID. 
        // Recibe un Guid como ID de la entidad a buscar.
        public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            // Busca la entidad de tipo T utilizando el ID proporcionado.
            // Utiliza el método FindAsync del DbSet de la entidad.
            return await _db.Set<T>().FindAsync(new object?[] { id }, ct);
        }

        // Método para listar todas las entidades de tipo T, con opción de aplicar una especificación para filtrar los resultados.
        // La especificación (ISpecification<T>) puede contener filtros o configuraciones adicionales sobre la consulta.
        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T>? spec = null, CancellationToken ct = default)
        {
            // Se obtiene la consulta inicial (query) para el DbSet de la entidad T.
            var query = SpecificationEvaluator.GetQuery(_db.Set<T>().AsQueryable(), spec);
            // Ejecuta la consulta y devuelve los resultados en una lista.
            return await query.ToListAsync(ct);
        }

        // Método para agregar una nueva entidad de tipo T a la base de datos.
        // El parámetro `entity` es la entidad que se quiere agregar.
        public async Task<T> AddAsync(T entity, CancellationToken ct = default)
        {
            // Se agrega la entidad al conjunto de entidades de tipo T.
            await _db.Set<T>().AddAsync(entity, ct);
            // Devuelve la entidad que se acaba de agregar.
            return entity;
        }

        // Método para actualizar una entidad existente de tipo T en la base de datos.
        // El parámetro `entity` es la entidad con los datos actualizados.
        public Task UpdateAsync(T entity, CancellationToken ct = default)
        {
            // Se marca la entidad para su actualización en la base de datos.
            _db.Set<T>().Update(entity);
            // Como no se devuelve nada, se retorna un Task completado.
            return Task.CompletedTask;
        }

        // Método para eliminar una entidad de tipo T de la base de datos.
        // El parámetro `entity` es la entidad que se quiere eliminar.
        public Task DeleteAsync(T entity, CancellationToken ct = default)
        {
            // Se marca la entidad para su eliminación en la base de datos.
            _db.Set<T>().Remove(entity);
            // Como no se devuelve nada, se retorna un Task completado.
            return Task.CompletedTask;
        }
    }
}
