using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Riverbed.Test.FreshBrew.Models;
using Riverbed.Test.FreshBrewApi.Services;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Riverbed.Test.FreshBrewApi.Controllers
{
    [Route("api/[controller]")]
    public class Order2Controller : Controller
    {
        private IOrderItem orderItemData;
        readonly ILogger<Order2Controller> log;
        public Order2Controller(IOrderItem orderItem, ILogger<Order2Controller> _log)
        {
            orderItemData = orderItem;
            log = _log;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<OrderItem> Get()
        {
            log.LogInformation("Executing: HttpGet Get");
            return orderItemData.GetAll();
        }

        [HttpGet("{id}", Name = "GetOrder2Item")]
        public IActionResult GetById(int id)
        {
            log.LogInformation("Executing: HttpGet GetOrder2Item");
            var item = orderItemData.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Order item)
        {
            log.LogInformation("Executing: HttpPost GetOrder2Item");
            if (item == null)
            {
                return BadRequest();
            }
            OrderItem orderItem= orderItemData.Add(item);
            return CreatedAtRoute("GetOrder2Item", new { id = orderItem.Id }, orderItem);
           // return new ObjectResult(orderItem);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Order item)
        {
            log.LogInformation("Update is executing HttpPut Update:item.id:" + $"id={id},{item.Name},{item.DelayByInSeconds} ");
            if (item == null )
            {
                return BadRequest();
            }
            var itemInDb = orderItemData.Update(id,item);
            if (itemInDb == null)
            {
                return NotFound();
            }
            return new ObjectResult(itemInDb);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            log.LogInformation("Executing: HttpDelete Delete");
            bool result= orderItemData.Delete(id);          
            if (!result)
            {
                return NotFound();
            }            
            return new OkResult();
        }
    }
}
