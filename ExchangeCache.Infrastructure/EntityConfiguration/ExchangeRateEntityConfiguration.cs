using ExchangeCache.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeCache.Infrastructure.EntityConfiguration
{
    class ExchangeRateEntityConfiguration : IEntityTypeConfiguration<ExchangeRate>
    {
        public void Configure(EntityTypeBuilder<ExchangeRate> builder)
        {
            builder.ToTable("ExchangeRates");
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id).ValueGeneratedOnAdd();

            builder.Property(r => r.SourceCurrency).IsRequired().HasMaxLength(3);
            builder.Property(r => r.TargetCurrency).IsRequired().HasMaxLength(3);
            builder.Property(r => r.Rate).IsRequired().HasColumnType("decimal(18, 12)");
            builder.Property(r => r.ReceivedAt).IsRequired();

            builder.HasIndex(r => new { r.SourceCurrency, r.ReceivedAt });
        }
    }
}
