using DDDEastAnglia.Api.Data;
using DDDEastAnglia.Api.Data.Entities;
using DDDEastAnglia.Api.MediatR.Requests.Categories.Products;
using DDDEastAnglia.Api.Models;
using FluentValidation;
using MediatR;
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
        private readonly IMediator _mediator;

        public CategoryProductsController(
            IMediator mediator
            ) {
            _mediator = mediator;
        }

        // GET api/categories/1/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductModel>>> Get(
            int categoryId
            ) {

            try {

                var models = await _mediator.Send(new GetAllRequest {
                    CategoryId = categoryId
                });

                return Ok(models);

            } catch (ValidationException e) {
                return BadRequest(e);
            }
        }


        // POST api/categories/1/products
        [HttpPost]
        public async Task<ActionResult<ProductModel>> Post(int categoryId, [FromBody, Bind("Title", "Description")] Product model) {

            try {

                return await _mediator.Send(new CreateRequest {
                    CategoryId = categoryId,
                    Description = model.Description,
                    Title = model.Title
                });

            } catch (ValidationException e) {
                return BadRequest(e);
            }

        }

    }
}
