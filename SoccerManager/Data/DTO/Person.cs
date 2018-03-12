using SoccerManager.Data.DTO.Identity;
using SoccerManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerManager.Data.DTO
{
    public class Person
    {
        public int PersonId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public PersonType[] PersonTypes { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
