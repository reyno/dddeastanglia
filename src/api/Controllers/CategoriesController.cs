using DDDEastAnglia.Api.Data;
using DDDEastAnglia.Api.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDDEastAnglia.Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly Db _db;

        public CategoriesController(
            Db db
            ) {
            _db = db;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> Get()
        {

            var categories = await _db.Categories.ToListAsync();

            return Ok(categories);

        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult<Category>> Post([FromBody, Bind("Title", "Description")] Category model)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _db.Categories.AnyAsync(x => x.Title.Equals(model.Title)))
                return BadRequest("Duplicate");

            var newCategory = new Category
            {
                Title= model.Title,
                Description = model.Description
            };

            _db.Categories.Add(newCategory);
            await _db.SaveChangesAsync();

            return Ok(newCategory);

        }
    }
}
