using NUnit.Framework;
using Moq;
using System;
using TheWeekendGolfer.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TheWeekendGolfer.Web.Data;
using TheWeekendGolfer.Data;
using FluentAssertions;
using System.Threading.Tasks;

namespace TheWeekendGolfer.Tests
{
    [TestFixture]
    public class HandicapAccessLayerTest
    {
        private GolfDbContext _context;
        private HandicapAccessLayer _sut;
        private DateTime _createdAt;


        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<GolfDbContext>()
                  .UseInMemoryDatabase(Guid.NewGuid().ToString())
                  .Options;
            _context = new GolfDbContext(options);
            _createdAt = DateTime.Now;
            var Handicaps = new List<Handicap>()
            {
                new Handicap(){
                    Id = new Guid("00000000-0000-0000-0000-000000000001"),
                    PlayerId = new Guid("00000000-0000-0000-0001-000000000000"),
                    CurrentHandicap = 23,
                    Value=45,
                    Date = _createdAt
                },
                new Handicap(){
                    Id = new Guid("00000000-0000-0000-0000-000000000002"),
                    PlayerId = new Guid("00000000-0000-0000-0002-000000000000"),
                    CurrentHandicap = 18,
                    Value=19,
                    Date = _createdAt
                },
                new Handicap(){
                    Id = new Guid("00000000-0000-0000-0000-000000000003"),
                    PlayerId = new Guid("00000000-0000-0000-0001-000000000000"),
                    CurrentHandicap = 20,
                    Value=15,
                    Date = _createdAt.AddDays(1)
                }
            }.AsQueryable();

            _context.Handicaps.AddRange(Handicaps);
            _context.SaveChanges();
            _sut = new HandicapAccessLayer(_context);
        }
        
        [TestCase("00000000-0000-0000-0001-000000000000")]
        public void TestGetLatestHandicap(string id)
        {
            var expected = new Handicap()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000003"),
                PlayerId = new Guid("00000000-0000-0000-0001-000000000000"),
                CurrentHandicap = 20,
                Value = 15,
                Date = _createdAt.AddDays(1)
            };

            var actual = _sut.GetLatestHandicap(new Guid(id));

            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("00000000-0000-0000-0001-000000000000")]
        public void TestGetOrderedHandicaps(string id)
        {
            var expected = new List<Handicap>(){
            new Handicap()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000003"),
                PlayerId = new Guid("00000000-0000-0000-0001-000000000000"),
                CurrentHandicap = 20,
                Value = 15,
                Date = _createdAt.AddDays(1)
            },
            new Handicap()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                PlayerId = new Guid("00000000-0000-0000-0001-000000000000"),
                CurrentHandicap = 23,
                Value = 45,
                Date = _createdAt
            }
            };

            var actual = _sut.GetOrderedHandicaps(new Guid(id));

            actual.Should().BeEquivalentTo(expected,options=>options.WithStrictOrdering());
        }



        [TestCase]
        public async Task TestAddHandicap()
        {
            var expected = _context.Handicaps.Count() + 1;

            await _sut.AddHandicap(
                new Handicap()
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000004"),
                    PlayerId = new Guid("00000000-0000-0000-0001-000000000000"),
                    CurrentHandicap = 17,
                    Value = 14,
                    Date = _createdAt.AddDays(2)
                }
            );

            var actual = _context.Handicaps;

            actual.Should().HaveCount(expected);
        }



    }
}
