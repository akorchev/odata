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
  [ODataRoute("odata/CodewareDb/DatePeriods")]
  public partial class DatePeriodsController : Controller
  {
    private CodewareDbContext context;

    public DatePeriodsController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/DatePeriods
    [HttpGet]
    public IEnumerable<DatePeriod> Get()
    {
      var items = this.context.DatePeriods.AsQueryable<DatePeriod>();

      this.OnDatePeriodsRead(ref items);

      return items;
    }

    partial void OnDatePeriodsRead(ref IQueryable<DatePeriod> items);

    [HttpGet("{Code}")]
    public IActionResult GetDatePeriod(string key)
    {
        var item = this.context.DatePeriods.Where(i=>i.Code == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnDatePeriodDeleted(DatePeriod item);

    [HttpDelete("{Code}")]
    public IActionResult DeleteDatePeriod(string key)
    {
        var item = this.context.DatePeriods
            .Where(i => i.Code == key)
            .Include(i => i.Categories)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnDatePeriodDeleted(item);
        this.context.DatePeriods.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnDatePeriodUpdated(DatePeriod item);

    [HttpPut("{Code}")]
    public IActionResult PutDatePeriod(string key, [FromBody]DatePeriod newItem)
    {
        if (newItem == null || newItem.Code != key)
        {
            return BadRequest();
        }

        this.OnDatePeriodUpdated(newItem);
        this.context.DatePeriods.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{Code}")]
    public IActionResult PatchDatePeriod(string key, [FromBody]JObject patch)
    {
        var item = this.context.DatePeriods.Where(i=>i.Code == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        EntityPatch.Apply(item, patch);

        this.OnDatePeriodUpdated(item);
        this.context.DatePeriods.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnDatePeriodCreated(DatePeriod item);

    [HttpPost]
    public IActionResult Post([FromBody] DatePeriod item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnDatePeriodCreated(item);
        this.context.DatePeriods.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/DatePeriods/{item.Code}", item);
    }
  }
}
