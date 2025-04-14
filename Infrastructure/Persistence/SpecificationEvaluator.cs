﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Persistence
{
    static class SpecificationEvaluator
    {
        //Generate Query
        public static IQueryable<TEntity> GetQuery<TEntity,TKey>(IQueryable<TEntity> inputQuery,ISpecifications<TEntity,TKey> spec)
            where TEntity : BaseEntity<TKey>
        {
            var query = inputQuery;
            if (spec.Criteria is not null)
            query=    query.Where(spec.Criteria);

            if(spec.OrderBy is not null)    
                query= query.OrderBy(spec.OrderBy);
            else if(spec.OrderByDescending is not null) 
                query = query.OrderByDescending(spec.OrderByDescending);

            if(spec.IsPagination)
                query= query.Skip(spec.Skip).Take(spec.Take);

          query =  spec.IncludeExpression.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));

            return query;

        }
    }
}
// _context.Products.Include(P=>P.ProductBrand).Include(P =>P.ProductType).ToListAsync() as IEnumerable<TEntity>