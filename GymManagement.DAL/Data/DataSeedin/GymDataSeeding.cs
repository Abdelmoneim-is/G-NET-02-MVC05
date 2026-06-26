using GymManagement.DAL.Data.DbContexts;
using GymManagement.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymManagement.DAL.Data.DataSeedin
{
    public abstract class GymDataSeeding
    {
        public static async Task SeedingAsync (GymDbContext dbContext , string fileName ,ILogger logger , CancellationToken ct = default)
        {
            try
            {
                if(!await dbContext.Plans.AnyAsync())
                {


                    var plans = GetSeeddinWithJson<Plan>(fileName, "plans.json");
                    if (plans.Any())
                    {
                        dbContext.Plans.AddRange(plans);
                        logger.LogInformation($"Plans Has Seeded  With Count{plans.Count}");
                    }
                    if(dbContext.ChangeTracker.HasChanges())
                    {
                        await dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        logger.LogInformation("Data Has Seeded Already");
                    }
                }
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Failed Seeded Data");
                throw;
            }
        }

        public static List<T> GetSeeddinWithJson<T> (string folderPath , string fileName)
        {
            var filePath = Path.Combine(folderPath, fileName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File Not Found {filePath}");
            }
            var data = File.ReadAllText(filePath);

            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };
            return JsonSerializer.Deserialize<List<T>>(data, options) ?? [];
        }
    }
}
