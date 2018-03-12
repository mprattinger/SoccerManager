using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerManager.Data.DTO.Identity
{
    public class UserRole : IdentityRole
    {
        [Display(Name = "Rollenbeschreibung")]
        public string Description { get; set; }
    }
}
