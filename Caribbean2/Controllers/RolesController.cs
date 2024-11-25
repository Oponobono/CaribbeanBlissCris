using Caribbean2.Data;
using Caribbean2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class RolesController : Controller
{
    private readonly ApplicationDbContext _context;

    public RolesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [RoleAuthorize(3, 4)]
    public async Task<IActionResult> Index()
    {
        try
        {
            return View(await _context.Roles.ToListAsync());
        }
        catch (Exception ex)
        {
            TempData["AlertType"] = "error";
            TempData["AlertMessage"] = "Error al cargar la lista de roles: " + ex.Message;
            return RedirectToAction("Error", "Home"); // Redirige a una vista de error personalizada si la tienes
        }
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            TempData["AlertType"] = "error";
            TempData["AlertMessage"] = "El ID del rol no puede ser nulo.";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            var rol = await _context.Roles
                .Include(r => r.Permisos)
                .FirstOrDefaultAsync(m => m.IdRol == id);

            if (rol == null)
            {
                TempData["AlertType"] = "error";
                TempData["AlertMessage"] = "No se encontró el rol solicitado.";
                return RedirectToAction(nameof(Index));
            }

            return View(rol);
        }
        catch (Exception ex)
        {
            TempData["AlertType"] = "error";
            TempData["AlertMessage"] = "Error al cargar los detalles del rol: " + ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    public IActionResult Create()
    {
        try
        {
            ViewBag.Permisos = _context.Permisos
                .Select(p => new { p.IdPermiso, p.NombrePermiso })
                .ToList();
            return View();
        }
        catch (Exception ex)
        {
            TempData["AlertType"] = "error";
            TempData["AlertMessage"] = "Error al cargar la vista de creación: " + ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IdRol,NombreRol,DescripcionRol,EstadoRol")] Rol rol, List<int> permisosSeleccionados)
    {
        try
        {
            bool rolExistente = await _context.Roles.AnyAsync(r => r.NombreRol == rol.NombreRol);
            if (rolExistente)
            {
                TempData["AlertType"] = "error";
                TempData["AlertMessage"] = "Ya existe un rol con este nombre.";
                return View(rol);
            }

            if (ModelState.IsValid)
            {
                if (permisosSeleccionados != null && permisosSeleccionados.Any())
                {
                    rol.Permisos = await _context.Permisos
                        .Where(p => permisosSeleccionados.Contains(p.IdPermiso))
                        .ToListAsync();
                }

                _context.Add(rol);
                await _context.SaveChangesAsync();

                TempData["AlertType"] = "success";
                TempData["AlertMessage"] = "Rol creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Permisos = await _context.Permisos.ToListAsync();
            return View(rol);
        }
        catch (Exception ex)
        {
            TempData["AlertType"] = "error";
            TempData["AlertMessage"] = "Error al crear el rol: " + ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            TempData["AlertType"] = "error";
            TempData["AlertMessage"] = "El ID del rol no puede ser nulo.";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            var rol = await _context.Roles.Include(r => r.Permisos).FirstOrDefaultAsync(r => r.IdRol == id);

            if (rol == null)
            {
                TempData["AlertType"] = "error";
                TempData["AlertMessage"] = "No se encontró el rol solicitado.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Permisos = await _context.Permisos.ToListAsync();
            return View(rol);
        }
        catch (Exception ex)
        {
            TempData["AlertType"] = "error";
            TempData["AlertMessage"] = "Error al cargar la vista de edición: " + ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IdRol,NombreRol,DescripcionRol,EstadoRol")] Rol rol, List<int> permisosSeleccionados)
    {
        if (id != rol.IdRol)
        {
            TempData["AlertType"] = "error";
            TempData["AlertMessage"] = "El ID del rol no coincide.";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            var rolActual = await _context.Roles.Include(r => r.Permisos).FirstOrDefaultAsync(r => r.IdRol == id);

            if (rolActual == null)
            {
                TempData["AlertType"] = "error";
                TempData["AlertMessage"] = "No se encontró el rol solicitado.";
                return RedirectToAction(nameof(Index));
            }

            rolActual.NombreRol = rol.NombreRol;
            rolActual.DescripcionRol = rol.DescripcionRol;
            rolActual.EstadoRol = rol.EstadoRol;

            rolActual.Permisos.Clear();
            if (permisosSeleccionados != null)
            {
                rolActual.Permisos = await _context.Permisos
                    .Where(p => permisosSeleccionados.Contains(p.IdPermiso))
                    .ToListAsync();
            }

            _context.Update(rolActual);
            await _context.SaveChangesAsync();

            TempData["AlertType"] = "success";
            TempData["AlertMessage"] = "Rol editado exitosamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["AlertType"] = "error";
            TempData["AlertMessage"] = "Error al editar el rol: " + ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    // GET: Roles/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        try
        {
            if (id == null)
            {
                TempData["AlertType"] = "warning";
                TempData["AlertMessage"] = "El ID del rol no puede ser nulo.";
                return RedirectToAction("Index");
            }

            var rol = await _context.Roles.FirstOrDefaultAsync(m => m.IdRol == id);
            if (rol == null)
            {
                TempData["AlertType"] = "error";
                TempData["AlertMessage"] = "El rol especificado no fue encontrado.";
                return RedirectToAction("Index");
            }

            return View(rol);
        }
        catch (Exception ex)
        {
            TempData["AlertType"] = "error";
            TempData["AlertMessage"] = $"Ocurrió un error al intentar cargar el rol para eliminar: {ex.Message}";
            return RedirectToAction("Index");
        }
    }


    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var rol = await _context.Roles.FindAsync(id);

            if (rol != null)
            {
                _context.Roles.Remove(rol);
                await _context.SaveChangesAsync();

                TempData["AlertType"] = "success";
                TempData["AlertMessage"] = "Rol eliminado exitosamente.";
            }
            else
            {
                TempData["AlertType"] = "error";
                TempData["AlertMessage"] = "El rol no existe.";
            }

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            if(id == 1 || id ==2 || id == 3 || id == 4)
            {
                TempData["AlertType"] = "error";
                TempData["AlertMessage"] = "No puedes eliminar un rol predefinido.";                
            }
            else
            {
                TempData["AlertType"] = "error";
                TempData["AlertMessage"] = "Error al eliminar el rol: " + ex.Message;
            }
            
            return RedirectToAction(nameof(Index));
        }
    }
}
