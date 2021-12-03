using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Studentenbeheer.Data;
using Studentenbeheer.Models;

namespace Studentenbeheer.Controllers
{
    public class InschrijvingensController : Controller
    {
        private readonly StudentenbeheerContext _context;

        public InschrijvingensController(StudentenbeheerContext context)
        {
            _context = context;
        }

        // GET: Inschrijvingens
        public async Task<IActionResult> Index()
        {
            var studentenbeheerContext = _context.Inschrijvingen.Include(i => i.Module).Include(i => i.Student);
            return View(await studentenbeheerContext.ToListAsync());
        }

        // GET: Inschrijvingens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inschrijvingen = await _context.Inschrijvingen
                .Include(i => i.Module)
                .Include(i => i.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inschrijvingen == null)
            {
                return NotFound();
            }

            return View(inschrijvingen);
        }

        // GET: Inschrijvingens/Create
        public IActionResult Create()
        {
            ViewData["ModuleId"] = new SelectList(_context.Module, "Id", "Naam");
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "Achternaam");
            return View();
        }

        // POST: Inschrijvingens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StudentId,ModuleId,Inschrijvingsdatum,AfgelegdOp,Resultaat")] Inschrijvingen inschrijvingen)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inschrijvingen);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ModuleId"] = new SelectList(_context.Module, "Id", "Naam", inschrijvingen.ModuleId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "Achternaam", inschrijvingen.StudentId);
            return View(inschrijvingen);
        }

        // GET: Inschrijvingens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inschrijvingen = await _context.Inschrijvingen.FindAsync(id);
            if (inschrijvingen == null)
            {
                return NotFound();
            }
            ViewData["ModuleId"] = new SelectList(_context.Module, "Id", "Naam", inschrijvingen.ModuleId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "Achternaam", inschrijvingen.StudentId);
            return View(inschrijvingen);
        }

        // POST: Inschrijvingens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentId,ModuleId,Inschrijvingsdatum,AfgelegdOp,Resultaat")] Inschrijvingen inschrijvingen)
        {
            if (id != inschrijvingen.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inschrijvingen);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InschrijvingenExists(inschrijvingen.Id))
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
            ViewData["ModuleId"] = new SelectList(_context.Module, "Id", "Naam", inschrijvingen.ModuleId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "Achternaam", inschrijvingen.StudentId);
            return View(inschrijvingen);
        }

        // GET: Inschrijvingens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inschrijvingen = await _context.Inschrijvingen
                .Include(i => i.Module)
                .Include(i => i.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inschrijvingen == null)
            {
                return NotFound();
            }

            return View(inschrijvingen);
        }

        // POST: Inschrijvingens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inschrijvingen = await _context.Inschrijvingen.FindAsync(id);
            _context.Inschrijvingen.Remove(inschrijvingen);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InschrijvingenExists(int id)
        {
            return _context.Inschrijvingen.Any(e => e.Id == id);
        }
    }
}
