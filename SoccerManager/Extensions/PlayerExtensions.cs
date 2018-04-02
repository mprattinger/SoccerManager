using SoccerManager.Data.DTO;
using SoccerManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerManager.Extensions
{
    public static class PlayerExtensions
    {
        public static PlayerViewModel MapToListViewModel(this Player player)
        {
            var vm = new PlayerViewModel();
            vm.PlayerId = player.PlayerId;
            vm.FirstName = player.Person.FirstName;
            vm.LastName = player.Person.LastName;
            vm.ClubId = player.ClubId;
            vm.Club = player.Club;
            return vm;
        }
    }
}
