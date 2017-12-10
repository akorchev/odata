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
  [ODataRoute("odata/CodewareDb/Companies")]
  public partial class CompaniesController : Controller
  {
    private CodewareDbContext context;

    public CompaniesController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/Companies
    [HttpGet]
    public IEnumerable<Company> Get()
    {
      var items = this.context.Companies.AsQueryable<Company>();

      this.OnCompaniesRead(ref items);

      return items;
    }

    partial void OnCompaniesRead(ref IQueryable<Company> items);

    [HttpGet("{CompanyCode}")]
    public IActionResult GetCompany(string key)
    {
        var item = this.context.Companies.Where(i=>i.CompanyCode == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnCompanyDeleted(Company item);

    [HttpDelete("{CompanyCode}")]
    public IActionResult DeleteCompany(string key)
    {
        var item = this.context.Companies
            .Where(i => i.CompanyCode == key)
            .Include(i => i.Details)
            .Include(i => i.Invoices)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnCompanyDeleted(item);
        this.context.Companies.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnCompanyUpdated(Company item);

    [HttpPut("{CompanyCode}")]
    public IActionResult PutCompany(string key, [FromBody]Company newItem)
    {
        if (newItem == null || newItem.CompanyCode != key)
        {
            return BadRequest();
        }

        this.OnCompanyUpdated(newItem);
        this.context.Companies.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{CompanyCode}")]
    public IActionResult PatchCompany(string key, [FromBody]JObject patch)
    {
        var item = this.context.Companies.Where(i=>i.CompanyCode == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        EntityPatch.Apply(item, patch);

        this.OnCompanyUpdated(item);
        this.context.Companies.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnCompanyCreated(Company item);

    [HttpPost]
    public IActionResult Post([FromBody] Company item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnCompanyCreated(item);
        this.context.Companies.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/Companies/{item.CompanyCode}", item);
    }
  }
}
