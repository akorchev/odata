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
  [ODataRoute("odata/CodewareDb/KnowledgeBaseScriptInits")]
  public partial class KnowledgeBaseScriptInitsController : Controller
  {
    private CodewareDbContext context;

    public KnowledgeBaseScriptInitsController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/KnowledgeBaseScriptInits
    [HttpGet]
    public IEnumerable<KnowledgeBaseScriptInit> Get()
    {
      var items = this.context.KnowledgeBaseScriptInits.AsQueryable<KnowledgeBaseScriptInit>();

      this.OnKnowledgeBaseScriptInitsRead(ref items);

      return items;
    }

    partial void OnKnowledgeBaseScriptInitsRead(ref IQueryable<KnowledgeBaseScriptInit> items);

    [HttpGet("{KBScriptInitID}")]
    public IActionResult GetKnowledgeBaseScriptInit(int key)
    {
        var item = this.context.KnowledgeBaseScriptInits.Where(i=>i.KBScriptInitID == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnKnowledgeBaseScriptInitDeleted(KnowledgeBaseScriptInit item);

    [HttpDelete("{KBScriptInitID}")]
    public IActionResult DeleteKnowledgeBaseScriptInit(int key)
    {
        var item = this.context.KnowledgeBaseScriptInits
            .Where(i => i.KBScriptInitID == key)
            .Include(i => i.KnowledgeBases)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnKnowledgeBaseScriptInitDeleted(item);
        this.context.KnowledgeBaseScriptInits.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnKnowledgeBaseScriptInitUpdated(KnowledgeBaseScriptInit item);

    [HttpPut("{KBScriptInitID}")]
    public IActionResult PutKnowledgeBaseScriptInit(int key, [FromBody]KnowledgeBaseScriptInit newItem)
    {
        if (newItem == null || newItem.KBScriptInitID != key)
        {
            return BadRequest();
        }

        this.OnKnowledgeBaseScriptInitUpdated(newItem);
        this.context.KnowledgeBaseScriptInits.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{KBScriptInitID}")]
    public IActionResult PatchKnowledgeBaseScriptInit(int key, [FromBody]JObject patch)
    {
        var item = this.context.KnowledgeBaseScriptInits.Where(i=>i.KBScriptInitID == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        EntityPatch.Apply(item, patch);

        this.OnKnowledgeBaseScriptInitUpdated(item);
        this.context.KnowledgeBaseScriptInits.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnKnowledgeBaseScriptInitCreated(KnowledgeBaseScriptInit item);

    [HttpPost]
    public IActionResult Post([FromBody] KnowledgeBaseScriptInit item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnKnowledgeBaseScriptInitCreated(item);
        this.context.KnowledgeBaseScriptInits.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/KnowledgeBaseScriptInits/{item.KBScriptInitID}", item);
    }
  }
}
