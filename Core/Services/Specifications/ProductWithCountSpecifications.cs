using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Shared;

namespace Services.Specifications
{
    public class ProductWithCountSpecifications : BaseSpecifications<Product,int>
    {
        public ProductWithCountSpecifications(ProductSpecificationsParameters specsParams) 
            : base(
                   P =>
                    (string.IsNullOrEmpty(specsParams.Search) || P.Name.ToLower().Contains(specsParams.Search.ToLower())) &&
                   (!specsParams.BrandId.HasValue || P.BrandId == specsParams.BrandId) &&
                   (!specsParams.TypeId.HasValue || P.TypeId == specsParams.TypeId)
                   
                  
                  )
        {
            
        }

    }
}
