using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private static List<ValueModel> Values = new List<ValueModel>(Enumerable.Range(1, 3).Select(i => new ValueModel
        {
            Id = i,
            Value = $"Value {i}"
        }));

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<ValueModel>> Get()
        {
            return Ok(Values);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<ValueModel> Get(int id)
        {

            var value = Values.SingleOrDefault(x => x.Id == id);

            if (value == null) return NotFound();

            return value;

        }

        // POST api/values
        [HttpPost]
        public ActionResult<ValueModel> Post([FromBody, Bind("Value")] ValueModel model)
        {

            if (Values.Any(x => x.Value.Equals(model.Value)))
                return BadRequest("Duplicate");

            var nextId = Values.Count == 0 ? 1 : Values.Max(x => x.Id) + 1;

            var newValue = new ValueModel
            {
                Id = nextId,
                Value = model.Value
            };

            Values.Add(newValue);

            return Ok(newValue);

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody]ValueModel model)
        {

            if (id != model.Id) return BadRequest();

            var value = Values.SingleOrDefault(x => x.Id == model.Id);

            if (value == null) return NotFound();

            value.Value = model.Value;

            return NoContent();

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {

            var value = Values.SingleOrDefault(x => x.Id == id);

            if (value == null) return NotFound();

            Values.Remove(value);

            return NoContent();

        }
    }
}
