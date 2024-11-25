using Microsoft.AspNetCore.Mvc;
using Caribbean2.Data;
using Caribbean2.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Caribbean2.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerMetricas(DateTime? startDate, DateTime? endDate)
        {
            startDate ??= DateTime.Now.AddMonths(-1); // Último mes por defecto
            endDate ??= DateTime.Now;

            var metricas = await _context.Metricas
                .Where(m => m.Fecha >= startDate && m.Fecha <= endDate)
                .ToListAsync();

            return Json(metricas);
        }

        public IActionResult Index()
        {
            var metricas = _context.Metricas
            .OrderByDescending(m => m.Fecha)
            .Take(1) // Cambiar si necesitas múltiples registros
            .FirstOrDefault();

            return View(metricas);
        }

        [HttpGet]
        public JsonResult GetIngresosChartData()
        {
            var data = _context.Metricas
                .OrderBy(m => m.Fecha)
                .Select(m => new { Fecha = m.Fecha, IngresosTotales = m.IngresosTotales })
                .ToList();

            return Json(data);
        }
    }
}
