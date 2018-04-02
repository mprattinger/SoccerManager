using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerManager.Data.DTO
{
    public class Game : IAuditable
    {
        public Guid GameId { get; set; }
        [Required]
        public DateTime GameDay { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Place { get; set; }
        public bool Home { get; set; }

        public Guid TeamId { get; set; }
        public Team Team { get; set; }
    }
}
