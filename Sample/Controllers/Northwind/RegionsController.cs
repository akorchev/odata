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
  [ODataRoute("odata/Northwind/Regions")]
  public partial class RegionsController : Controller
  {
    private Data.NorthwindContext context;

    public RegionsController(Data.NorthwindContext context)
    {
      this.context = context;
    }
    // GET /odata/Northwind/Regions
    [HttpGet]
    public IEnumerable<Models.Northwind.Region> Get()
    {
      var items = this.context.Regions.AsQueryable<Models.Northwind.Region>();

      this.OnRegionsRead(ref items);

      return items;
    }

    partial void OnRegionsRead(ref IQueryable<Models.Northwind.Region> items);

    [HttpGet("{RegionID}")]
    public IActionResult GetRegion(int key)
    {
        var item = this.context.Regions.Where(i=>i.RegionID == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnRegionDeleted(Models.Northwind.Region item);

    [HttpDelete("{RegionID}")]
    public IActionResult DeleteRegion(int key)
    {
        var item = this.context.Regions
            .Where(i => i.RegionID == key)
            .Include(i => i.Territories)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnRegionDeleted(item);
        this.context.Regions.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnRegionUpdated(Models.Northwind.Region item);

    [HttpPut("{RegionID}")]
    public IActionResult PutRegion(int key, [FromBody]Models.Northwind.Region newItem)
    {
        if (newItem == null || newItem.RegionID != key)
        {
            return BadRequest();
        }

        this.OnRegionUpdated(newItem);
        this.context.Regions.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{RegionID}")]
    public IActionResult PatchRegion(int key, [FromBody]JObject patch)
    {
        var item = this.context.Regions.Where(i=>i.RegionID == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        Data.EntityPatch.Apply(item, patch);

        this.OnRegionUpdated(item);
        this.context.Regions.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnRegionCreated(Models.Northwind.Region item);

    [HttpPost]
    public IActionResult Post([FromBody] Models.Northwind.Region item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnRegionCreated(item);
        this.context.Regions.Add(item);
        this.context.SaveChanges();

        return Created($"odata/Northwind/Regions/{item.RegionID}", item);
    }
  }
}
