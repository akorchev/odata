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
  [ODataRoute("odata/CodewareDb/KnowledgeBases")]
  public partial class KnowledgeBasesController : Controller
  {
    private CodewareDbContext context;

    public KnowledgeBasesController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/KnowledgeBases
    [HttpGet]
    public IEnumerable<KnowledgeBase> Get()
    {
      var items = this.context.KnowledgeBases.AsQueryable<KnowledgeBase>();

      this.OnKnowledgeBasesRead(ref items);

      return items;
    }

    partial void OnKnowledgeBasesRead(ref IQueryable<KnowledgeBase> items);

    [HttpGet("{KBID}")]
    public IActionResult GetKnowledgeBase(int key)
    {
        var item = this.context.KnowledgeBases.Where(i=>i.KBID == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnKnowledgeBaseDeleted(KnowledgeBase item);

    [HttpDelete("{KBID}")]
    public IActionResult DeleteKnowledgeBase(int key)
    {
        var item = this.context.KnowledgeBases
            .Where(i => i.KBID == key)
            .Include(i => i.Details)
            .Include(i => i.CustomerKnowledgeBases)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnKnowledgeBaseDeleted(item);
        this.context.KnowledgeBases.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnKnowledgeBaseUpdated(KnowledgeBase item);

    [HttpPut("{KBID}")]
    public IActionResult PutKnowledgeBase(int key, [FromBody]KnowledgeBase newItem)
    {
        if (newItem == null || newItem.KBID != key)
        {
            return BadRequest();
        }

        this.OnKnowledgeBaseUpdated(newItem);
        this.context.KnowledgeBases.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{KBID}")]
    public IActionResult PatchKnowledgeBase(int key, [FromBody]JObject patch)
    {
        var item = this.context.KnowledgeBases.Where(i=>i.KBID == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        EntityPatch.Apply(item, patch);

        this.OnKnowledgeBaseUpdated(item);
        this.context.KnowledgeBases.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnKnowledgeBaseCreated(KnowledgeBase item);

    [HttpPost]
    public IActionResult Post([FromBody] KnowledgeBase item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnKnowledgeBaseCreated(item);
        this.context.KnowledgeBases.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/KnowledgeBases/{item.KBID}", item);
    }
  }
}
