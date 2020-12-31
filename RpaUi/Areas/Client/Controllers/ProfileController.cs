using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RpaData.DataContext;
using RpaData.Models;
using RpaData.Models.ViewModels;

namespace RpaUi.Areas.Client.Controllers
{
    [Authorize]
    [Area("Client")]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Client/Profile
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblPharmacists = await _context.tblPharmacists.FindAsync(id);
            if (tblPharmacists == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Users, "Id", "Id", tblPharmacists.ApplicationUserId);
            ViewData["tblPharmacyid"] = new SelectList(_context.tblPharmacies, "id", "pharmacyName", tblPharmacists.tblPharmacyid);
            var ProfileViewModel = new ProfileViewModel {
                Id = tblPharmacists.Id,
                Created = tblPharmacists.Created,
                ClientId = tblPharmacists.ApplicationUserId,
                tblPharmacyid = tblPharmacists.tblPharmacyid,
                profileComplete = tblPharmacists.profileComplete,
                qualifications = tblPharmacists.qualifications,
                workAddress = tblPharmacists.workAddress,
                goodStandingMCAZ = tblPharmacists.goodStandingMCAZ,
                goodStandingReasonMCAZ = tblPharmacists.goodStandingReasonMCAZ,
                goodStandingPCZ = tblPharmacists.goodStandingPCZ,
                goodStandingReasonPCZ = tblPharmacists.goodStandingReasonPCZ,
                dateOfJoiningRPA = tblPharmacists.dateOfJoiningRPA,
                yearsInPractice = tblPharmacists.yearsInPractice

            };

            return View(ProfileViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int id, [Bind("ClientId,tblPharmacyid,workAddress,qualifications,yearsInPractice,dateOfJoiningRPA,goodStandingPCZ,goodStandingReasonPCZ,goodStandingMCAZ,goodStandingReasonMCAZ,Id,Created")] ProfileViewModel tblPharmacists)
        {
            if (id != tblPharmacists.Id)
            {
                return NotFound();
            }
            tblPharmacists.profileComplete = true;

            if (ModelState.IsValid)
            {
           

                var tblPharmacist = new tblPharmacists
                {
                    Id = tblPharmacists.Id,
                    Created = tblPharmacists.Created,
                    ApplicationUserId = tblPharmacists.ClientId,
                    tblPharmacyid = tblPharmacists.tblPharmacyid,
                    profileComplete = tblPharmacists.profileComplete,
                    qualifications = tblPharmacists.qualifications,
                    workAddress = tblPharmacists.workAddress,
                    goodStandingMCAZ = tblPharmacists.goodStandingMCAZ,
                    goodStandingReasonMCAZ = tblPharmacists.goodStandingReasonMCAZ,
                    goodStandingPCZ = tblPharmacists.goodStandingPCZ,
                    goodStandingReasonPCZ = tblPharmacists.goodStandingReasonPCZ,
                    dateOfJoiningRPA = tblPharmacists.dateOfJoiningRPA,
                    yearsInPractice = tblPharmacists.yearsInPractice
                    

                };

                try
                {
                    _context.Update(tblPharmacist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!tblPharmacistsExists(tblPharmacists.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index),"Home");
            }
            ViewData["ClientId"] = new SelectList(_context.Users, "Id", "Id", tblPharmacists.ClientId);
            ViewData["tblPharmacyid"] = new SelectList(_context.tblPharmacies, "id", "pharmacyName", tblPharmacists.tblPharmacyid);
            return View(tblPharmacists);
        }

        // GET: Client/Profile/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblPharmacists = await _context.tblPharmacists
                .Include(t => t.ApplicationUser)
                .Include(t => t.tblPharmacy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblPharmacists == null)
            {
                return NotFound();
            }

            return View(tblPharmacists);
        }

        // GET: Client/Profile/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["tblPharmacyid"] = new SelectList(_context.tblPharmacies, "id", "pharmacyAddress");
            return View();
        }

        // POST: Client/Profile/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClientId,tblPharmacyid,workAddress,qualifications,yearsInPractice,dateOfJoiningRPA,goodStandingPCZ,goodStandingReasonPCZ,goodStandingMCAZ,goodStandingReasonMCAZ,profileComplete,Id,Created")] tblPharmacists tblPharmacists)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblPharmacists);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Users, "Id", "Id", tblPharmacists.ApplicationUserId);
            ViewData["tblPharmacyid"] = new SelectList(_context.tblPharmacies, "id", "pharmacyAddress", tblPharmacists.tblPharmacyid);
            return View(tblPharmacists);
        }

        // GET: Client/Profile/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblPharmacists = await _context.tblPharmacists.FindAsync(id);
            if (tblPharmacists == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Users, "Id", "Id", tblPharmacists.ApplicationUserId);
            ViewData["tblPharmacyid"] = new SelectList(_context.tblPharmacies, "id", "pharmacyAddress", tblPharmacists.tblPharmacyid);
            return View(tblPharmacists);
        }

        // POST: Client/Profile/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClientId,tblPharmacyid,workAddress,qualifications,yearsInPractice,dateOfJoiningRPA,goodStandingPCZ,goodStandingReasonPCZ,goodStandingMCAZ,goodStandingReasonMCAZ,Id,Created")] tblPharmacists tblPharmacists)
        {
            if (id != tblPharmacists.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                tblPharmacists.profileComplete = true;
                try
                {
                    _context.Update(tblPharmacists);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!tblPharmacistsExists(tblPharmacists.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Users, "Id", "Id", tblPharmacists.ApplicationUserId);
            ViewData["tblPharmacyid"] = new SelectList(_context.tblPharmacies, "id", "pharmacyAddress", tblPharmacists.tblPharmacyid);
            return View(tblPharmacists);
        }

        // GET: Client/Profile/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblPharmacists = await _context.tblPharmacists
                .Include(t => t.ApplicationUser)
                .Include(t => t.tblPharmacy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblPharmacists == null)
            {
                return NotFound();
            }

            return View(tblPharmacists);
        }

        // POST: Client/Profile/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblPharmacists = await _context.tblPharmacists.FindAsync(id);
            _context.tblPharmacists.Remove(tblPharmacists);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool tblPharmacistsExists(int id)
        {
            return _context.tblPharmacists.Any(e => e.Id == id);
        }
    }
}
