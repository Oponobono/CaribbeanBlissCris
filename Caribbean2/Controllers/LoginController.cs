using Caribbean2.Data;
using Caribbean2.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Caribbean2.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Login
        public IActionResult Index()
        {
            try
            {
                if (HttpContext.Session.GetString("UsuarioID") != null)
                {
                    return RedirectToAction("Index", "Caribbean");
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["AlertType"] = "error";
                TempData["AlertMessage"] = $"Error al cargar la página de inicio de sesión: {ex.Message}";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            try
            {
                Console.WriteLine("Email: " + email);

                // Buscar el usuario en la base de datos
                var user = _context.Usuarios
                    .FirstOrDefault(e => e.Correo == email && e.Contrasena == password);

                if (user != null)
                {
                    var userRole = _context.Roles.FirstOrDefault(r => r.IdRol == user.IdRol);

                    if (userRole != null)
                    {
                        HttpContext.Session.SetString("UserRole", userRole.DescripcionRol);
                        HttpContext.Session.SetInt32("UserRoleId", user.IdRol);
                        HttpContext.Session.SetInt32("UserId", user.UsuarioID);

                        TempData["AlertType"] = "success";
                        TempData["AlertMessage"] = "Inicio de sesión exitoso";
                        return RedirectToAction("Index", "Caribbean");
                    }

                }

                TempData["AlertType"] = "error";
                TempData["AlertMessage"] = "Credenciales incorrectas. Por favor, intenta de nuevo.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["AlertType"] = "error";
                TempData["AlertMessage"] = $"Ocurrió un error al iniciar sesión: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public IActionResult Logout()
        {
            try
            {
                HttpContext.Session.Clear();
                TempData["AlertType"] = "info";
                TempData["AlertMessage"] = "Has cerrado sesión exitosamente.";
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                TempData["AlertType"] = "error";
                TempData["AlertMessage"] = $"Error al cerrar sesión: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public IActionResult AccessDenied()
        {
            TempData["AlertType"] = "warning";
            TempData["AlertMessage"] = "No tienes permiso para acceder a esta página.";
            return RedirectToAction("Index", "Login");
        }
    }
}
