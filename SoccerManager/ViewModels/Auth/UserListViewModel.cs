using SoccerManager.Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerManager.ViewModels.Auth
{
    public class UserListViewModel
    {
        public string UserName { get; set; }
        public string EMail { get; set; }
        public bool IsProductivePassword { get; set; }
        public bool IsLoginAllowed { get; set; }
        public List<Person> AssignedPersons { get; set; }
    }
}
