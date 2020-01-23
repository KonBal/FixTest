using ExchangeCache.Domain.Models;
using ExchangeCache.Infrastructure;
using ExchangeCache.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExchangeCache.Test
{
    [TestClass]
    public class ExchangeRateRepositoryTest
    {
        [TestMethod]
        public void Add_rate_to_database()
        {
            var options = new DbContextOptionsBuilder<ExchangeRateContext>()
                .UseInMemoryDatabase(databaseName: "Add_rate_to_database")
                .Options;

            var time = DateTime.Now;
            using (var context = new ExchangeRateContext(options))
            {
                var repo = new ExchangeRateRepository(context);
                repo.Add(new ExchangeRate
                {
                    Rate = 10,
                    ReceivedAt = time,
                    SourceCurrency = "USD",
                    TargetCurrency = "RUB"
                });
                context.SaveChanges();
            }

            using (var context = new ExchangeRateContext(options))
            {
                Assert.AreEqual(1, context.ExchangeRates.Count());
                Assert.AreEqual(time, context.ExchangeRates.Single().ReceivedAt);
            }
        }

        [TestMethod]
        public void Add_rate_range_to_database()
        {
            var options = new DbContextOptionsBuilder<ExchangeRateContext>()
                .UseInMemoryDatabase(databaseName: "Add_rate_range_to_database")
                .Options;

            var time = DateTime.Now;
            using (var context = new ExchangeRateContext(options))
            {
                var repo = new ExchangeRateRepository(context);
                repo.AddRange(new List<ExchangeRate>{new ExchangeRate
                {
                    Rate = 10,
                    ReceivedAt = time,
                    SourceCurrency = "USD",
                    TargetCurrency = "RUB"
                },
                new ExchangeRate
                {
                    Rate = 101,
                    ReceivedAt = time,
                    SourceCurrency = "USD",
                    TargetCurrency = "RUB"
                } });
                context.SaveChanges();
            }

            using (var context = new ExchangeRateContext(options))
            {
                Assert.AreEqual(2, context.ExchangeRates.Count());
            }
        }

        [TestMethod]
        public void Get_latest_from_database()
        {
            var options = new DbContextOptionsBuilder<ExchangeRateContext>()
                .UseInMemoryDatabase(databaseName: "Get_latest_from_database")
                .Options;

            var time = DateTime.Now;
            using (var context = new ExchangeRateContext(options))
            {

                context.ExchangeRates.AddRange(new List<ExchangeRate>
                {
                new ExchangeRate
                {
                    Rate = 10,
                    ReceivedAt = time,
                    SourceCurrency = "USD",
                    TargetCurrency = "RUB"
                },
                new ExchangeRate{
                    Rate = 110,
                    ReceivedAt = time,
                    SourceCurrency = "USD",
                    TargetCurrency = "EUR"
                },
                new ExchangeRate
                {
                    Rate = 101,
                    ReceivedAt = time + TimeSpan.FromMinutes(3),
                    SourceCurrency = "USD",
                    TargetCurrency = "RUB"
                }
                });
                context.SaveChanges();
            }

            using (var context = new ExchangeRateContext(options))
            {
                var repo = new ExchangeRateRepository(context);
                var result = repo.GetLatestAsync("USD").Result;
                Assert.AreEqual(2, result.Count);
            }
        }

        [TestMethod]
        public void Get_latest_for_pair_from_database()
        {
            var options = new DbContextOptionsBuilder<ExchangeRateContext>()
                .UseInMemoryDatabase(databaseName: "Get_latest_for_pair_from_database")
                .Options;

            var time = DateTime.Now;
            using (var context = new ExchangeRateContext(options))
            {

                context.ExchangeRates.AddRange(new List<ExchangeRate>
                {
                new ExchangeRate
                {
                    Rate = 10,
                    ReceivedAt = time,
                    SourceCurrency = "USD",
                    TargetCurrency = "RUB"
                },
                new ExchangeRate{
                    Rate = 110,
                    ReceivedAt = time,
                    SourceCurrency = "USD",
                    TargetCurrency = "EUR"
                },
                new ExchangeRate
                {
                    Rate = 101,
                    ReceivedAt = time + TimeSpan.FromMinutes(3),
                    SourceCurrency = "USD",
                    TargetCurrency = "RUB"
                }
                });
                context.SaveChanges();
            }

            using (var context = new ExchangeRateContext(options))
            {
                var repo = new ExchangeRateRepository(context);
                var result = repo.GetLatestForPairAsync("USD", "RUB").Result;
                Assert.AreEqual(101, result.Rate);
            }
        }

    }
}
