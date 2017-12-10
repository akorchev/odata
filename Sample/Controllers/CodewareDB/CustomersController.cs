using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using CodewareDb.Models;
using CodewareDb.Data;
using CodewareDb.Models.CodewareDb;

namespace CodewareDb.Controllers.CodewareDb
{
  [EnableQuery]
  [ODataRoute("odata/CodewareDb/Customers")]
  public partial class CustomersController : Controller
  {
    private CodewareDbContext context;

    public CustomersController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/Customers
    [HttpGet]
    public IEnumerable<Customer> Get()
    {
      var items = this.context.Customers.AsQueryable<Customer>();

      this.OnCustomersRead(ref items);

      return items;
    }

    partial void OnCustomersRead(ref IQueryable<Customer> items);

    [HttpGet("{CustomerCode}")]
    public IActionResult GetCustomer(string key)
    {
        var item = this.context.Customers.Where(i=>i.CustomerCode == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnCustomerDeleted(Customer item);

    [HttpDelete("{CustomerCode}")]
    public IActionResult DeleteCustomer(string key)
    {
        var item = this.context.Customers
            .Where(i => i.CustomerCode == key)
            .Include(i => i.CustomerKnowledgeBases)
            .Include(i => i.Projects)
            .Include(i => i.SupportIssues)
            .Include(i => i.Invoices)
            .Include(i => i.SupportCategories)
            .Include(i => i.Users)
            .Include(i => i.CustomerContacts)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnCustomerDeleted(item);
        this.context.Customers.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnCustomerUpdated(Customer item);

    [HttpPut("{CustomerCode}")]
    public IActionResult PutCustomer(string key, [FromBody]Customer newItem)
    {
        if (newItem == null || newItem.CustomerCode != key)
        {
            return BadRequest();
        }

        this.OnCustomerUpdated(newItem);
        this.context.Customers.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{CustomerCode}")]
    public IActionResult PatchCustomer(string key, [FromBody]JObject patch)
    {
        var item = this.context.Customers.Where(i=>i.CustomerCode == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        EntityPatch.Apply(item, patch);

        this.OnCustomerUpdated(item);
        this.context.Customers.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnCustomerCreated(Customer item);

    [HttpPost]
    public IActionResult Post([FromBody] Customer item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnCustomerCreated(item);
        this.context.Customers.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/Customers/{item.CustomerCode}", item);
    }
  }
}
