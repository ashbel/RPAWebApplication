using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RpaData.DataContext;
using RpaData.Models;

namespace RpaApis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ResourcesApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ResourcesApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ResourcesApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<tblResources>>> GettblResources()
        {
            //var identity = HttpContext.User.Identity as ClaimsIdentity;
            //var clientId = Convert.ToInt32(User.FindFirst("ClientId").Value);
            //var Id = Convert.ToInt32(clientId);

            //var memberships = await _context.tblMembershipClients.Where(c => c.tblPharmacistsId == Id).ToListAsync();

            var resources = await _context.tblResources
                .Include(t => t.tblResourceCategory)
                .Select(c => new
                {
                    ResourceName = c.ResourceName,
                    ResourceCategory = c.tblResourceCategory.catName,
                    ResourceDescription = c.ResourceDescription??"",
                    ResourceUrl = "http://rpa.co.zw/Uploads/" + c.FileName,
                    Id = c.Id,
                    ResourceDate = c.Created.ToString("dd-MM-yyyy")
                })
                .ToListAsync();
            return Ok(resources);
        }

        // GET: api/ResourcesApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<tblResources>> GettblResources(int id)
        {
            var tblResources = await _context.tblResources.FindAsync(id);

            if (tblResources == null)
            {
                return NotFound();
            }

            return tblResources;
        }

        // PUT: api/ResourcesApi/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PuttblResources(int id, tblResources tblResources)
        {
            if (id != tblResources.Id)
            {
                return BadRequest();
            }

            _context.Entry(tblResources).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tblResourcesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ResourcesApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<tblResources>> PosttblResources(tblResources tblResources)
        {
            _context.tblResources.Add(tblResources);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GettblResources", new { id = tblResources.Id }, tblResources);
        }

        // DELETE: api/ResourcesApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<tblResources>> DeletetblResources(int id)
        {
            var tblResources = await _context.tblResources.FindAsync(id);
            if (tblResources == null)
            {
                return NotFound();
            }

            _context.tblResources.Remove(tblResources);
            await _context.SaveChangesAsync();

            return tblResources;
        }

        private bool tblResourcesExists(int id)
        {
            return _context.tblResources.Any(e => e.Id == id);
        }
    }
}
