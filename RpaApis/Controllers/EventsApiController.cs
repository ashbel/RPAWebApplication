using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RpaData.DataContext;
using RpaData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RpaApis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EventsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EventsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/tblEvents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<tblEventsHistory>>> GettblEvents()
        {
            
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var clientId = Convert.ToInt32(User.FindFirst("ClientId").Value);
            var events = await _context.tblEventsHistory.Include(t=>t.Event).Where(c => c.tblPharmacistsId == clientId).ToListAsync();
            return events;
        }

        // GET: api/tblEvents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<tblEventsHistory>> GettblEvents(int id)
        {
            var tblEvents = await _context.tblEventsHistory.Include(t=>t.Event).FirstOrDefaultAsync(c=>c.Id == id);

            if (tblEvents == null)
            {
                return NotFound();
            }

            return tblEvents;
        }

        // PUT: api/tblEvents/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PuttblEvents(int id, tblEventsHistory tblEvents)
        {
            if (id != tblEvents.Id)
            {
                return BadRequest();
            }

            _context.Entry(tblEvents).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tblEventsExists(id))
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

        // POST: api/tblEvents
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<tblEvents>> PosttblEvents(tblEvents tblEvents)
        {
            _context.tblEvents.Add(tblEvents);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GettblEvents", new { id = tblEvents.Id }, tblEvents);
        }

        // DELETE: api/tblEvents/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<tblEvents>> DeletetblEvents(int id)
        {
            var tblEvents = await _context.tblEvents.FindAsync(id);
            if (tblEvents == null)
            {
                return NotFound();
            }

            _context.tblEvents.Remove(tblEvents);
            await _context.SaveChangesAsync();

            return tblEvents;
        }

        private bool tblEventsExists(int id)
        {
            return _context.tblEvents.Any(e => e.Id == id);
        }
    }
}
