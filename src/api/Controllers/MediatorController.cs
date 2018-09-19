using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDDEastAnglia.Api.Controllers {
    [Route("mediator")]
    [ApiController]
    public class MediatorController : ControllerBase {

        [HttpPost("{name}")]
        public async Task<ActionResult> Handle(string name) {

        }

    }
}
