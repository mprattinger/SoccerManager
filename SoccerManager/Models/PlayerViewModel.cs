using SoccerManager.Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerManager.Models
{
    public class PlayerViewModel
    {
        public Guid PlayerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid ClubId { get; set; }
        public Club Club { get; set; }
        public List<Team> Teams { get; set; }
    }
}
