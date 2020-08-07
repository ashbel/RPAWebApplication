using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RpaData.DataContext;
using RpaData.Models;

namespace RpaUi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class tblEventsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public tblEventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/tblEvents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<tblEvents>>> GettblEvents()
        {
            return await _context.tblEvents.ToListAsync();
        }

        // GET: api/tblEvents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<tblEvents>> GettblEvents(int id)
        {
            var tblEvents = await _context.tblEvents.FindAsync(id);

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
        public async Task<IActionResult> PuttblEvents(int id, tblEvents tblEvents)
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
