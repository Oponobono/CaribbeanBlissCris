using Microsoft.AspNetCore.Identity;

namespace Caribbean2.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
    }
}
