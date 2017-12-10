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
  [ODataRoutePrefix("odata/Northwind/Territories")]
  public partial class TerritoriesController : Controller
  {
    private Data.NorthwindContext context;

    public TerritoriesController(Data.NorthwindContext context)
    {
      this.context = context;
    }
    // GET /odata/Northwind/Territories
    [HttpGet]
    public IEnumerable<Models.Northwind.Territory> Get()
    {
      var items = this.context.Territories.AsQueryable<Models.Northwind.Territory>();

      this.OnTerritoriesRead(ref items);

      return items;
    }

    partial void OnTerritoriesRead(ref IQueryable<Models.Northwind.Territory> items);

    [HttpGet("{TerritoryID}")]
    public SingleResult<Models.Northwind.Territory> GetTerritory(string key)
    {
        var items = this.context.Territories.Where(i=>i.TerritoryID == key);

            return SingleResult.Create(items);
        }
    partial void OnTerritoryDeleted(Models.Northwind.Territory item);

    [HttpDelete("{TerritoryID}")]
    public IActionResult DeleteTerritory(string key)
    {
        var item = this.context.Territories
            .Where(i => i.TerritoryID == key)
            .Include(i => i.EmployeeTerritories)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnTerritoryDeleted(item);
        this.context.Territories.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnTerritoryUpdated(Models.Northwind.Territory item);

    [HttpPut("{TerritoryID}")]
    public IActionResult PutTerritory(string key, [FromBody]Models.Northwind.Territory newItem)
    {
        if (newItem == null || newItem.TerritoryID != key)
        {
            return BadRequest();
        }

        this.OnTerritoryUpdated(newItem);
        this.context.Territories.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{TerritoryID}")]
    public IActionResult PatchTerritory(string key, [FromBody]JObject patch)
    {
        var item = this.context.Territories.Where(i=>i.TerritoryID == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        Data.EntityPatch.Apply(item, patch);

        this.OnTerritoryUpdated(item);
        this.context.Territories.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnTerritoryCreated(Models.Northwind.Territory item);

    [HttpPost]
    public IActionResult Post([FromBody] Models.Northwind.Territory item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnTerritoryCreated(item);
        this.context.Territories.Add(item);
        this.context.SaveChanges();

        return Created($"odata/Northwind/Territories/{item.TerritoryID}", item);
    }
  }
}
