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
  [ODataRoute("odata/CodewareDb/DateSelectors")]
  public partial class DateSelectorsController : Controller
  {
    private CodewareDbContext context;

    public DateSelectorsController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/DateSelectors
    [HttpGet]
    public IEnumerable<DateSelector> Get()
    {
      var items = this.context.DateSelectors.AsQueryable<DateSelector>();

      this.OnDateSelectorsRead(ref items);

      return items;
    }

    partial void OnDateSelectorsRead(ref IQueryable<DateSelector> items);

    [HttpGet("{Code}")]
    public IActionResult GetDateSelector(string key)
    {
        var item = this.context.DateSelectors.Where(i=>i.Code == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnDateSelectorDeleted(DateSelector item);

    [HttpDelete("{Code}")]
    public IActionResult DeleteDateSelector(string key)
    {
        var item = this.context.DateSelectors
            .Where(i => i.Code == key)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnDateSelectorDeleted(item);
        this.context.DateSelectors.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnDateSelectorUpdated(DateSelector item);

    [HttpPut("{Code}")]
    public IActionResult PutDateSelector(string key, [FromBody]DateSelector newItem)
    {
        if (newItem == null || newItem.Code != key)
        {
            return BadRequest();
        }

        this.OnDateSelectorUpdated(newItem);
        this.context.DateSelectors.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{Code}")]
    public IActionResult PatchDateSelector(string key, [FromBody]JObject patch)
    {
        var item = this.context.DateSelectors.Where(i=>i.Code == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        EntityPatch.Apply(item, patch);

        this.OnDateSelectorUpdated(item);
        this.context.DateSelectors.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnDateSelectorCreated(DateSelector item);

    [HttpPost]
    public IActionResult Post([FromBody] DateSelector item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnDateSelectorCreated(item);
        this.context.DateSelectors.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/DateSelectors/{item.Code}", item);
    }
  }
}
