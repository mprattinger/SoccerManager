using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoccerManager.Data.DTO
{
    public class Club : IAuditable
    {
        public Guid ClubId { get; set; }
        [Required]
        public string ClubName { get; set; }

        public IEnumerable<Team> ClubTeams { get; set; }
    }
}
