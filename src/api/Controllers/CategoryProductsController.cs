using DDDEastAnglia.Api.Data;
using DDDEastAnglia.Api.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDDEastAnglia.Api.Controllers {
    [Route("api/categories/{categoryId:int}/products")]
    [ApiController]
    public class CategoryProductsController : ControllerBase {
        private readonly Db _db;

        public CategoryProductsController(
            Db db
            ) {
            _db = db;
        }

        // GET api/categories/1/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get(
            int categoryId
            ) {

            if (!await _db.Categories.AnyAsync(x => x.Id == categoryId))
                return NotFound();

            var products = await _db
                .Products
                .Where(x => x.Category.Id == categoryId)
                .ToListAsync();

            return Ok(products);

        }


        // POST api/categories/1/products
        [HttpPost]
        public async Task<ActionResult<Product>> Post(int categoryId, [FromBody, Bind("Title", "Description")] Product model) {

            var category = await _db.Categories.FindAsync(categoryId);
            if (category == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _db.Products.AnyAsync(x
                => x.Category.Id == categoryId
                && x.Title.Equals(model.Title)
                ))
                return BadRequest("Duplicate");

            var newProduct = new Product {
                Title = model.Title,
                Description = model.Description,
                Category = category
            };

            _db.Products.Add(newProduct);
            await _db.SaveChangesAsync();

            return Ok(newProduct);

        }

    }
}
