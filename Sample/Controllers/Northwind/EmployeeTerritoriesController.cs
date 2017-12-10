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
  [ODataRoute("odata/Northwind/EmployeeTerritories")]
  public partial class EmployeeTerritoriesController : Controller
  {
    private Data.NorthwindContext context;

    public EmployeeTerritoriesController(Data.NorthwindContext context)
    {
      this.context = context;
    }
    // GET /odata/Northwind/EmployeeTerritories
    [HttpGet]
    public IEnumerable<Models.Northwind.EmployeeTerritory> Get()
    {
      var items = this.context.EmployeeTerritories.AsQueryable<Models.Northwind.EmployeeTerritory>();

      this.OnEmployeeTerritoriesRead(ref items);

      return items;
    }

    partial void OnEmployeeTerritoriesRead(ref IQueryable<Models.Northwind.EmployeeTerritory> items);

    [HttpGet("{EmployeeID}")]
    public IActionResult GetEmployeeTerritory(int key)
    {
        var item = this.context.EmployeeTerritories.Where(i=>i.EmployeeID == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnEmployeeTerritoryDeleted(Models.Northwind.EmployeeTerritory item);

    [HttpDelete("{EmployeeID}")]
    public IActionResult DeleteEmployeeTerritory(int key)
    {
        var item = this.context.EmployeeTerritories
            .Where(i => i.EmployeeID == key)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnEmployeeTerritoryDeleted(item);
        this.context.EmployeeTerritories.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnEmployeeTerritoryUpdated(Models.Northwind.EmployeeTerritory item);

    [HttpPut("{EmployeeID}")]
    public IActionResult PutEmployeeTerritory(int key, [FromBody]Models.Northwind.EmployeeTerritory newItem)
    {
        if (newItem == null || newItem.EmployeeID != key)
        {
            return BadRequest();
        }

        this.OnEmployeeTerritoryUpdated(newItem);
        this.context.EmployeeTerritories.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{EmployeeID}")]
    public IActionResult PatchEmployeeTerritory(int key, [FromBody]JObject patch)
    {
        var item = this.context.EmployeeTerritories.Where(i=>i.EmployeeID == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        Data.EntityPatch.Apply(item, patch);

        this.OnEmployeeTerritoryUpdated(item);
        this.context.EmployeeTerritories.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnEmployeeTerritoryCreated(Models.Northwind.EmployeeTerritory item);

    [HttpPost]
    public IActionResult Post([FromBody] Models.Northwind.EmployeeTerritory item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnEmployeeTerritoryCreated(item);
        this.context.EmployeeTerritories.Add(item);
        this.context.SaveChanges();

        return Created($"odata/Northwind/EmployeeTerritories/{item.EmployeeID}", item);
    }
  }
}
