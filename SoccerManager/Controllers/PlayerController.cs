using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoccerManager.Data;
using SoccerManager.Data.DTO;
using SoccerManager.Extensions;
using SoccerManager.Models;

namespace SoccerManager.Controllers
{
    [Produces("application/json")]
    [Route("api/Player")]
    public class PlayerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlayerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Player
        [HttpGet]
        public async Task<IEnumerable<PlayerViewModel>> GetPlayers()
        {
            var ret = new List<PlayerViewModel>();
            var players = await _context.Players
                .Include(x => x.Person).AsNoTracking()
                .Include(y => y.Club).AsNoTracking()
                .ToListAsync();
            foreach(var p in players)
            {
                var squads = await _context.Squads.Where(s => s.PlayerId == p.PlayerId).ToListAsync();
                var teams = await _context.Teams.Where(t => squads.Any(x => x.TeamId == t.TeamId)).ToListAsync();
                var pvm = p.MapToListViewModel();
                pvm.Teams = teams;
                ret.Add(pvm);
            }
            return ret;
        }

        // GET: api/Player/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayer([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var player = await _context.Players
                .Include(x => x.Person).AsNoTracking()
                .Include(y => y.Club).AsNoTracking()
                .SingleOrDefaultAsync(m => m.PlayerId == id);

            if (player == null)
            {
                return NotFound();
            }

            var squads = await _context.Squads.Where(s => s.PlayerId == player.PlayerId).ToListAsync();
            var teams = await _context.Teams.Where(t => squads.Any(x => x.TeamId == t.TeamId)).ToListAsync();
            var pvm = player.MapToListViewModel();
            pvm.Teams = teams;

            return Ok(player);
        }

        // PUT: api/Player/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlayer([FromRoute] Guid id, [FromBody] Player player)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != player.PlayerId)
            {
                return BadRequest();
            }

            _context.Entry(player).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Player
        [HttpPost]
        public async Task<IActionResult> PostPlayer([FromBody] Player player)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlayer", new { id = player.PlayerId }, player);
        }

        // DELETE: api/Player/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var player = await _context.Players.SingleOrDefaultAsync(m => m.PlayerId == id);
            if (player == null)
            {
                return NotFound();
            }

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();

            return Ok(player);
        }

        private bool PlayerExists(Guid id)
        {
            return _context.Players.Any(e => e.PlayerId == id);
        }
    }
}