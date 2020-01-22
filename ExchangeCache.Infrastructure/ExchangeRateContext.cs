using ExchangeCache.Domain.SeedWork;
using ExchangeCache.Domain.Models;
using ExchangeCache.Infrastructure.EntityConfiguration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeCache.Infrastructure
{
    public class ExchangeRateContext : DbContext, IUnitOfWork
    {
        public ExchangeRateContext(DbContextOptions<ExchangeRateContext> options) : base(options) { }

        public DbSet<ExchangeRate> ExchangeRates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ExchangeRateEntityConfiguration());
        }
    }
}
