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
    public class MetricasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MetricasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Metricas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Metricas.ToListAsync());
        }

        // GET: Metricas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metrica = await _context.Metricas
                .FirstOrDefaultAsync(m => m.IdMetrica == id);
            if (metrica == null)
            {
                return NotFound();
            }

            return View(metrica);
        }

        // GET: Metricas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Metricas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdMetrica,Fecha,IngresosTotales,TasaOcupacion,OcupacionDiaria,OcupacionSemanal,OcupacionMensual,ReservasNuevas,Cancelaciones,ImpactoFinancieroCancelaciones,PromedioDiasAnticipacionReserva,NumeroHuespedes,SuscritosBoletin,DuracionPromedioEstadia")] Metrica metrica)
        {
            if (ModelState.IsValid)
            {
                _context.Add(metrica);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(metrica);
        }

        // GET: Metricas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metrica = await _context.Metricas.FindAsync(id);
            if (metrica == null)
            {
                return NotFound();
            }
            return View(metrica);
        }

        // POST: Metricas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdMetrica,Fecha,IngresosTotales,TasaOcupacion,OcupacionDiaria,OcupacionSemanal,OcupacionMensual,ReservasNuevas,Cancelaciones,ImpactoFinancieroCancelaciones,PromedioDiasAnticipacionReserva,NumeroHuespedes,SuscritosBoletin,DuracionPromedioEstadia")] Metrica metrica)
        {
            if (id != metrica.IdMetrica)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(metrica);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MetricaExists(metrica.IdMetrica))
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
            return View(metrica);
        }

        // GET: Metricas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metrica = await _context.Metricas
                .FirstOrDefaultAsync(m => m.IdMetrica == id);
            if (metrica == null)
            {
                return NotFound();
            }

            return View(metrica);
        }

        // POST: Metricas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var metrica = await _context.Metricas.FindAsync(id);
            if (metrica != null)
            {
                _context.Metricas.Remove(metrica);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MetricaExists(int id)
        {
            return _context.Metricas.Any(e => e.IdMetrica == id);
        }
    }
}
