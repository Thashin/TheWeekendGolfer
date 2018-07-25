using CsvHelper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TheWeekendGolfer.Web.Models;

namespace TheWeekendGolfer.Web.Data
{
    public static class GolfDbExtensions
    {
        /*TODO:
         *Configure field to use appsettings.json
         * Add handling for null data
         * Ignoring missing id column in CSV
        */ 
        public static void EnsureSeedDataForContext(this GolfDbContext context)
        {
            if(context.Courses.Any())
            {
                return;
            }

            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "TheWeekendGolfer.Web.Domain.SeedData.SlopeRatings.csv";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    CsvReader csvReader = new CsvReader(reader);
                    csvReader.Configuration.HeaderValidated = null;
                    csvReader.Configuration.MissingFieldFound = null;
                    IEnumerable<Course> courses = csvReader.GetRecords<Course>();
                    context.Courses.AddRange(courses);
                    context.SaveChanges();
                }
            }

        }

    }
}
