using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerManager.Data.DTO
{
    public class Squad : IAuditable
    {
        public Guid SquadId { get; set; }
        public Guid TeamId { get; set; }
        public Guid PlayerId { get; set; }
    }
}
