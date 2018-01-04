using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MyApp.Controllers.Sample
{
  using Models;
  using Data;
  using Models.Sample;

  [EnableQuery]
  [ODataRoutePrefix("odata/Sample/Orders")]
  public partial class OrdersController : ODataController
  {
    private Data.SampleContext context;

    public OrdersController(Data.SampleContext context)
    {
      this.context = context;
    }
        // GET /odata/Sample/Orders
        //[HttpGet]
    public IEnumerable<Models.Sample.Order> Get()
    {
      var items = this.context.Orders.AsQueryable<Models.Sample.Order>();

      this.OnOrdersRead(ref items);

      return items;
    }

    partial void OnOrdersRead(ref IQueryable<Models.Sample.Order> items);

    [HttpGet("{Id}")]
    public IActionResult GetOrder(int key)
    {
        var item = this.context.Orders.Where(i=>i.Id == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnOrderDeleted(Models.Sample.Order item);

    [HttpDelete("{Id}")]
    public IActionResult DeleteOrder(int key)
    {
        var item = this.context.Orders
            .Where(i => i.Id == key)
            .Include(i => i.OrderDetails)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnOrderDeleted(item);
        this.context.Orders.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnOrderUpdated(Models.Sample.Order item);

    [HttpPut("{Id}")]
    public IActionResult PutOrder(int key, [FromBody]Models.Sample.Order newItem)
    {
        if (newItem == null || newItem.Id != key)
        {
            return BadRequest();
        }

        this.OnOrderUpdated(newItem);
        this.context.Orders.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{Id}")]
    public IActionResult PatchOrder(int key, [FromBody]JObject patch)
    {
        var item = this.context.Orders.Where(i=>i.Id == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        Data.EntityPatch.Apply(item, patch);

        this.OnOrderUpdated(item);
        this.context.Orders.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnOrderCreated(Models.Sample.Order item);

    [HttpPost]
    public IActionResult Post([FromBody] Models.Sample.Order item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnOrderCreated(item);
        this.context.Orders.Add(item);
        this.context.SaveChanges();

        return Created($"odata/Sample/Orders/{item.Id}", item);
    }
  }
}
