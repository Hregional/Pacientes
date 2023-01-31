using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DatosPacientes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Pacientes : ControllerBase
    {
        // GET: api/<Pacientes>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<Pacientes>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<Pacientes>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<Pacientes>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<Pacientes>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
