using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;

namespace Presentation
{
    //Api Controller
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IServiceManager serviceManager) : ControllerBase
    {
        //endpoint : Public Non-Static Method 

        [HttpGet]//Get: /api/products

        public async Task<IActionResult> GetAllProducts()
        {
           var result = await serviceManager.ProductService.GetAllProductAsync();
            if (result is null) return BadRequest();//400
            return Ok(result);//200

        }


        [HttpGet("{id}")]//Get: /api/products/12
        public async Task <IActionResult> GetProductById(int id)
        {
          var result = await  serviceManager.ProductService.GetProductByIdAsync(id);
            if(result is null) return NotFound();//404

            return Ok(result);
        }

    }
}
