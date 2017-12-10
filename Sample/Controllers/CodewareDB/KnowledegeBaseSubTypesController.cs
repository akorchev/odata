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
  [ODataRoute("odata/CodewareDb/KnowledegeBaseSubTypes")]
  public partial class KnowledegeBaseSubTypesController : Controller
  {
    private CodewareDbContext context;

    public KnowledegeBaseSubTypesController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/KnowledegeBaseSubTypes
    [HttpGet]
    public IEnumerable<KnowledegeBaseSubType> Get()
    {
      var items = this.context.KnowledegeBaseSubTypes.AsQueryable<KnowledegeBaseSubType>();

      this.OnKnowledegeBaseSubTypesRead(ref items);

      return items;
    }

    partial void OnKnowledegeBaseSubTypesRead(ref IQueryable<KnowledegeBaseSubType> items);

    [HttpGet("{KBSubTypeID}")]
    public IActionResult GetKnowledegeBaseSubType(int key)
    {
        var item = this.context.KnowledegeBaseSubTypes.Where(i=>i.KBSubTypeID == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnKnowledegeBaseSubTypeDeleted(KnowledegeBaseSubType item);

    [HttpDelete("{KBSubTypeID}")]
    public IActionResult DeleteKnowledegeBaseSubType(int key)
    {
        var item = this.context.KnowledegeBaseSubTypes
            .Where(i => i.KBSubTypeID == key)
            .Include(i => i.KnowledgeBases)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnKnowledegeBaseSubTypeDeleted(item);
        this.context.KnowledegeBaseSubTypes.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnKnowledegeBaseSubTypeUpdated(KnowledegeBaseSubType item);

    [HttpPut("{KBSubTypeID}")]
    public IActionResult PutKnowledegeBaseSubType(int key, [FromBody]KnowledegeBaseSubType newItem)
    {
        if (newItem == null || newItem.KBSubTypeID != key)
        {
            return BadRequest();
        }

        this.OnKnowledegeBaseSubTypeUpdated(newItem);
        this.context.KnowledegeBaseSubTypes.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{KBSubTypeID}")]
    public IActionResult PatchKnowledegeBaseSubType(int key, [FromBody]JObject patch)
    {
        var item = this.context.KnowledegeBaseSubTypes.Where(i=>i.KBSubTypeID == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        EntityPatch.Apply(item, patch);

        this.OnKnowledegeBaseSubTypeUpdated(item);
        this.context.KnowledegeBaseSubTypes.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnKnowledegeBaseSubTypeCreated(KnowledegeBaseSubType item);

    [HttpPost]
    public IActionResult Post([FromBody] KnowledegeBaseSubType item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnKnowledegeBaseSubTypeCreated(item);
        this.context.KnowledegeBaseSubTypes.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/KnowledegeBaseSubTypes/{item.KBSubTypeID}", item);
    }
  }
}
