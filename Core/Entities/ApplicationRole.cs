using Microsoft.AspNetCore.Identity;

namespace Core.Entities
{
    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; } // Role ait açıklama (Örn: "Sadece teklifleri görebilir")
    }
}