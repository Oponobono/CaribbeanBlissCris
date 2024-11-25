using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Caribbean2.Data;
using Caribbean2.Models;
using Microsoft.CodeAnalysis.Scripting;

namespace Caribbean2.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuarios.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarios = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.UsuarioID == id);
            if (usuarios == null)
            {
                return NotFound();
            }

            return View(usuarios);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UsuarioID,NombresApellidos,TipoIdentificacion,Identificacion,Telefono,Correo,Contrasena,FechaRegistro,Estado")] Usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuarios);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuarios);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarios = await _context.Usuarios.FindAsync(id);
            if (usuarios == null)
            {
                return NotFound();
            }
            return View(usuarios);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UsuarioID,NombresApellidos,TipoIdentificacion,Identificacion,Telefono,Correo,Contrasena,FechaRegistro,Estado")] Usuarios usuarios)
        {
            if (id != usuarios.UsuarioID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuarios);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuariosExists(usuarios.UsuarioID))
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
            return View(usuarios);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarios = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.UsuarioID == id);
            if (usuarios == null)
            {
                return NotFound();
            }

            return View(usuarios);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuarios = await _context.Usuarios.FindAsync(id);
            if (usuarios != null)
            {
                _context.Usuarios.Remove(usuarios);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuariosExists(int id)
        {
            return _context.Usuarios.Any(e => e.UsuarioID == id);
        }

        // GET: Usuarios/Register
        public IActionResult Register()
        {
            ViewData["RolId"] = new SelectList(_context.Roles, "IdRol", "DescripcionRol");
            return View();
        }

        // POST: Usuarios/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("NombresApellidos,TipoIdentificacion,Identificacion,Telefono,Correo,Contrasena")] Usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                // Verifica si ya existe un usuario con el mismo correo
                var existingUser = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Correo == usuarios.Correo);

                if (existingUser != null)
                {
                    ViewBag.Error = "Ya existe un usuario con este correo.";
                    return View(usuarios);
                }


                // Agregar fecha de registro y estado predeterminado
                usuarios.FechaRegistro = DateTime.Now;
                usuarios.Estado = true;

                // Guardar el nuevo usuario
                _context.Add(usuarios);
                await _context.SaveChangesAsync();

                // Redirige al Login tras el registro
                return RedirectToAction("Index", "Login");
            }
            ViewData["RolId"] = new SelectList(_context.Roles, "IdRol", "DescripcionRol", usuarios.IdRol);
            return View(usuarios);
        }

    }
}
