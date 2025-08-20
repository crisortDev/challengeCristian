using Challenge.Core.Abstraction;
using Microsoft.EntityFrameworkCore;


namespace Infra.Repositories
{
    // Clase estática que evalúa y aplica las especificaciones a las consultas de Entity Framework.
    // Se utiliza para aplicar filtros, relaciones (includes) y otros criterios de búsqueda de forma flexible.
    internal static class SpecificationEvaluator
    {
        // Método que recibe una consulta de tipo IQueryable y una especificación.
        // Aplica los criterios y los "includes" definidos en la especificación a la consulta.
        // Devuelve la consulta modificada.
        public static IQueryable<T> GetQuery<T>(IQueryable<T> input, ISpecification<T>? spec) where T : class
        {
            // Si la especificación tiene un criterio de filtro (Criteria), se aplica a la consulta.
            // Esto permite filtrar las entidades en base a una condición dinámica.
            if (spec?.Criteria != null)
                input = input.Where(spec.Criteria);

            // Si la especificación tiene relaciones para incluir (Includes), se aplican a la consulta.
            // "Includes" define las relaciones de las entidades que deben ser cargadas (por ejemplo, relaciones de navegación).
            if (spec != null)
                input = spec.Includes.Aggregate(input, (q, include) => q.Include(include));

            // Devuelve la consulta modificada, con los filtros y relaciones aplicadas.
            return input;
        }
    }
}
