using SoccerManager.Data.DTO;
using SoccerManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerManager.Extensions
{
    public static class PersonExtension
    {
        public static List<PersonType> GetPersonTypes(this Person person)
        {
            var ret = new List<PersonType>();

            var flags = person.PersonTypes.ToCharArray();
            if (flags[0] == '1') ret.Add(PersonType.ADMINISTRATOR);
            if (flags[1] == '1') ret.Add(PersonType.PLAYER);
            if (flags[2] == '1') ret.Add(PersonType.USER);

            return ret;
        }
    }
}
