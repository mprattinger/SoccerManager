using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoccerManager.Data.DTO
{
    public class Team : IAuditable
    {
        public Guid TeamId { get; set; }
        [Required]
        public string TeamName { get; set; }

        [Required]
        public Guid ClubId { get; set; }
        public Club Club { get; set; }

        public List<Game> Games { get; set; }
    }
}
