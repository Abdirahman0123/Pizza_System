using Sieve.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Pizza_System.Model
{
    public class Menu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Sieve(CanFilter = true)]
        [Required(ErrorMessage ="Name is required")]
        public string? Name { get; set; }

        [Sieve(CanFilter = true)]
        [Required(ErrorMessage = "Toppings is required")]
        public string? Toppings { get; set; }

        [Required(ErrorMessage = "Crust is required")]
        public string? Crust { get; set; }

        [Sieve(CanSort =true, CanFilter = true)]
        [Required(ErrorMessage = "Size is required")]
        [Range(5, int.MaxValue, ErrorMessage = "Minimum 5 inch size")]
        public long Size { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        [Required(ErrorMessage = "Vegan is required: true or false")]
        public Boolean Vegan { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        [Required(ErrorMessage = "Price is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Price must be greater than 1")]
        public long Price { get; set; }

        [IgnoreDataMember] 
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
