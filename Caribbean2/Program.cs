using Caribbean2.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registrar el contexto de datos con el contenedor de dependencias
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configurar sesiones
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Habilitar el middleware de sesiones
app.UseSession();

// Middleware global para redirigir usuarios no autenticados
app.Use(async (context, next) =>
{
    var path = context.Request.Path.ToString().ToLower();

    // Permitir acceso sin autenticación a ciertas rutas específicas
    if (context.Session.GetInt32("UserRoleId") == null &&
        !path.Equals("/caribbean/index") && // Página de aterrizaje
        !path.StartsWith("/login") &&      // Login
        !path.StartsWith("/usuarios/register")) // Registro
    {
        context.Response.Redirect("/Login/Index");
        return;
    }

    await next();
});

// Autorizar después de configurar el middleware global
app.UseAuthorization();

// Configuración de rutas
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Caribbean}/{action=Index}/{id?}");
});

app.Run();

// Clase RoleAuthorizeAttribute
public class RoleAuthorizeAttribute : ActionFilterAttribute
{
    private readonly int[] _roles;

    public RoleAuthorizeAttribute(params int[] roles)
    {
        _roles = roles;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var userRoleId = context.HttpContext.Session.GetInt32("UserRoleId");

        if (userRoleId == null || !_roles.Contains(userRoleId.Value))
        {
            // Redirigir si no tiene uno de los roles necesarios
            context.Result = new RedirectToActionResult("AccessDenied", "Login", null);
        }

        base.OnActionExecuting(context);
    }
}
