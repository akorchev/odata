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
  [ODataRoute("odata/CodewareDb/KnowledgeBaseTypes")]
  public partial class KnowledgeBaseTypesController : Controller
  {
    private CodewareDbContext context;

    public KnowledgeBaseTypesController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/KnowledgeBaseTypes
    [HttpGet]
    public IEnumerable<KnowledgeBaseType> Get()
    {
      var items = this.context.KnowledgeBaseTypes.AsQueryable<KnowledgeBaseType>();

      this.OnKnowledgeBaseTypesRead(ref items);

      return items;
    }

    partial void OnKnowledgeBaseTypesRead(ref IQueryable<KnowledgeBaseType> items);

    [HttpGet("{KBTypeID}")]
    public IActionResult GetKnowledgeBaseType(int key)
    {
        var item = this.context.KnowledgeBaseTypes.Where(i=>i.KBTypeID == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnKnowledgeBaseTypeDeleted(KnowledgeBaseType item);

    [HttpDelete("{KBTypeID}")]
    public IActionResult DeleteKnowledgeBaseType(int key)
    {
        var item = this.context.KnowledgeBaseTypes
            .Where(i => i.KBTypeID == key)
            .Include(i => i.KnowledgeBases)
            .Include(i => i.KnowledegeBaseSubTypes)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnKnowledgeBaseTypeDeleted(item);
        this.context.KnowledgeBaseTypes.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnKnowledgeBaseTypeUpdated(KnowledgeBaseType item);

    [HttpPut("{KBTypeID}")]
    public IActionResult PutKnowledgeBaseType(int key, [FromBody]KnowledgeBaseType newItem)
    {
        if (newItem == null || newItem.KBTypeID != key)
        {
            return BadRequest();
        }

        this.OnKnowledgeBaseTypeUpdated(newItem);
        this.context.KnowledgeBaseTypes.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{KBTypeID}")]
    public IActionResult PatchKnowledgeBaseType(int key, [FromBody]JObject patch)
    {
        var item = this.context.KnowledgeBaseTypes.Where(i=>i.KBTypeID == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        EntityPatch.Apply(item, patch);

        this.OnKnowledgeBaseTypeUpdated(item);
        this.context.KnowledgeBaseTypes.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnKnowledgeBaseTypeCreated(KnowledgeBaseType item);

    [HttpPost]
    public IActionResult Post([FromBody] KnowledgeBaseType item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnKnowledgeBaseTypeCreated(item);
        this.context.KnowledgeBaseTypes.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/KnowledgeBaseTypes/{item.KBTypeID}", item);
    }
  }
}
