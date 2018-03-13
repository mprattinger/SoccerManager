using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerManager.ViewModels.Mappings
{
    public class ViewModelToEntityMappingProfile
    {
        public ViewModelToEntityMappingProfile()
        {
            //CreateMap<RegistrationViewModel, AppUser>().ForMember(au => au.UserName, map => map.MapFrom(vm => vm.Email));
        }
    }
}
