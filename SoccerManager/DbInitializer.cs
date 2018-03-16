using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoccerManager.Data;
using SoccerManager.Data.DTO.Identity;
using Microsoft.AspNetCore.Identity;
using SoccerManager.Data.DTO;
using SoccerManager.Models;

namespace SoccerManager
{
    public class DbInitializer
    {
        private static ApplicationDbContext _context;
        private static UserManager<User> _userManager;
        private static ILogger _logger;
        private static IConfiguration _configuration;
        private static RoleManager<UserRole> _roleManager;

        public static void Initialize(IServiceProvider serviceProvider)
        {
            var loggerF = serviceProvider.GetService<ILoggerFactory>();
            _context = serviceProvider.GetService<ApplicationDbContext>();
            _configuration = serviceProvider.GetService<IConfiguration>();
            _roleManager = serviceProvider.GetService<RoleManager<UserRole>>();
            _userManager = serviceProvider.GetService<UserManager<User>>();

            _logger = loggerF.CreateLogger("DbInitializer");

            //_logger.LogInformation("Creating roles and users...");
            //if (_context.Roles.Any())
            //{
            //    _logger.LogInformation("Roles created!");
            //}
            //else
            //{
            //    _logger.LogInformation("Creating admin role...");
            //    createAdminRole();
            //    //_logger.LogInformation("Creating user role...");
            //    //createUserRole();
            //}

            User adminUser = null;
            if (_context.Users.Any())
            {
                _logger.LogInformation("Users created!");
                adminUser = _userManager.FindByEmailAsync("admin@admin.com").Result;
            }
            else
            {
                _logger.LogInformation("Creating admin user...");
                adminUser = createAdminUser();
                //_logger.LogInformation("Add admin user to admin role");
                //addAdmin2Role(adminUser);
            }

            _logger.LogInformation("Seeding database...");
            var clubs = createClubs();
            createTeams(clubs);
            var persons = createPersons(adminUser);
            createPlayers(clubs, persons);
            _context.SaveChanges();
        }



        #region User and Roles
        //private static void createAdminRole()
        //{
        //    var role = new UserRole { Name = "admin", Description = "Administrator" };
        //    var result = _roleManager.CreateAsync(role).Result;
        //    if (result.Succeeded)
        //    {
        //        _logger.LogInformation("Admin role created!");
        //    }
        //    else
        //    {
        //        var err = string.Join(" ", result.Errors.Select(e => e.Code + ": " + e.Description));
        //        _logger.LogInformation("When creating the admin role, the following errors occured: " + err);
        //        throw new ApplicationException("Error creating a role");
        //    }
        //}

        //private static void createUserRole()
        //{
        //    var role = new UserRole { Name = "user", Description = "Benutzer" };
        //    var result = _roleManager.CreateAsync(role).Result;
        //    if (result.Succeeded)
        //    {
        //        _logger.LogInformation("User role created!");
        //    }
        //    else
        //    {
        //        var err = string.Join(" ", result.Errors.Select(e => e.Code + ": " + e.Description));
        //        _logger.LogInformation("When creating the user role, the following errors occured: " + err);
        //        throw new ApplicationException("Error creating a role");
        //    }
        //}

        private static User createAdminUser()
        {
            var user = new User { UserName = "admin@admin.com", Email = "admin@admin.com", IsProductivePassword = true, LoginAllowed = true };
            var result = _userManager.CreateAsync(user, "At4711").Result;
            if (result.Succeeded)
            {
                _logger.LogInformation("Admin user created!");
            }
            else
            {
                var err = string.Join(" ", result.Errors.Select(e => e.Code + ": " + e.Description));
                _logger.LogInformation("When creating the admin user, the following errors occured: " + err);
                throw new ApplicationException("Error creating a user");
            }
            return user;
        }

        private static void addAdmin2Role(User user)
        {
            var result = _userManager.AddToRoleAsync(user, "admin").Result;
            if (result.Succeeded)
            {
                _logger.LogInformation("Admin user added to admin role!");
            }
            else
            {
                var err = string.Join(" ", result.Errors.Select(e => e.Code + ": " + e.Description));
                _logger.LogInformation("When adding the admin user to the admin role, the following errors occured: " + err);
                throw new ApplicationException("Error adding a user to role");
            }
        }
        #endregion

        #region Seeding
        private static Person[] createPersons(User adminUser)
        {
            _logger.LogInformation("Create persons");
            var ptypesAdmin = new PersonType[] { PersonType.ADMINISTRATOR };
            var ptypesPlayer = new PersonType[] { PersonType.PLAYER };
            var ptypesPlayerUser = new PersonType[] { PersonType.PLAYER, PersonType.USER };
            var persons = new Person[]
            {
                new Person { FirstName = "Admin", LastName = "User", PersonTypes = "100", UserId = adminUser.UserName },
                new Person{ FirstName = "Michael", LastName = "Prattinger", PersonTypes = "011"},
                new Person{ FirstName = "Christian", LastName = "Sulyok", PersonTypes = "001"},
                new Person{ FirstName = "Daniel", LastName = "Art", PersonTypes = "011"}
            };
            _context.Persons.AddRange(persons);
            return persons;
        }

        private static Club[] createClubs()
        {
            _logger.LogInformation("Creating clubs...");
            var clubs = new Club[]
            {
                new Club{ ClubName = "SV Steinberg"}
            };
            _context.Clubs.AddRange(clubs);
            return clubs;
        }

        private static void createTeams(Club[] clubs)
        {
            _logger.LogInformation("Creating teams...");
            var teams = new Team[]
            {
                new Team{ TeamName = "Kampfmannschaft", ClubId = clubs.FirstOrDefault().ClubId },
                new Team{ TeamName = "Reserve", ClubId = clubs.FirstOrDefault().ClubId }
            };
            _context.Teams.AddRange(teams);
        }

        private static void createPlayers(Club[] clubs, Person[] persons)
        {
            _logger.LogInformation("Creating players...");
            var players = new List<Player>();
            foreach (var p in persons)
            {
                var player = new Player { Person = p, Club = clubs.FirstOrDefault() };
                _context.Players.Add(player);
            }
        }
        #endregion
    }
}
