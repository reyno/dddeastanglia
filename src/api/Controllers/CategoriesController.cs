using DDDEastAnglia.Api.Data;
using DDDEastAnglia.Api.Data.Entities;
using DDDEastAnglia.Api.MediatR.Requests.Categories;
using DDDEastAnglia.Api.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDDEastAnglia.Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriesController(
            IMediator mediator
            ) {
            _mediator = mediator;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryModel>>> Get()
        {

            var categories = await _mediator.Send(new GetAllRequest());

            return Ok(categories);

        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult<Category>> Post([FromBody, Bind("Title", "Description")] Category model)
        {
            try {

                var newCategory = await _mediator.Send(new CreateRequest {
                    Title = model.Title,
                    Description = model.Description
                });

                return Ok(newCategory);

            } catch (ValidationException validationException) {
                return BadRequest(validationException);
            }


        }
    }
}
