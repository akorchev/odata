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
  [ODataRoute("odata/CodewareDb/InvoiceDetails")]
  public partial class InvoiceDetailsController : Controller
  {
    private CodewareDbContext context;

    public InvoiceDetailsController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/InvoiceDetails
    [HttpGet]
    public IEnumerable<InvoiceDetail> Get()
    {
      var items = this.context.InvoiceDetails.AsQueryable<InvoiceDetail>();

      this.OnInvoiceDetailsRead(ref items);

      return items;
    }

    partial void OnInvoiceDetailsRead(ref IQueryable<InvoiceDetail> items);

    [HttpGet("{InvoiceDetailID}")]
    public IActionResult GetInvoiceDetail(int key)
    {
        var item = this.context.InvoiceDetails.Where(i=>i.InvoiceDetailID == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnInvoiceDetailDeleted(InvoiceDetail item);

    [HttpDelete("{InvoiceDetailID}")]
    public IActionResult DeleteInvoiceDetail(int key)
    {
        var item = this.context.InvoiceDetails
            .Where(i => i.InvoiceDetailID == key)
            .Include(i => i.Details)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnInvoiceDetailDeleted(item);
        this.context.InvoiceDetails.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnInvoiceDetailUpdated(InvoiceDetail item);

    [HttpPut("{InvoiceDetailID}")]
    public IActionResult PutInvoiceDetail(int key, [FromBody]InvoiceDetail newItem)
    {
        if (newItem == null || newItem.InvoiceDetailID != key)
        {
            return BadRequest();
        }

        this.OnInvoiceDetailUpdated(newItem);
        this.context.InvoiceDetails.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{InvoiceDetailID}")]
    public IActionResult PatchInvoiceDetail(int key, [FromBody]JObject patch)
    {
        var item = this.context.InvoiceDetails.Where(i=>i.InvoiceDetailID == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        EntityPatch.Apply(item, patch);

        this.OnInvoiceDetailUpdated(item);
        this.context.InvoiceDetails.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnInvoiceDetailCreated(InvoiceDetail item);

    [HttpPost]
    public IActionResult Post([FromBody] InvoiceDetail item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnInvoiceDetailCreated(item);
        this.context.InvoiceDetails.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/InvoiceDetails/{item.InvoiceDetailID}", item);
    }
  }
}
