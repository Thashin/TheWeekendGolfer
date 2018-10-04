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
using TheWeekendGolfer.Data.ApplicationUserDB;

namespace TheWeekendGolfer.Tests
{
    [TestFixture]
    public class PlayerAccessLayerTest
    {
        private GolfDbContext _context;
        private PlayerAccessLayer _sut;
        private DateTime _createdAt;
        private DateTime _modifiedAt;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<GolfDbContext>()
                  .UseInMemoryDatabase(Guid.NewGuid().ToString())
                  .Options;
            _context = new GolfDbContext(options);
            _createdAt = DateTime.Now;
            _modifiedAt = DateTime.Now;
            var players = new List<Player>()
            {
                new Player(){
                    Id = new Guid("00000000-0000-0000-0000-000000000001"),
                    UserId = new Guid("00000000-0000-0000-0001-000000000000"),
                    FirstName = "Thashin",
                    LastName = "Naidoo",
                    Handicap = new Decimal(19.3),
                    Created = _createdAt,
                    Modified = _modifiedAt
                },
                new Player(){
                    Id = new Guid("00000000-0000-0000-0000-000000000002"),
                    UserId = new Guid("00000000-0000-0000-0002-000000000000"),
                    FirstName = "Michael",
                    LastName = "Nelmes",
                    Handicap = new Decimal(24.8),
                    Created = _createdAt,
                    Modified = _modifiedAt
                }
            }.AsQueryable();

            _context.Players.AddRange(players);
            _context.SaveChanges();
            var userOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString())
                         .Options;
            var userContext = new ApplicationDbContext(userOptions);
            userContext.Add(new ApplicationUser() { Id = "00000000-0000-0000-0003-000000000000" });
            userContext.SaveChanges();
            _sut = new PlayerAccessLayer(_context,userContext);
        }

        [TestCase("00000000-0000-0000-0000-000000000001")]
        [TestCase("00000000-0000-0000-0000-000000000002")]
        public void TestGetPlayer(string id)
        {
            var actual =  _sut.GetPlayer(new Guid(id));

            actual.Should().BeOfType<Player>();
        }

        [TestCase("00000000-0000-0000-0000-000000000005")]
        public void TestGetPlayerException(string id)
        {
            Action action = () => _sut.GetPlayer(new Guid(id));

            action.Should().Throw<Exception>();
        }

        [TestCase("00000000-0000-0000-0000-000000000001")]
        [TestCase("00000000-0000-0000-0000-000000000002")]
        public void TestGetPlayerByUserId(string id)
        {
            var actual = _sut.GetPlayer(new Guid(id));

            actual.Should().BeOfType<Player>();
        }

        [TestCase("00000000-0000-0000-0000-000000000005")]
        public void TestGetPlayerByUserIdException(string id)
        {
            Action action = () => _sut.GetPlayer(new Guid(id));

            action.Should().Throw<Exception>();
        }

        [TestCase]
        public void TestGetAllPlayers()
        {
            var expected = new List<Player>(){
                new Player(){
                    Id = new Guid("00000000-0000-0000-0000-000000000001"),
                    UserId = new Guid("00000000-0000-0000-0001-000000000000"),
                    FirstName = "Thashin",
                    LastName = "Naidoo",
                    Handicap = new Decimal(19.3),
                    Created = _createdAt,
                    Modified = _modifiedAt
                },
                new Player(){
                    Id = new Guid("00000000-0000-0000-0000-000000000002"),
                    UserId = new Guid("00000000-0000-0000-0002-000000000000"),
                    FirstName = "Michael",
                    LastName = "Nelmes",
                    Handicap = new Decimal(24.8),
                    Created = _createdAt,
                    Modified = _modifiedAt
                }
            };

            var actual = _sut.GetAllPlayers();

            actual.Should().BeEquivalentTo(expected);

        }

        [TestCase]
        public async Task TestAddPlayer()
        {
            var expected = _context.Players.Count() + 1;

            await _sut.AddPlayer(new Player()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000003"),
                UserId = new Guid("00000000-0000-0000-0003-000000000000"),
                FirstName = "Adam",
                LastName = "Van Halsdingen",
                Handicap = new Decimal(44.8),
                Created = _createdAt,
                Modified = _modifiedAt
            });

            var actual = _context.Players;

            actual.Should().HaveCount(expected);
        }

        [TestCase]
        public async Task TestAddPlayerNoHandicap()
        {
            var expected = _context.Players.Count() + 1;

            await _sut.AddPlayer(new Player()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000003"),
                UserId = new Guid("00000000-0000-0000-0003-000000000000"),
                FirstName = "Jake",
                LastName = "Hannell",
                Handicap = null,
                Created = _createdAt,
                Modified = _modifiedAt
            });

            var actual =  _context.Players;

            actual.Should().HaveCount(expected);
        }


    }
}
