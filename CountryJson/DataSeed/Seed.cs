using CountryJson.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace CountryJson.DataSeed
{
    public class Seed
    {
        private readonly ApplicationDbContext _context;

        public Seed(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SeedDataAsync()
        {
            // Seed States and Districts
            var states = LoadStatesFromJson();

            foreach (var state in states)
            {
                var existingState = await _context.States
                    .Include(s => s.Districts)
                    .FirstOrDefaultAsync(s => s.StateName == state.StateName);

                if (existingState == null)
                {
                    // Add new state if not present
                    await _context.States.AddAsync(state);
                }
                else
                {
                    // Ensure districts are added to the state if not already present
                    foreach (var district in state.Districts)
                    {
                        if (!existingState.Districts.Any(d => d.Name == district.Name))
                        {
                            existingState.Districts.Add(district);
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        private static List<State> LoadStatesFromJson()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Json", "list.json");
            var jsonData = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<State>>(jsonData, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
