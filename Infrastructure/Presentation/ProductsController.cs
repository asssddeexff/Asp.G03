using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Presentation.Attributes;
using Service.Abstractions;
using Shared;
using Shared.ErrorModels;

namespace Presentation
{
    //Api Controller
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IServiceManager serviceManager) : ControllerBase
    {
        //endpoint : Public Non-Static Method 

        //Sort:nameasc[default]
        //Sort:namedesc
        //Sort:priceDesc
        //Sort:priceAsc


        [HttpGet]//Get: /api/products

        [ProducesResponseType(StatusCodes.Status200OK,Type=typeof(ProductResultDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]

        [Cache(100)]
        [Authorize]
        public async Task<ActionResult<PaginationResponse<ProductResultDto>>> GetAllProducts([FromQuery] ProductSpecificationsParameters specsParams)
        {
           var result = await serviceManager.ProductService.GetAllProductAsync(specsParams);
           
            return Ok(result);//200

        }


        [HttpGet("{id}")]//Get: /api/products/12
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationResponse<ProductResultDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
          [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        public async Task <ActionResult<ProductResultDto>> GetProductById(int id)
        {
          var result = await  serviceManager.ProductService.GetProductByIdAsync(id);
            //if(result is null) return NotFound();//404

            return Ok(result);
        }

        //TODO : Get All Brands 
        [HttpGet("brands")]//Get: /api/products/brands
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BrandResultDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task <ActionResult<BrandResultDto>> GetAllBrands()
        {
          var result = await  serviceManager.ProductService.GetAllBrandsAsync();
            if(result is null) return BadRequest(); //400
            return Ok(result);//200
        }

        //TODO : Get All Types

        [HttpGet("types")]//Get: /api/products/brands
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TypeResultDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<TypeResultDto>> GetAllTypes()
        {
            var result = await serviceManager.ProductService.GetAllTypesAsync();
            if (result is null) return BadRequest(); //400
            return Ok(result);//200
        }


    }
}
