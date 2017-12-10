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
  [ODataRoute("odata/CodewareDb/VatCodes")]
  public partial class VatCodesController : Controller
  {
    private CodewareDbContext context;

    public VatCodesController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/VatCodes
    [HttpGet]
    public IEnumerable<VatCode> Get()
    {
      var items = this.context.VatCodes.AsQueryable<VatCode>();

      this.OnVatCodesRead(ref items);

      return items;
    }

    partial void OnVatCodesRead(ref IQueryable<VatCode> items);

    [HttpGet("{VatCode1}")]
    public IActionResult GetVatCode(string key)
    {
        var item = this.context.VatCodes.Where(i=>i.VatCode1 == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnVatCodeDeleted(VatCode item);

    [HttpDelete("{VatCode1}")]
    public IActionResult DeleteVatCode(string key)
    {
        var item = this.context.VatCodes
            .Where(i => i.VatCode1 == key)
            .Include(i => i.Details)
            .Include(i => i.Customers)
            .Include(i => i.InvoiceDetails)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnVatCodeDeleted(item);
        this.context.VatCodes.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnVatCodeUpdated(VatCode item);

    [HttpPut("{VatCode1}")]
    public IActionResult PutVatCode(string key, [FromBody]VatCode newItem)
    {
        if (newItem == null || newItem.VatCode1 != key)
        {
            return BadRequest();
        }

        this.OnVatCodeUpdated(newItem);
        this.context.VatCodes.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{VatCode1}")]
    public IActionResult PatchVatCode(string key, [FromBody]JObject patch)
    {
        var item = this.context.VatCodes.Where(i=>i.VatCode1 == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        EntityPatch.Apply(item, patch);

        this.OnVatCodeUpdated(item);
        this.context.VatCodes.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnVatCodeCreated(VatCode item);

    [HttpPost]
    public IActionResult Post([FromBody] VatCode item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnVatCodeCreated(item);
        this.context.VatCodes.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/VatCodes/{item.VatCode1}", item);
    }
  }
}
