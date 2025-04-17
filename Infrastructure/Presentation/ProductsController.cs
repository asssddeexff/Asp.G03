using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using Shared;

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

        public async Task<IActionResult> GetAllProducts([FromQuery] ProductSpecificationsParameters specsParams)
        {
           var result = await serviceManager.ProductService.GetAllProductAsync(specsParams);
            if (result is null) return BadRequest();//400
            return Ok(result);//200

        }


        [HttpGet("{id}")]//Get: /api/products/12
        public async Task <IActionResult> GetProductById(int id)
        {
          var result = await  serviceManager.ProductService.GetProductByIdAsync(id);
            //if(result is null) return NotFound();//404

            return Ok(result);
        }

        //TODO : Get All Brands 
        [HttpGet("brands")]//Get: /api/products/brands
        public async Task <IActionResult> GetAllBrands()
        {
          var result = await  serviceManager.ProductService.GetAllBrandsAsync();
            if(result is null) return BadRequest(); //400
            return Ok(result);//200
        }

        //TODO : Get All Types

        [HttpGet("types")]//Get: /api/products/brands
        public async Task<IActionResult> GetAllTypes()
        {
            var result = await serviceManager.ProductService.GetAllTypesAsync();
            if (result is null) return BadRequest(); //400
            return Ok(result);//200
        }


    }
}
