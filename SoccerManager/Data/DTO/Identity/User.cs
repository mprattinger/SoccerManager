using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SoccerManager.Data.DTO.Identity
{
    public class User : IdentityUser
    {
        [Required]
        public bool IsProductivePassword { get; set; }
    }
}
