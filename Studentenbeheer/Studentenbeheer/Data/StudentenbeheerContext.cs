using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Studentenbeheer.Models;

namespace Studentenbeheer.Data
{
    public class StudentenbeheerContext : DbContext
    {
        public StudentenbeheerContext (DbContextOptions<StudentenbeheerContext> options)
            : base(options)
        {
        }

        public DbSet<Studentenbeheer.Models.Student> Student { get; set; }
    }
}
