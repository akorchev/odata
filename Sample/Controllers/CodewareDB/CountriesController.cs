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
  [ODataRoute("odata/CodewareDb/Countries")]
  public partial class CountriesController : Controller
  {
    private CodewareDbContext context;

    public CountriesController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/Countries
    [HttpGet]
    public IEnumerable<Country> Get()
    {
      var items = this.context.Countries.AsQueryable<Country>();

      this.OnCountriesRead(ref items);

      return items;
    }

    partial void OnCountriesRead(ref IQueryable<Country> items);

    [HttpGet("{CountryCode}")]
    public IActionResult GetCountry(string key)
    {
        var item = this.context.Countries.Where(i=>i.CountryCode == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnCountryDeleted(Country item);

    [HttpDelete("{CountryCode}")]
    public IActionResult DeleteCountry(string key)
    {
        var item = this.context.Countries
            .Where(i => i.CountryCode == key)
            .Include(i => i.Customers)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnCountryDeleted(item);
        this.context.Countries.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnCountryUpdated(Country item);

    [HttpPut("{CountryCode}")]
    public IActionResult PutCountry(string key, [FromBody]Country newItem)
    {
        if (newItem == null || newItem.CountryCode != key)
        {
            return BadRequest();
        }

        this.OnCountryUpdated(newItem);
        this.context.Countries.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{CountryCode}")]
    public IActionResult PatchCountry(string key, [FromBody]JObject patch)
    {
        var item = this.context.Countries.Where(i=>i.CountryCode == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        EntityPatch.Apply(item, patch);

        this.OnCountryUpdated(item);
        this.context.Countries.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnCountryCreated(Country item);

    [HttpPost]
    public IActionResult Post([FromBody] Country item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnCountryCreated(item);
        this.context.Countries.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/Countries/{item.CountryCode}", item);
    }
  }
}
