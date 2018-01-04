using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MyApp.Controllers.Northwind
{
  using Models;
  using Data;
  using Models.Northwind;

  [EnableQuery]
  [ODataRoutePrefix("odata/Northwind/Orders")]
  public partial class OrdersController : ODataController
    {
    private Data.NorthwindContext context;

    public OrdersController(Data.NorthwindContext context)
    {
      this.context = context;
    }
    // GET /odata/Northwind/Orders
    //[HttpGet]
    public IEnumerable<Models.Northwind.Order> Get()
    {
      var items = this.context.Orders.AsQueryable<Models.Northwind.Order>();

      this.OnOrdersRead(ref items);

      return items;
    }

    partial void OnOrdersRead(ref IQueryable<Models.Northwind.Order> items);

    [HttpGet("{OrderID}")]
    public SingleResult<Models.Northwind.Order> GetOrder(int key)
    {
        var items = this.context.Orders.Where(i=>i.OrderID == key);

            return SingleResult.Create(items);
        }
    partial void OnOrderDeleted(Models.Northwind.Order item);

    [HttpDelete("{OrderID}")]
    public IActionResult DeleteOrder(int key)
    {
        var item = this.context.Orders
            .Where(i => i.OrderID == key)
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

    partial void OnOrderUpdated(Models.Northwind.Order item);

    [HttpPut("{OrderID}")]
    public IActionResult PutOrder(int key, [FromBody]Models.Northwind.Order newItem)
    {
        if (newItem == null || newItem.OrderID != key)
        {
            return BadRequest();
        }

        this.OnOrderUpdated(newItem);
        this.context.Orders.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{OrderID}")]
    public IActionResult PatchOrder(int key, [FromBody]JObject patch)
    {
        var item = this.context.Orders.Where(i=>i.OrderID == key).FirstOrDefault();

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

    partial void OnOrderCreated(Models.Northwind.Order item);

    [HttpPost]
    public IActionResult Post([FromBody] Models.Northwind.Order item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnOrderCreated(item);
        this.context.Orders.Add(item);
        this.context.SaveChanges();

        return Created($"odata/Northwind/Orders/{item.OrderID}", item);
    }
  }
}
