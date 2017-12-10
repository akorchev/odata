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
  [ODataRoute("odata/CodewareDb/ProjectCalculations")]
  public partial class ProjectCalculationsController : Controller
  {
    private CodewareDbContext context;

    public ProjectCalculationsController(CodewareDbContext context)
    {
      this.context = context;
    }
    // GET /odata/CodewareDb/ProjectCalculations
    [HttpGet]
    public IEnumerable<ProjectCalculation> Get()
    {
      var items = this.context.ProjectCalculations.AsQueryable<ProjectCalculation>();

      this.OnProjectCalculationsRead(ref items);

      return items;
    }

    partial void OnProjectCalculationsRead(ref IQueryable<ProjectCalculation> items);

    [HttpGet("{ProjectCalcID}")]
    public IActionResult GetProjectCalculation(int key)
    {
        var item = this.context.ProjectCalculations.Where(i=>i.ProjectCalcID == key).SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }
    partial void OnProjectCalculationDeleted(ProjectCalculation item);

    [HttpDelete("{ProjectCalcID}")]
    public IActionResult DeleteProjectCalculation(int key)
    {
        var item = this.context.ProjectCalculations
            .Where(i => i.ProjectCalcID == key)
            .Include(i => i.Projects)
            .SingleOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        this.OnProjectCalculationDeleted(item);
        this.context.ProjectCalculations.Remove(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnProjectCalculationUpdated(ProjectCalculation item);

    [HttpPut("{ProjectCalcID}")]
    public IActionResult PutProjectCalculation(int key, [FromBody]ProjectCalculation newItem)
    {
        if (newItem == null || newItem.ProjectCalcID != key)
        {
            return BadRequest();
        }

        this.OnProjectCalculationUpdated(newItem);
        this.context.ProjectCalculations.Update(newItem);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    [HttpPatch("{ProjectCalcID}")]
    public IActionResult PatchProjectCalculation(int key, [FromBody]JObject patch)
    {
        var item = this.context.ProjectCalculations.Where(i=>i.ProjectCalcID == key).FirstOrDefault();

        if (item == null)
        {
            return BadRequest();
        }

        EntityPatch.Apply(item, patch);

        this.OnProjectCalculationUpdated(item);
        this.context.ProjectCalculations.Update(item);
        this.context.SaveChanges();

        return new NoContentResult();
    }

    partial void OnProjectCalculationCreated(ProjectCalculation item);

    [HttpPost]
    public IActionResult Post([FromBody] ProjectCalculation item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        this.OnProjectCalculationCreated(item);
        this.context.ProjectCalculations.Add(item);
        this.context.SaveChanges();

        return Created($"odata/CodewareDb/ProjectCalculations/{item.ProjectCalcID}", item);
    }
  }
}
