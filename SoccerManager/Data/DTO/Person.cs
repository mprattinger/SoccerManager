using SoccerManager.Data.DTO.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace SoccerManager.Data.DTO
{
    public class Person : IAuditable
    {
        public Guid PersonId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PersonTypes { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
