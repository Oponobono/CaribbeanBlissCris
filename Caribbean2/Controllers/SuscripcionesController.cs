using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Caribbean2.Data;
using Caribbean2.Models;

namespace Caribbean2.Controllers
{
    public class SuscripcionesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SuscripcionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Suscripciones
        public async Task<IActionResult> Index()
        {
            return View(await _context.Suscripciones.ToListAsync());
        }

        // GET: Suscripciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suscripcion = await _context.Suscripciones
                .FirstOrDefaultAsync(m => m.IdSuscripcion == id);
            if (suscripcion == null)
            {
                return NotFound();
            }

            return View(suscripcion);
        }

        // GET: Suscripciones/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Suscripciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSuscripcion,Nombre,Email,FechaSuscripcion")] Suscripcion suscripcion)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(suscripcion);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "Registro exitoso." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = $"Error: {ex.Message}" });
                }
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = "Datos inválidos: " + string.Join(", ", errors) });
            }

            return Json(new { success = false, message = "Datos inválidos, por favor verifique." });
        }

        // GET: Suscripciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suscripcion = await _context.Suscripciones.FindAsync(id);
            if (suscripcion == null)
            {
                return NotFound();
            }
            return View(suscripcion);
        }

        // POST: Suscripciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdSuscripcion,Nombre,Email,FechaSuscripcion")] Suscripcion suscripcion)
        {
            if (id != suscripcion.IdSuscripcion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(suscripcion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuscripcionExists(suscripcion.IdSuscripcion))
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
            return View(suscripcion);
        }

        // GET: Suscripciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suscripcion = await _context.Suscripciones
                .FirstOrDefaultAsync(m => m.IdSuscripcion == id);
            if (suscripcion == null)
            {
                return NotFound();
            }

            return View(suscripcion);
        }

        // POST: Suscripciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var suscripcion = await _context.Suscripciones.FindAsync(id);
            if (suscripcion != null)
            {
                _context.Suscripciones.Remove(suscripcion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SuscripcionExists(int id)
        {
            return _context.Suscripciones.Any(e => e.IdSuscripcion == id);
        }
    }
}
