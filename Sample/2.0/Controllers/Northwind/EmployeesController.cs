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
  [ODataRoutePrefix("odata/Northwind/Employees")]
  public partial class EmployeesController : Controller
  {
    private Data.NorthwindContext context;

    public EmployeesController(Data.NorthwindContext context)
    {
      this.context = context;
    }
    // GET /odata/Northwind/Employees
    [HttpGet]
    public IEnumerable<Models.Northwind.Employee> Get()
    {
      var items = this.context.Employees.AsQueryable<Models.Northwind.Employee>();

      this.OnEmployeesRead(ref items);

      return items;
    }

    partial void OnEmployeesRead(ref IQueryable<Models.Northwind.Employee> items);

    [HttpGet("{EmployeeID}")]
    public SingleResult<Models.Northwind.Employee> GetEmployee(int key)
    {
            var items = this.context.Employees.Where(i => i.EmployeeID == key);

            return SingleResult.Create(items);
        }
    partial void OnEmployeeDeleted(Models.Northwind.Employee item);

    [HttpDelete("{EmployeeID}")]
    public IActionResult DeleteEmployee(int key)
    {
        var item = this.context.Employees
            .Where(i => i.EmployeeID == key)
            .Include(i => i.Orders)
            .Include(i => i.EmployeeTerritories)
            .Include(i => i.Employees)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnEmployeeDeleted(item);
        this.context.Employees.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnEmployeeUpdated(Models.Northwind.Employee item);

    [HttpPut("{EmployeeID}")]
    public IActionResult PutEmployee(int key, [FromBody]Models.Northwind.Employee newItem)
    {
        if (newItem == null || newItem.EmployeeID != key)
        {
            return BadRequest();
        }

        this.OnEmployeeUpdated(newItem);
        this.context.Employees.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{EmployeeID}")]
    public IActionResult PatchEmployee(int key, [FromBody]JObject patch)
    {
        var item = this.context.Employees.Where(i=>i.EmployeeID == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        Data.EntityPatch.Apply(item, patch);

        this.OnEmployeeUpdated(item);
        this.context.Employees.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnEmployeeCreated(Models.Northwind.Employee item);

    [HttpPost]
    public IActionResult Post([FromBody] Models.Northwind.Employee item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnEmployeeCreated(item);
        this.context.Employees.Add(item);
        this.context.SaveChanges();

        return Created($"odata/Northwind/Employees/{item.EmployeeID}", item);
    }
  }
}
