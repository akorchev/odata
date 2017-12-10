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
  [ODataRoute("odata/CodewareDb/DropdownLists")]
  public partial class DropdownListsController : Controller
  {
    private CodewareDbContext context;

    public DropdownListsController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/DropdownLists
    [HttpGet]
    public IEnumerable<DropdownList> Get()
    {
      var items = this.context.DropdownLists.AsQueryable<DropdownList>();

      this.OnDropdownListsRead(ref items);

      return items;
    }

    partial void OnDropdownListsRead(ref IQueryable<DropdownList> items);

    [HttpGet("{Code}")]
    public IActionResult GetDropdownList(string key)
    {
        var item = this.context.DropdownLists.Where(i=>i.Code == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnDropdownListDeleted(DropdownList item);

    [HttpDelete("{Code}")]
    public IActionResult DeleteDropdownList(string key)
    {
        var item = this.context.DropdownLists
            .Where(i => i.Code == key)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnDropdownListDeleted(item);
        this.context.DropdownLists.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnDropdownListUpdated(DropdownList item);

    [HttpPut("{Code}")]
    public IActionResult PutDropdownList(string key, [FromBody]DropdownList newItem)
    {
        if (newItem == null || newItem.Code != key)
        {
            return BadRequest();
        }

        this.OnDropdownListUpdated(newItem);
        this.context.DropdownLists.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{Code}")]
    public IActionResult PatchDropdownList(string key, [FromBody]JObject patch)
    {
        var item = this.context.DropdownLists.Where(i=>i.Code == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        EntityPatch.Apply(item, patch);

        this.OnDropdownListUpdated(item);
        this.context.DropdownLists.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnDropdownListCreated(DropdownList item);

    [HttpPost]
    public IActionResult Post([FromBody] DropdownList item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnDropdownListCreated(item);
        this.context.DropdownLists.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/DropdownLists/{item.Code}", item);
    }
  }
}
