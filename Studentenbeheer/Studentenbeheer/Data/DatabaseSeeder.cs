using Microsoft.EntityFrameworkCore;
using Studentenbeheer.Models;

namespace Studentenbeheer.Data
{
    public class DatabaseSeeder
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new StudentenbeheerContext(
                serviceProvider.GetRequiredService<DbContextOptions<StudentenbeheerContext>>()))
            {
                context.Database.EnsureCreated();

                if (context.Gender.Any() || context.Student.Any())
                {
                    return;
                }

                context.Gender.AddRange(

                    new Gender
                    {
                        ID = 'M',
                        Name = "Male"
                    },

                    new Gender
                    {
                        ID = 'F',
                        Name = "Female"
                    },

                    new Gender
                    {
                        ID = '-',
                        Name = "None"
                    }

                );
                context.SaveChanges();

                context.Student.AddRange(

                    new Student
                    {
                        Voornaam = "Ine",
                        Achternaam = "DeBast",
                        Geboortedatum = DateTime.Now,
                        GeslachtId = 'F'
                    },

                    new Student
                    {
                        Voornaam = "Antoine",
                        Achternaam = "Couck",
                        Geboortedatum = DateTime.Now,
                        GeslachtId = 'M'
                    },

                    new Student
                    {
                        Voornaam = "Melvin",
                        Achternaam = "Angeli",
                        Geboortedatum = DateTime.Now,
                        GeslachtId = '-'
                    }

                );
                context.SaveChanges();
            }
        }
    }
}
