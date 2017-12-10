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
  [ODataRoute("odata/CodewareDb/KnowledgeBaseScriptTypes")]
  public partial class KnowledgeBaseScriptTypesController : Controller
  {
    private CodewareDbContext context;

    public KnowledgeBaseScriptTypesController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/KnowledgeBaseScriptTypes
    [HttpGet]
    public IEnumerable<KnowledgeBaseScriptType> Get()
    {
      var items = this.context.KnowledgeBaseScriptTypes.AsQueryable<KnowledgeBaseScriptType>();

      this.OnKnowledgeBaseScriptTypesRead(ref items);

      return items;
    }

    partial void OnKnowledgeBaseScriptTypesRead(ref IQueryable<KnowledgeBaseScriptType> items);

    [HttpGet("{KBScriptType}")]
    public IActionResult GetKnowledgeBaseScriptType(string key)
    {
        var item = this.context.KnowledgeBaseScriptTypes.Where(i=>i.KBScriptType == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnKnowledgeBaseScriptTypeDeleted(KnowledgeBaseScriptType item);

    [HttpDelete("{KBScriptType}")]
    public IActionResult DeleteKnowledgeBaseScriptType(string key)
    {
        var item = this.context.KnowledgeBaseScriptTypes
            .Where(i => i.KBScriptType == key)
            .Include(i => i.KnowledgeBases)
            .Include(i => i.KnowledgeBaseScriptInits)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnKnowledgeBaseScriptTypeDeleted(item);
        this.context.KnowledgeBaseScriptTypes.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnKnowledgeBaseScriptTypeUpdated(KnowledgeBaseScriptType item);

    [HttpPut("{KBScriptType}")]
    public IActionResult PutKnowledgeBaseScriptType(string key, [FromBody]KnowledgeBaseScriptType newItem)
    {
        if (newItem == null || newItem.KBScriptType != key)
        {
            return BadRequest();
        }

        this.OnKnowledgeBaseScriptTypeUpdated(newItem);
        this.context.KnowledgeBaseScriptTypes.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{KBScriptType}")]
    public IActionResult PatchKnowledgeBaseScriptType(string key, [FromBody]JObject patch)
    {
        var item = this.context.KnowledgeBaseScriptTypes.Where(i=>i.KBScriptType == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        EntityPatch.Apply(item, patch);

        this.OnKnowledgeBaseScriptTypeUpdated(item);
        this.context.KnowledgeBaseScriptTypes.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnKnowledgeBaseScriptTypeCreated(KnowledgeBaseScriptType item);

    [HttpPost]
    public IActionResult Post([FromBody] KnowledgeBaseScriptType item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnKnowledgeBaseScriptTypeCreated(item);
        this.context.KnowledgeBaseScriptTypes.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/KnowledgeBaseScriptTypes/{item.KBScriptType}", item);
    }
  }
}
