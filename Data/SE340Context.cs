using Microsoft.EntityFrameworkCore;
using SE340.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE340.Data
{
    public class SE340Context : DbContext
    {
        public SE340Context(DbContextOptions<SE340Context> options)
            : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; }
    }
}
