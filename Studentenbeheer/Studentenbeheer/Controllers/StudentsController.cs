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
    public class StudentsController : Controller
    {
        private readonly StudentenbeheerContext _context;

        public StudentsController(StudentenbeheerContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(string searchFieldName, char searchGender, string orderBy)
        {
            //var students = from s in _context.Student
            //                         select s;

            //if (searchGender != 0)
            //{
            //    students = from s in _context.Student
            //               where s.GeslachtId == searchGender
            //               select s;
            //}

            //if (!string.IsNullOrEmpty(searchFieldName))
            //{
            //    students = from s in students
            //                         where s.Achternaam.Contains(searchFieldName) || s.Voornaam.Contains(searchFieldName)
            //                         orderby s.Achternaam, s.Voornaam
            //                         select s;
            //}

            //var studentenbeheerContext = _context.Student.Include(s => s.Geslacht);

            //ViewData["genderId"] = new SelectList(_context.Gender.ToList(), "ID", "Name");

            //await studentenbeheerContext.ToListAsync();
            //return View(await students.ToListAsync());

            // Lijst alle message op.  We gebruiken Linq
            var students = from m in _context.Student select m;

            // Pas de groepfilter (selectedGroup) toe als deze niet leeg is
            if (searchGender != 0)
                students = from s in _context.Student
                           where s.GeslachtId == searchGender
                           select s;

            //Pas de titleFilter toe(als deze niet leeg is) en zorg dat de groep-instanties daaraan toegevoegd zijn en sorteer
            if (!string.IsNullOrEmpty(searchFieldName))
                students = from s in students
                           where s.Achternaam.Contains(searchFieldName) || s.Voornaam.Contains(searchFieldName)
                           orderby s.Achternaam, s.Voornaam
                           select s;

            ViewData["VoornaamField"] = string.IsNullOrEmpty(orderBy) ? "Voornaam_Desc" : "Voornaam";
            ViewData["AchternaamField"] = string.IsNullOrEmpty(orderBy) ? "Achetrnaam_Desc" : "Achternaam";
            ViewData["GeboortedatumField"] = string.IsNullOrEmpty(orderBy) ? "Geboortedatum_Desc" : "";

            switch (orderBy)
            {
                case "Voornaam":
                    students = students.OrderBy(s => s.Voornaam);
                    break;
                case "Voornaam_Desc":
                    students = students.OrderByDescending(s => s.Voornaam);
                    break;
                case "Achternaam":
                    students = students.OrderBy(s => s.Achternaam);
                    break;
                case "Achernaam_Desc":
                    students = students.OrderByDescending(s => s.Achternaam);
                    break;
                case "Geboortedatum_Desc":
                    students = students.OrderByDescending(s => s.Geboortedatum);
                    break;
                default:
                    students = students.OrderBy(s => s.Geboortedatum);
                    break;
            }

            // Lijst van groepen 
            IQueryable<Gender> genderToSelect = from g in _context.Gender orderby g.Name select g;

            // Maak een object van de view-model-class en voeg daarin alle wat we nodig hebben
            StudentIndexViewModel studentIndexViewModel = new StudentIndexViewModel()
            {
        //        public String VoornaamFilter { get; set; }
        //public string AchternaamFilter { get; set; }
        //[DataType(DataType.Date)]
        //public DateTime GeboortedatumFilter { get; set; }
        //public List<Student> FilteredStudent { get; set; }
        //public SelectList GenderToSelect { get; set; }
                VoornaamFilter = searchFieldName,
                AchternaamFilter = searchFieldName,
                GenderToSelect = new SelectList(await genderToSelect.ToListAsync(), "Id", "Name", searchGender)
            };
            return View(studentIndexViewModel);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(s => s.Geslacht)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            ViewData["GeslachtId"] = new SelectList(_context.Gender, "ID", "Name");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Voornaam,Achternaam,Geboortedatum,GeslachtId")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GeslachtId"] = new SelectList(_context.Gender, "ID", "Name", student.GeslachtId);
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["GeslachtId"] = new SelectList(_context.Gender, "ID", "Name", student.GeslachtId);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Voornaam,Achternaam,Geboortedatum,GeslachtId")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
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
            ViewData["GeslachtId"] = new SelectList(_context.Gender, "ID", "Name", student.GeslachtId);
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(s => s.Geslacht)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Student.FindAsync(id);
            _context.Student.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.Id == id);
        }
    }
}
