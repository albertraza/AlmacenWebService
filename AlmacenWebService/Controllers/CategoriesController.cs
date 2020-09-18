
using AlmacenWebService.Entities;
using AlmacenWebService.Entities.Abstactions;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlmacenWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController: ControllerBase
    {
        private readonly ICrud<Category> categoryHandler;

        public CategoriesController(ICrud<Category> categoryHandler)
        {
            this.categoryHandler = categoryHandler;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> Get()
        {
            return Ok(await categoryHandler.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]Category category)
        {
            await categoryHandler.CreateAsync(category);
            return CreatedAtRoute("getCategory", new { id = category.Id }, category);
        }

        [HttpGet("{id}", Name ="getCategory")]
        public async Task<ActionResult<Category>> Get(int? id)
        {
            if (id == null)
                return BadRequest();

            var category = await categoryHandler.GetAsync((int)id);

            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            await categoryHandler.DeleteAsync((int)id);

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int? id ,[FromBody] Category category)
        {
            if (id == null)
                return BadRequest();

            if (id != category.Id)
                return BadRequest();

            if (category.Id == 0)
                category.Id = (int)id;

            await categoryHandler.UpdateAsync(category);
            return Ok(category);
        }
    }
}
