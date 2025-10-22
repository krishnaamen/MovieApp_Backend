using System.ComponentModel.DataAnnotations;

namespace MovieAppPortfolio.DataServiceLayer
{
    public class User
    {
        public int UserId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string? Username { get; set; }
        
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        
        [Required]
        public string? Password { get; set; }
        
        public DateTime? CreatedAt { get; set; }
    }
}