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
  [ODataRoute("odata/CodewareDb/Invoices")]
  public partial class InvoicesController : Controller
  {
    private CodewareDbContext context;

    public InvoicesController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/Invoices
    [HttpGet]
    public IEnumerable<Invoice> Get()
    {
      var items = this.context.Invoices.AsQueryable<Invoice>();

      this.OnInvoicesRead(ref items);

      return items;
    }

    partial void OnInvoicesRead(ref IQueryable<Invoice> items);

    [HttpGet("{InvoiceID}")]
    public IActionResult GetInvoice(int key)
    {
        var item = this.context.Invoices.Where(i=>i.InvoiceID == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnInvoiceDeleted(Invoice item);

    [HttpDelete("{InvoiceID}")]
    public IActionResult DeleteInvoice(int key)
    {
        var item = this.context.Invoices
            .Where(i => i.InvoiceID == key)
            .Include(i => i.Details)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnInvoiceDeleted(item);
        this.context.Invoices.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnInvoiceUpdated(Invoice item);

    [HttpPut("{InvoiceID}")]
    public IActionResult PutInvoice(int key, [FromBody]Invoice newItem)
    {
        if (newItem == null || newItem.InvoiceID != key)
        {
            return BadRequest();
        }

        this.OnInvoiceUpdated(newItem);
        this.context.Invoices.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{InvoiceID}")]
    public IActionResult PatchInvoice(int key, [FromBody]JObject patch)
    {
        var item = this.context.Invoices.Where(i=>i.InvoiceID == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        EntityPatch.Apply(item, patch);

        this.OnInvoiceUpdated(item);
        this.context.Invoices.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnInvoiceCreated(Invoice item);

    [HttpPost]
    public IActionResult Post([FromBody] Invoice item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnInvoiceCreated(item);
        this.context.Invoices.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/Invoices/{item.InvoiceID}", item);
    }
  }
}
