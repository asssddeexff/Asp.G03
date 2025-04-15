using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;

namespace Service.Abstractions
{
    public interface IProductService
    {
        //Get All Product
        //Task<IEnumerable<ProductResultDto>> GetAllProductAsync(int? brandId, int? typeId, string? sort, int PageIndex = 1, int PageSize = 5);
        Task<PaginationResponse<ProductResultDto>> GetAllProductAsync(ProductSpecificationsParameters specsParams);
        //Get Product By Id

        Task<ProductResultDto?> GetProductByIdAsync(int id);
        //Get All Types
        Task<IEnumerable<TypeResultDto>> GetAllTypesAsync();
        //Get All Brands
        Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync();

    }
}
