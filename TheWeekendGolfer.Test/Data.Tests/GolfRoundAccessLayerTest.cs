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
    public class GolfRoundAccessLayerTest
    {
        private GolfDbContext _context;
        private GolfRoundAccessLayer _sut;
        private DateTime _createdAt;


        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<GolfDbContext>()
                  .UseInMemoryDatabase(Guid.NewGuid().ToString())
                  .Options;
            _context = new GolfDbContext(options);
            _createdAt = DateTime.Now;
            var golfRounds = new List<GolfRound>()
            {
                new GolfRound(){
                    Id = new Guid("00000000-0000-0000-0000-000000000001"),
                    Date = _createdAt,
                    CourseId = new Guid("00000000-0000-0000-0001-000000000000"),
                    Created =_createdAt,
  
                },
                new GolfRound(){
                    Id = new Guid("00000000-0000-0000-0000-000000000002"),
                    Date = _createdAt,
                    CourseId = new Guid("00000000-0000-0000-0002-000000000000"),
                    Created =_createdAt,

                },
                new GolfRound(){
                    Id = new Guid("00000000-0000-0000-0000-000000000003"),
                    Date =_createdAt,
                    CourseId = new Guid("00000000-0000-0000-0001-000000000000"),
                    Created =_createdAt,

                },
                new GolfRound(){
                    Id = new Guid("00000000-0000-0000-0000-000000000004"),
                    Date = _createdAt,
                    CourseId = new Guid("00000000-0000-0000-0003-000000000000"),
                    Created =_createdAt,

                },
            }.AsQueryable();
            _context.GolfRounds.AddRange(golfRounds);
            _context.SaveChanges();
            _sut = new GolfRoundAccessLayer(_context);
        }

        [TestCase("00000000-0000-0000-0000-000000000001")]
        [TestCase("00000000-0000-0000-0000-000000000002")]
        public async Task TestGetGolfRound(string id)
        {
            var actual = await _sut.GetGolfRound(new Guid(id));

            actual.Should().BeOfType<GolfRound>();
        }

        [TestCase("00000000-0000-0000-0000-000000000005")]
        public async Task TestGetGolfRoundException(string id)
        {
            Func<Task> action = async() => await _sut.GetGolfRound(new Guid(id));

            action.Should().Throw<Exception>();
        }

        [TestCase]
        public async Task TestGetAllGolfRoundCourseIds()
        {
            var courseIds = new List<Guid>(){
                  new Guid("00000000-0000-0000-0000-000000000001"),
                  new Guid("00000000-0000-0000-0000-000000000002"),
                  new Guid("00000000-0000-0000-0000-000000000003")
            };

            var expected = new List<Guid>() {
                                 new Guid("00000000-0000-0000-0001-000000000000"),
                  new Guid("00000000-0000-0000-0002-000000000000"),
                                   new Guid("00000000-0000-0000-0001-000000000000")
            };

            var actual = await _sut.GetAllGolfRoundCourseIds(courseIds);

            actual.Should().BeEquivalentTo(expected);

        }

        [TestCase]
        public async Task TestGetAllGolfRounds()
        {
            var expected = new List<GolfRound>() {
                new GolfRound(){
                    Id = new Guid("00000000-0000-0000-0000-000000000001"),
                    Date = _createdAt,
                    CourseId = new Guid("00000000-0000-0000-0001-000000000000"),
                    Created =_createdAt,

                },
                new GolfRound(){
                    Id = new Guid("00000000-0000-0000-0000-000000000002"),
                    Date = _createdAt,
                    CourseId = new Guid("00000000-0000-0000-0002-000000000000"),
                    Created =_createdAt,

                },
                new GolfRound(){
                    Id = new Guid("00000000-0000-0000-0000-000000000003"),
                    Date = _createdAt,
                    CourseId = new Guid("00000000-0000-0000-0001-000000000000"),
                    Created =_createdAt,

                },
                new GolfRound(){
                    Id = new Guid("00000000-0000-0000-0000-000000000004"),
                    Date = _createdAt,
                    CourseId = new Guid("00000000-0000-0000-0003-000000000000"),
                    Created =_createdAt,

                }
            };

            var actual = await _sut.GetAllGolfRounds();

            actual.Should().BeEquivalentTo(expected);

        }

        [TestCase]
        public async Task TestAddGolfRound()
        {
            var expected = _context.GolfRounds.Count() + 1;

            await _sut.AddGolfRound(new GolfRound()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000006"),
                Date = DateTime.Now,
                CourseId = new Guid("00000000-0000-0000-0003-000000000000"),
                Created = _createdAt,
            });

            var actual = _context.GolfRounds;

            actual.Should().HaveCount(expected);
        }


    }
}
