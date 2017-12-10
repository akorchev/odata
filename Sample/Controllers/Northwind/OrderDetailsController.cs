using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MyApp.Controllers.Northwind
{
  using Models;
  using Data;
  using Models.Northwind;

  [EnableQuery]
  [ODataRoute("odata/Northwind/OrderDetails")]
  public partial class OrderDetailsController : Controller
  {
    private Data.NorthwindContext context;

    public OrderDetailsController(Data.NorthwindContext context)
    {
      this.context = context;
    }
    // GET /odata/Northwind/OrderDetails
    [HttpGet]
    public IEnumerable<Models.Northwind.OrderDetail> Get()
    {
      var items = this.context.OrderDetails.AsQueryable<Models.Northwind.OrderDetail>();

      this.OnOrderDetailsRead(ref items);

      return items;
    }

    partial void OnOrderDetailsRead(ref IQueryable<Models.Northwind.OrderDetail> items);

    [HttpGet("{OrderID}")]
    public IActionResult GetOrderDetail(int key)
    {
        var item = this.context.OrderDetails.Where(i=>i.OrderID == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnOrderDetailDeleted(Models.Northwind.OrderDetail item);

    [HttpDelete("{OrderID}")]
    public IActionResult DeleteOrderDetail(int key)
    {
        var item = this.context.OrderDetails
            .Where(i => i.OrderID == key)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnOrderDetailDeleted(item);
        this.context.OrderDetails.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnOrderDetailUpdated(Models.Northwind.OrderDetail item);

    [HttpPut("{OrderID}")]
    public IActionResult PutOrderDetail(int key, [FromBody]Models.Northwind.OrderDetail newItem)
    {
        if (newItem == null || newItem.OrderID != key)
        {
            return BadRequest();
        }

        this.OnOrderDetailUpdated(newItem);
        this.context.OrderDetails.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{OrderID}")]
    public IActionResult PatchOrderDetail(int key, [FromBody]JObject patch)
    {
        var item = this.context.OrderDetails.Where(i=>i.OrderID == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        Data.EntityPatch.Apply(item, patch);

        this.OnOrderDetailUpdated(item);
        this.context.OrderDetails.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnOrderDetailCreated(Models.Northwind.OrderDetail item);

    [HttpPost]
    public IActionResult Post([FromBody] Models.Northwind.OrderDetail item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnOrderDetailCreated(item);
        this.context.OrderDetails.Add(item);
        this.context.SaveChanges();

        return Created($"odata/Northwind/OrderDetails/{item.OrderID}", item);
    }
  }
}
