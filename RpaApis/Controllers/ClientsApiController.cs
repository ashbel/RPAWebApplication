using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RpaData.DataContext;
using RpaData.Models;

namespace RpaUi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ClientsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClientsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ClientsApi
        [HttpGet]
        public async Task<ActionResult <tblPharmacists>> GettblPharmacists()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var clientId = identity.FindFirst("UserId").Value;

            return await _context.tblPharmacists.Include(c => c.ApplicationUser).Include(t => t.tblPharmacy).FirstOrDefaultAsync(c => c.ApplicationUserId == clientId);
        }

        // GET: api/ClientsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<tblPharmacists>> GettblPharmacists(int id)
        {
            var tblPharmacists = await _context.tblPharmacists.FindAsync(id);

            if (tblPharmacists == null)
            {
                return NotFound();
            }

            return tblPharmacists;
        }

        // PUT: api/ClientsApi/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PuttblPharmacists(int id, tblPharmacists tblPharmacists)
        {
            if (id != tblPharmacists.Id)
            {
                return BadRequest();
            }

            _context.Entry(tblPharmacists).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tblPharmacistsExists(id))
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

        // POST: api/ClientsApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<tblPharmacists>> PosttblPharmacists(tblPharmacists tblPharmacists)
        {
            _context.tblPharmacists.Add(tblPharmacists);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GettblPharmacists", new { id = tblPharmacists.Id }, tblPharmacists);
        }

        // DELETE: api/ClientsApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<tblPharmacists>> DeletetblPharmacists(int id)
        {
            var tblPharmacists = await _context.tblPharmacists.FindAsync(id);
            if (tblPharmacists == null)
            {
                return NotFound();
            }

            _context.tblPharmacists.Remove(tblPharmacists);
            await _context.SaveChangesAsync();

            return tblPharmacists;
        }

        private bool tblPharmacistsExists(int id)
        {
            return _context.tblPharmacists.Any(e => e.Id == id);
        }
    }
}
