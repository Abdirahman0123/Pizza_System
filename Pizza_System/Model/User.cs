using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pizza_System.Model
{
    public class User: IdentityUser
    {
        internal static readonly object Identity;

        [Column("firstName")]
        public string? FirstName { get; set; }
        [Column("lastName")]
        public string? LastName { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
