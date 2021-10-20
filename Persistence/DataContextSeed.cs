using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Persistence
{
    public class DataContextSeed
    {
        private readonly UserManager<AppUser> _userManager;

        public static async Task SeedAsync(DataContext context, ILoggerFactory loggerFactory,
            IConfiguration configuration, UserManager<AppUser> userManager)
        {
            try
            {
                if (!context.Users.Any())
                {
                    var usersData = await File.ReadAllTextAsync("../Persistence/SeedData/adminSeed.json");
                    var users = JsonSerializer.Deserialize<List<AppUser>>(usersData);

                    if (users != null)
                    {
                        foreach (var user in users)
                        {
                            await userManager.CreateAsync(user, configuration["AdminPassword"]);
                        }
                    }

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                var logger = loggerFactory.CreateLogger<DataContextSeed>();
                logger.LogError(e, e.Message);
            }
        }
    }
}