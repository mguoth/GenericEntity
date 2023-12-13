using Microsoft.AspNetCore.Mvc;
using Org.GenericEntity.Abstractions;
using Org.GenericEntity.Extensions.FileSystem;
using Org.GenericEntity.Model;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Concurrent;

namespace GenericAspNetCoreWebApi.Controllers
{
    [ApiController]
    [Route("addresses")]
    public class GenericController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ISchemaRepository _schemaRepository;

        private static readonly IDictionary<int, GenericEntity> _items = new ConcurrentDictionary<int, GenericEntity>();

        public GenericController(ILogger<GenericController> logger, ISchemaRepository schemaRepository)
        {
            _logger = logger;
            _schemaRepository = schemaRepository;
        }

        [HttpPost]
        public ActionResult<GenericEntity> Create(GenericEntity item)
        {
            _items.Add(item.Fields["id"].GetValue<int>(), item);
            return Ok(item);
        }
        
        [HttpGet]
        [Route("{id}")]
        public ActionResult<GenericEntity> Get(int id)
        {
            return Ok(_items[id]);
        }
        
        [HttpGet]
        public ActionResult<IList<GenericEntity>> List()
        {
            return Ok(_items);
        }
        
        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            _items.Remove(id);
            return NoContent();
        }
    }
}