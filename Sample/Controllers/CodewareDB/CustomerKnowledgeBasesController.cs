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
  [ODataRoute("odata/CodewareDb/CustomerKnowledgeBases")]
  public partial class CustomerKnowledgeBasesController : Controller
  {
    private CodewareDbContext context;

    public CustomerKnowledgeBasesController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/CustomerKnowledgeBases
    [HttpGet]
    public IEnumerable<CustomerKnowledgeBase> Get()
    {
      var items = this.context.CustomerKnowledgeBases.AsQueryable<CustomerKnowledgeBase>();

      this.OnCustomerKnowledgeBasesRead(ref items);

      return items;
    }

    partial void OnCustomerKnowledgeBasesRead(ref IQueryable<CustomerKnowledgeBase> items);

    [HttpGet("{CustomerKBID}")]
    public IActionResult GetCustomerKnowledgeBase(int key)
    {
        var item = this.context.CustomerKnowledgeBases.Where(i=>i.CustomerKBID == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnCustomerKnowledgeBaseDeleted(CustomerKnowledgeBase item);

    [HttpDelete("{CustomerKBID}")]
    public IActionResult DeleteCustomerKnowledgeBase(int key)
    {
        var item = this.context.CustomerKnowledgeBases
            .Where(i => i.CustomerKBID == key)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnCustomerKnowledgeBaseDeleted(item);
        this.context.CustomerKnowledgeBases.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnCustomerKnowledgeBaseUpdated(CustomerKnowledgeBase item);

    [HttpPut("{CustomerKBID}")]
    public IActionResult PutCustomerKnowledgeBase(int key, [FromBody]CustomerKnowledgeBase newItem)
    {
        if (newItem == null || newItem.CustomerKBID != key)
        {
            return BadRequest();
        }

        this.OnCustomerKnowledgeBaseUpdated(newItem);
        this.context.CustomerKnowledgeBases.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{CustomerKBID}")]
    public IActionResult PatchCustomerKnowledgeBase(int key, [FromBody]JObject patch)
    {
        var item = this.context.CustomerKnowledgeBases.Where(i=>i.CustomerKBID == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        EntityPatch.Apply(item, patch);

        this.OnCustomerKnowledgeBaseUpdated(item);
        this.context.CustomerKnowledgeBases.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnCustomerKnowledgeBaseCreated(CustomerKnowledgeBase item);

    [HttpPost]
    public IActionResult Post([FromBody] CustomerKnowledgeBase item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnCustomerKnowledgeBaseCreated(item);
        this.context.CustomerKnowledgeBases.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/CustomerKnowledgeBases/{item.CustomerKBID}", item);
    }
  }
}
