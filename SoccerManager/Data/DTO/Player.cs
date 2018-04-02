using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoccerManager.Data.DTO
{
    public class Player : IAuditable
    {
        public Guid PlayerId { get; set; }
        
        [Required]
        public Guid PersonId { get; set; }
        public Person Person { get; set; }

        [Required]
        public Guid ClubId { get; set; }
        public Club Club { get; set; }
    }
}
