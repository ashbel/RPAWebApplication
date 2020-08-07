using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RpaData.DataContext;
using RpaData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpaApis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PaymentsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PaymentsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/tblPayments

        [HttpGet]
        public async Task<ActionResult<IEnumerable<tblPayments>>> GettblPayments()
        {
            return await _context.tblPayments.ToListAsync();
        }

        // GET: api/tblPayments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<tblPayments>> GettblPayments(int id)
        {
            var tblPayments = await _context.tblPayments.FindAsync(id);

            if (tblPayments == null)
            {
                return NotFound();
            }

            return tblPayments;
        }

        // PUT: api/tblPayments/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PuttblPayments(int id, tblPayments tblPayments)
        {
            if (id != tblPayments.Id)
            {
                return BadRequest();
            }

            _context.Entry(tblPayments).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tblPaymentsExists(id))
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

        // POST: api/tblPayments
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<tblPayments>> PosttblPayments(tblPayments tblPayments)
        {
            _context.tblPayments.Add(tblPayments);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GettblPayments", new { id = tblPayments.Id }, tblPayments);
        }

        // DELETE: api/tblPayments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<tblPayments>> DeletetblPayments(int id)
        {
            var tblPayments = await _context.tblPayments.FindAsync(id);
            if (tblPayments == null)
            {
                return NotFound();
            }

            _context.tblPayments.Remove(tblPayments);
            await _context.SaveChangesAsync();

            return tblPayments;
        }

        private bool tblPaymentsExists(int id)
        {
            return _context.tblPayments.Any(e => e.Id == id);
        }
    }
}
