
using Core.Entities;
using Core.Specification;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity :BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery,
        ISpecification<TEntity> spac)
        {
            var query=inputQuery;
            if(spac.Criteria !=null)
            {
                query=query.Where(spac.Criteria);
            }
            query=spac.Includes.Aggregate(query,(current,include)=>current.Include(include));
            return query;
        }
    }
}