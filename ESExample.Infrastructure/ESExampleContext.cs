using ESExampleApp.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ESExampleApp.Infrastructure
{
    public class ESExampleContext : DbContext
    {
        public ESExampleContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Person> Person { get; set; }
    }
}
