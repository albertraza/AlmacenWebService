using AlmacenWebService.Entities;
using AlmacenWebService.Entities.Abstactions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlmacenWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController: ControllerBase
    {
        private readonly ICrud<Product> productsDbHandler;

        public ProductsController(ICrud<Product> productDbHandler)
        {
            this.productsDbHandler = productDbHandler;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IProduct>>> Get()
        {
            return Ok(await productsDbHandler.GetAllAsync());
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await productsDbHandler.CreateAsync(product);
            return CreatedAtRoute("getProduct", new { id = product.Id }, product);
        }

        [HttpGet("{id}", Name ="getProduct")]
        public async Task<ActionResult<IProduct>> GetProduct(int? id)
        {
            if (id == null)
                return BadRequest();

            var product = await productsDbHandler.GetAsync((int)id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int? id, [FromBody] Product product)
        {
            if (id == null)
                return BadRequest();

            if (id != product.Id)
                return BadRequest();

            await productsDbHandler.UpdateAsync(product);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            await productsDbHandler.DeleteAsync((int)id);

            return NoContent();
        }
    }
}