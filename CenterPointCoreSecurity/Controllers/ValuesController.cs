using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CenterPointCoreSecurity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CenterPointCoreSecurity.Controllers
{
    [Authorize(Roles = "SysAdmin")]
    //[Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        private ApplicationDBContext context;

        public ValuesController(ApplicationDBContext context)
        {
            this.context = context;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return context.Users.Select(u => u.UserName).ToArray();
            //return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
