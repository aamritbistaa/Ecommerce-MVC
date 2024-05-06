using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class CompanyUser
    {public int Id {  get; set; }
        [Required]
        public string Name { get; set;}
        public string? Address { get; set;}
        public string? PhoneNumber {  get; set;}

    }
}
