using System;
using Microsoft.EntityFrameworkCore;
using kforceApp.Models;

namespace kforceApp.Contexts
{
	public class appContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }

        public appContext(DbContextOptions<appContext> options) : base(options) { }
	}
}

