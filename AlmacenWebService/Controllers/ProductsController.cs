using AlmacenWebService.Entities;
using AlmacenWebService.Entities.Abstactions;
using AlmacenWebService.Models;
using AutoMapper;
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
        private readonly IMapper mapper;

        public ProductsController(ICrud<Product> productDbHandler, IMapper mapper)
        {
            productsDbHandler = productDbHandler;
            this.mapper = mapper;
        }

        [HttpGet(Name ="getProducts")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get()
        {
            var products = await productsDbHandler.GetAllAsync();
            var productsDTO = mapper.Map<List<ProductDTO>>(products);
            return productsDTO;
        }

        [HttpPost(Name ="createProduct")]
        public async Task<ActionResult> CreateProduct([FromBody] ProductCreateDTO productCreate)
        {
            var product = mapper.Map<Product>(productCreate);
            await productsDbHandler.CreateAsync(product);
            var productDTO = mapper.Map<ProductDTO>(product);
            return CreatedAtRoute("getProduct", new { id = product.Id }, productDTO);
        }

        [HttpGet("{id}", Name ="getProduct")]
        public async Task<ActionResult> GetProduct(int? id)
        {
            if (id == null)
                return BadRequest();

            var product = await productsDbHandler.GetAsync((int)id);

            if (product == null)
                return NotFound();

            var productDTO = mapper.Map<ProductDTO>(product);

            return Ok(productDTO);
        }

        [HttpPut("{id}", Name ="updateProduct")]
        public async Task<ActionResult> Update(int id, [FromBody] ProductCreateDTO productCreate)
        {
            var product = mapper.Map<Product>(productCreate);
            product.Id = id;
            await productsDbHandler.UpdateAsync(product);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await productsDbHandler.DeleteAsync(id);

            return NoContent();
        }
    }
}