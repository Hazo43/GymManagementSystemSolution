﻿using GymManagementDAL.Data.Context;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.DataSeed
{
    public static class GymDbContextSeeding
    {

        public static bool SeedData(GymDbContext dbContext)
        {
            try
            {
                var HasPlans = dbContext.Plans.Any();
                var HasCategory = dbContext.Categories.Any();
                if (HasPlans && HasCategory)  return true; 
                
                if(!HasPlans ) // لو مفهاش داتا روح ضيف اللي تحتها 
                {
                    var Plans = LoadDataFromJsonFile<Plan>("plans.json");
                    if(Plans.Any())
                        dbContext.Plans.AddRange(Plans);
                }
                if(!HasCategory )
                {
                    var Category = LoadDataFromJsonFile<Category>("categories.json");
                    if (Category.Any())
                        dbContext.Categories.AddRange(Category);
                }
                return dbContext.SaveChanges() > 0;
            }
            catch (Exception ex) 
            {
                Console.WriteLine($" Seeding Failed : {ex}");
                return false;
            }
        }

        private static List<T> LoadDataFromJsonFile<T>(string fileName)
        {
            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", fileName);

            if (!File.Exists(FilePath)) throw new FileNotFoundException();

            string Data = File.ReadAllText(FilePath);

            var Options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,  // => هيعديها Capitel or Small يعني لو الاختلاف في حرف 
            };

            return JsonSerializer.Deserialize<List<T>>( Data , Options ) ?? new List<T>();
        }

    }
}
