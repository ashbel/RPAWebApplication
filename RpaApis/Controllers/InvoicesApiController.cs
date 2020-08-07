using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RpaData.DataContext;
using RpaData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RpaApis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class InvoicesApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InvoicesApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/tblInvoices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<tblInvoices>>> GettblInvoices()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var clientId = identity.FindFirst("UserId").Value;

            var invoices = await _context.tblInvoices.ToListAsync();

            foreach(var item in invoices)
            {
                item.InvoiceType = _context.tblCodes.FirstOrDefault(c=>c.Id == item.InvoiceTypeId);
                item.tblPayments = _context.tblPayments.Where(c => c.InvoiceId == item.Id && c.ClientId == clientId).ToList();
            }

            return invoices;
        }

        // GET: api/tblInvoices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<tblInvoices>> GettblInvoices(int id)
        {
            var tblInvoices = await _context.tblInvoices.FindAsync(id);

            if (tblInvoices == null)
            {
                return NotFound();
            }

            return tblInvoices;
        }

        // PUT: api/tblInvoices/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PuttblInvoices(int id, tblInvoices tblInvoices)
        {
            if (id != tblInvoices.Id)
            {
                return BadRequest();
            }

            _context.Entry(tblInvoices).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tblInvoicesExists(id))
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

        // POST: api/tblInvoices
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<tblInvoices>> PosttblInvoices(tblInvoices tblInvoices)
        {
            _context.tblInvoices.Add(tblInvoices);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GettblInvoices", new { id = tblInvoices.Id }, tblInvoices);
        }

        // DELETE: api/tblInvoices/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<tblInvoices>> DeletetblInvoices(int id)
        {
            var tblInvoices = await _context.tblInvoices.FindAsync(id);
            if (tblInvoices == null)
            {
                return NotFound();
            }

            _context.tblInvoices.Remove(tblInvoices);
            await _context.SaveChangesAsync();

            return tblInvoices;
        }

        private bool tblInvoicesExists(int id)
        {
            return _context.tblInvoices.Any(e => e.Id == id);
        }
    }
}
