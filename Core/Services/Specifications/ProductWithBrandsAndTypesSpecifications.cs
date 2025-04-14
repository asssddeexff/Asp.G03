using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Shared;

namespace Services.Specifications
{
    public class ProductWithBrandsAndTypesSpecifications : BaseSpecifications<Product,int>
    {
        public ProductWithBrandsAndTypesSpecifications(int id) : base(P=>P.Id ==id)
        {
            ApplyIncludes();
        }
        public ProductWithBrandsAndTypesSpecifications(ProductSpecificationsParameters specsParams) 
            :  base (
                   P =>
                   (!specsParams.BrandId.HasValue || P.BrandId == specsParams.BrandId) &&
                   (!specsParams.TypeId.HasValue || P.TypeId == specsParams.TypeId)
                   )
        {
            ApplyIncludes();

            ApplySorting(specsParams.Sort);
           ApplyPagination(specsParams.PageIndex, specsParams.PageSize);
        }

        private void ApplyIncludes()
        {
            AddInclude(P => P.ProductBrand);
            AddInclude(P => P.ProductType);
        }

        private void ApplySorting(string? sort)
        {
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort.ToLower())
                {
                    
                    case "namedesc":
                        AddOrderByDescending(P => P.Name);
                        break;
                    case "priceasc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "pricedesc":
                        AddOrderByDescending(P => P.Price);
                        break;

                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(P => P.Name);

            }
        }

    }
}
