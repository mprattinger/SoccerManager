using System.ComponentModel.DataAnnotations;

namespace SoccerManager.Data.DTO
{
    public class Player : IAuditable
    {
        public int PlayerId { get; set; }
        
        [Required]
        public int PersonId { get; set; }
        public Person Person { get; set; }

        [Required]
        public int ClubId { get; set; }
        public Club Club { get; set; }
    }
}
