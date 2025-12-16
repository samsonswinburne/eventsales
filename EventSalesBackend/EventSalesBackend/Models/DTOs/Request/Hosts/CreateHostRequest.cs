using System.ComponentModel.DataAnnotations;

namespace EventSalesBackend.Models.DTOs.Request.Hosts
{
    public class CreateHostRequest
    {
        [Required]
        public required string FirstName { get; set; }
        [Required]
        public required string LastName { get; set; }
        [Required]
        public required DateOnly BirthDate { get; set; }

    }
}
