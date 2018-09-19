using NUnit.Framework;
using Moq;
using System;
using TheWeekendGolfer.Models;
using System.Collections.Generic;
using TheWeekendGolfer.Data;
using FluentAssertions;
using TheWeekendGolfer.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace TheWeekendGolfer.Tests
{
    [TestFixture]
    public class PartnerControllerTest
    {
        private Mock<IPlayerAccessLayer> _mockPlayerAccessLayer;
        private Mock<IPartnerAccessLayer> _mockPartnerAccessLayer;
        private Mock<UserManager<ApplicationUser>> _mockUserManager;
        private PartnerController _sut;
        private DateTime _createdAt;
        private DateTime _modifiedAt;


        [SetUp]
        public void Setup()
        {
            _mockUserManager =  new Mock<UserManager<ApplicationUser>>(
                    new Mock<IUserStore<ApplicationUser>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<ApplicationUser>>().Object,
                    new IUserValidator<ApplicationUser>[0],
                    new IPasswordValidator<ApplicationUser>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<ApplicationUser>>>().Object);
            _mockPlayerAccessLayer = new  Mock<IPlayerAccessLayer>() ;
            _mockPartnerAccessLayer = new Mock<IPartnerAccessLayer>() ;
            _sut = new PartnerController(_mockPartnerAccessLayer.Object, _mockPlayerAccessLayer.Object,_mockUserManager.Object);
            _createdAt = DateTime.Now;
            _modifiedAt = DateTime.Now;
        }



        [TestCase("00000000-0000-0000-0000-000000000001")]
        public async Task TestGetPartners(string playerId)
        {
            _mockPartnerAccessLayer.Setup(x => x.GetPartners(new Guid(playerId))).ReturnsAsync(new List<Player>(){
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
            });

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

            var actual = await _sut.GetPartners(new Guid(playerId)) as ObjectResult;

            actual.StatusCode.Should().Be(200);
            actual.Value.Should().BeEquivalentTo(expected);

        }


        [TestCase("00000000-0000-0000-0000-000000000001")]
        public async Task TestGetPotentialPartners(string playerId)
        {
            _mockPartnerAccessLayer.Setup(x => x.GetPotentialPartners(new Guid(playerId))).ReturnsAsync(new List<Player>(){
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
            });

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

            var actual = await _sut.GetPotentialPartners(new Guid(playerId)) as ObjectResult;

            actual.StatusCode.Should().Be(200);
            actual.Value.Should().BeEquivalentTo(expected);

        }


        [TestCase]
        public async Task TestRemovePartnerAsync()
        {
            var testUser = new ApplicationUser();
            _mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(testUser);

            var testPartner = new Partner()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                PlayerId = new Guid("00000000-0000-0000-0002-000000000000"),
                PartnerId = new Guid("00000000-0000-0003-0000-000000000000"),
            };
            _mockPlayerAccessLayer.Setup(x => x.GetPlayerByUserId(It.IsAny<Guid>())).
                ReturnsAsync(new Player()
                {
                    Id = new Guid("00000000-0000-0000-0002-000000000000"),
                    UserId = new Guid("00000000-0000-0000-0002-000000000000"),
                    FirstName = "Michael",
                    LastName = "Nelmes",
                    Handicap = new Decimal(24.8),
                    Created = _createdAt,
                    Modified = _modifiedAt
                });

            _mockPartnerAccessLayer.Setup(x => x.DeletePartner(testPartner)).
                            ReturnsAsync(true);
            var expected = new Guid("00000000-0000-0000-0000-000000000002");

            var actual = await _sut.RemovePartnerAsync(testPartner) as ObjectResult;

            actual.StatusCode.Should().Be(200);
        }


        [TestCase]
        public async Task TestAddPartnerAsync()
        {
            var testUser = new ApplicationUser();
            _mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(testUser);

            var testPartner = new Partner()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                PlayerId = new Guid("00000000-0000-0000-0002-000000000000"),
                PartnerId = new Guid("00000000-0000-0003-0000-000000000000"),
            };
            _mockPlayerAccessLayer.Setup(x => x.GetPlayerByUserId(It.IsAny<Guid>())).
                ReturnsAsync(new Player()
                {
                    Id = new Guid("00000000-0000-0000-0002-000000000000"),
                    UserId = new Guid("00000000-0000-0000-0002-000000000000"),
                    FirstName = "Michael",
                    LastName = "Nelmes",
                    Handicap = new Decimal(24.8),
                    Created = _createdAt,
                    Modified = _modifiedAt
                });

            _mockPartnerAccessLayer.Setup(x => x.AddPartner(testPartner)).
                            ReturnsAsync(true);
            var expected = new Guid("00000000-0000-0000-0000-000000000002");

            var actual = await _sut.AddPartnerAsync(testPartner) as ObjectResult;

            actual.StatusCode.Should().Be(200);
        }


    }
}
