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
    public class PartnerAccessLayerTest
    {
        private GolfDbContext _context;
        private PartnerAccessLayer _sut;
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
                },
                new Player()
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000003"),
                    UserId = new Guid("00000000-0000-0000-0003-000000000000"),
                    FirstName = "Adam",
                    LastName = "Van Halsdingen",
                    Handicap = new Decimal(44.8),
                    Created = _createdAt,
                    Modified = _modifiedAt
                },
                new Player()
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000004"),
                    UserId = new Guid("00000000-0000-0000-0004-000000000000"),
                    FirstName = "Jake",
                    LastName = "Hannell",
                    Handicap = null,
                    Created = _createdAt,
                    Modified = _modifiedAt
                }
            }.AsQueryable();

            var partners = new List<Partner>()
            {
                new Partner(){
                    Id = new Guid("00000000-0000-0000-0000-000000000001"),
                    PlayerId= new Guid("00000000-0000-0000-0000-000000000001"),
                    PartnerId= new Guid("00000000-0000-0000-0000-000000000002")
                },
                new Partner(){
                    Id = new Guid("00000000-0000-0000-0000-000000000002"),
                    PlayerId= new Guid("00000000-0000-0000-0000-000000000002"),
                    PartnerId= new Guid("00000000-0000-0000-0000-000000000001")
                },
                new Partner(){
                    Id = new Guid("00000000-0000-0000-0000-000000000003"),
                    PlayerId= new Guid("00000000-0000-0000-0000-000000000002"),
                    PartnerId= new Guid("00000000-0000-0000-0000-000000000003")
                },
                new Partner(){
                    Id = new Guid("00000000-0000-0000-0000-000000000004"),
                    PlayerId= new Guid("00000000-0000-0000-0000-000000000002"),
                    PartnerId= new Guid("00000000-0000-0000-0000-000000000004")
                },
                new Partner(){
                    Id = new Guid("00000000-0000-0000-0000-000000000005"),
                    PlayerId= new Guid("00000000-0000-0000-0000-000000000003"),
                    PartnerId= new Guid("00000000-0000-0000-0000-000000000001")
                }
            }.AsQueryable();

            _context.Players.AddRange(players);
            _context.Partners.AddRange(partners);
            _context.SaveChanges();
            _sut = new PartnerAccessLayer(_context);
        }

        [TestCase("00000000-0000-0000-0000-000000000002")]
        public async Task TestGetPartners(string id)
        {
            var actual = await _sut.GetPartners(new Guid(id));

            actual.Should().BeOfType<List<Player>>().And.HaveCount(3);
        }

        [TestCase("00000000-0000-0000-0000-000000000005")]
        public async Task TestGetPartnersNonPlayer(string id)
        {
            var actual = await _sut.GetPartners(new Guid(id));

            actual.Should().BeOfType<List<Player>>().And.HaveCount(0);
        }

        [TestCase("00000000-0000-0000-0000-000000000001")]
        public async Task TestGetAllPotentialPartners(string playerId)
        {
            var actual = await _sut.GetPotentialPartners(new Guid(playerId));

            actual.Should().BeOfType<List<Player>>().And.HaveCount(2);

        }

        [TestCase("00000000-0000-0000-0000-000000000005")]
        public async Task TestGetPotentialPartnersNonPlayer(string id)
        {
            var actual = await _sut.GetPotentialPartners(new Guid(id));

            actual.Should().BeOfType<List<Player>>().And.HaveCount(0);
        }


        [TestCase]
        public async Task TestAddPartner()
        {
            var expected = _context.Partners.Count() + 1;

            await _sut.AddPartner(new Partner()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000006"),
                PlayerId = new Guid("00000000-0000-0000-0000-000000000003"),
                PartnerId = new Guid("00000000-0000-0000-0000-000000000002")
            });

            var actual = _context.Partners;

            actual.Should().HaveCount(expected);
        }


    }
}
