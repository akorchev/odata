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
  [ODataRoutePrefix("odata/Sample/OrderDetails")]
  public partial class OrderDetailsController : Controller
  {
    private Data.SampleContext context;

    public OrderDetailsController(Data.SampleContext context)
    {
      this.context = context;
    }
    // GET /odata/Sample/OrderDetails
    [HttpGet]
    public IEnumerable<Models.Sample.OrderDetail> GetOrderDetails()
    {
      var items = this.context.OrderDetails.AsQueryable<Models.Sample.OrderDetail>();

      this.OnOrderDetailsRead(ref items);

      return items;
    }

    partial void OnOrderDetailsRead(ref IQueryable<Models.Sample.OrderDetail> items);

    [HttpGet("{Id}")]
    public IActionResult GetOrderDetail(int key)
    {
        var item = this.context.OrderDetails.Where(i=>i.Id == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnOrderDetailDeleted(Models.Sample.OrderDetail item);

    [HttpDelete("{Id}")]
    public IActionResult DeleteOrderDetail(int key)
    {
        var item = this.context.OrderDetails
            .Where(i => i.Id == key)
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

    partial void OnOrderDetailUpdated(Models.Sample.OrderDetail item);

    [HttpPut("{Id}")]
    public IActionResult PutOrderDetail(int key, [FromBody]Models.Sample.OrderDetail newItem)
    {
        if (newItem == null || newItem.Id != key)
        {
            return BadRequest();
        }

        this.OnOrderDetailUpdated(newItem);
        this.context.OrderDetails.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{Id}")]
    public IActionResult PatchOrderDetail(int key, [FromBody]JObject patch)
    {
        var item = this.context.OrderDetails.Where(i=>i.Id == key).FirstOrDefault();

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

    partial void OnOrderDetailCreated(Models.Sample.OrderDetail item);

    [HttpPost]
    public IActionResult Post([FromBody] Models.Sample.OrderDetail item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnOrderDetailCreated(item);
        this.context.OrderDetails.Add(item);
        this.context.SaveChanges();

        return Created($"odata/Sample/OrderDetails/{item.Id}", item);
    }
  }
}
