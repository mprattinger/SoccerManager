using System.ComponentModel.DataAnnotations;

namespace SoccerManager.Data.DTO
{
    public class Team : IAuditable
    {
        public int TeamId { get; set; }
        [Required]
        public string TeamName { get; set; }

        [Required]
        public int ClubId { get; set; }
        public Club Club { get; set; }
    }
}
