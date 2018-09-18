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
using TheWeekendGolfer.Web.Controllers;

namespace TheWeekendGolfer.Tests
{
    [TestFixture]
    public class PlayerControllerTest
    {

        private Mock<ICourseAccessLayer> _mockCourseAccessLayer;
        private Mock<IHandicapAccessLayer> _mockHandicapAccessLayer;
        private Mock<IPlayerAccessLayer> _mockPlayerAccessLayer;
        private Mock<IScoreAccessLayer> _mockScoreAccessLayer;
        private Mock<IGolfRoundAccessLayer> _mockGolfRoundAccessLayer;
        private PlayerController _sut;
        private DateTime _createdAt;
        private DateTime _modifiedAt;


        [SetUp]
        public void Setup()
        {
            _mockCourseAccessLayer = new Mock<ICourseAccessLayer>();
            _mockHandicapAccessLayer = new Mock<IHandicapAccessLayer>();
            _mockPlayerAccessLayer = new  Mock<IPlayerAccessLayer>() ;
            _mockScoreAccessLayer = new Mock<IScoreAccessLayer>() ;
            _mockGolfRoundAccessLayer = new Mock<IGolfRoundAccessLayer>() ;
            _sut = new PlayerController(_mockHandicapAccessLayer.Object,_mockPlayerAccessLayer.Object,_mockCourseAccessLayer.Object,
                                        _mockScoreAccessLayer.Object,_mockGolfRoundAccessLayer.Object);
            _createdAt = DateTime.Now;
            _modifiedAt = DateTime.Now;
        }



        [TestCase]
        public async Task TestIndex()
        {
            _mockPlayerAccessLayer.Setup(x => x.GetAllPlayers()).ReturnsAsync(new List<Player>(){
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

            var actual = await _sut.Index() as ObjectResult;

            actual.StatusCode.Should().Be(200);
            actual.Value.Should().BeEquivalentTo(expected);

        }

        [TestCase("00000000-0000-0000-0000-000000000001")]
        public async Task TestGetOrderedHandicaps(string playerId)
        {
            _mockHandicapAccessLayer.Setup(x => x.GetOrderedHandicaps(new Guid(playerId))).ReturnsAsync(new List<Handicap>(){
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
            });

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

            var actual = await _sut.GetOrderedHandicaps(new Guid(playerId)) as ObjectResult;

            actual.StatusCode.Should().Be(200);
            actual.Value.Should().BeEquivalentTo(expected);
        }


        [TestCase]
        public async Task TestCreateNoHandicap()
        {
            var testPlayer = new Player()
            {
                UserId = new Guid("00000000-0000-0000-0002-000000000000"),
                FirstName = "Michael",
                LastName = "Nelmes",
                Handicap = null,
                Created = _createdAt,
                Modified = _modifiedAt
            };
            _mockPlayerAccessLayer.Setup(x => x.AddPlayer(testPlayer)).ReturnsAsync(new Guid("00000000-0000-0000-0000-000000000002"));
            

            var expected = new Guid("00000000-0000-0000-0000-000000000002");

            var actual = await _sut.Create(testPlayer) as ObjectResult;

            actual.StatusCode.Should().Be(200);
            actual.Value.Should().BeEquivalentTo(expected);
        }

        [TestCase]
        public async Task TestCreateWithHandicap()
        {
            var testPlayer = new Player()
            {
                UserId = new Guid("00000000-0000-0000-0002-000000000000"),
                FirstName = "Michael",
                LastName = "Nelmes",
                Handicap = new Decimal(23.4),
                Created = _createdAt,
                Modified = _modifiedAt
            };

            var testHandicap = new Handicap()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                PlayerId = new Guid("00000000-0000-0000-0000-000000000001"),
                CurrentHandicap = new Decimal(23.4),
                Value = new Decimal(23.4),
                Date = _createdAt.AddDays(2)
            };

            _mockHandicapAccessLayer.Setup(x => x.AddHandicap(It.IsAny<Handicap>())).ReturnsAsync(true);
            _mockPlayerAccessLayer.Setup(x => x.AddPlayer(testPlayer)).ReturnsAsync(new Guid("00000000-0000-0000-0000-000000000002"));


            var expected = new Guid("00000000-0000-0000-0000-000000000002");

            var actual = await _sut.Create(testPlayer) as ObjectResult;

            actual.StatusCode.Should().Be(200);
            actual.Value.Should().BeEquivalentTo(expected);
        }

        [TestCase("00000000-0000-0000-0000-000000000001")]
        public async Task TestAllPlayerRoundCourses(string playerId)
        {
            _mockScoreAccessLayer.Setup(x => x.GetAllPlayerScores(new Guid(playerId))).ReturnsAsync(new List<Score>()
            {
                new Score(){
                    Id = new Guid("00000000-0000-0000-0000-000000000001"),
                    PlayerId = new Guid("00000000-0000-0000-0001-000000000000"),
                    Value=45,
                    GolfRoundId = new Guid("00000000-0000-0001-0000-000000000000"),
                    Created = _createdAt,
                },
                new Score(){
                    Id = new Guid("00000000-0000-0000-0000-000000000002"),
                    PlayerId = new Guid("00000000-0000-0000-0001-000000000000"),
                    Value=63,
                    GolfRoundId = new Guid("00000000-0000-0002-0000-000000000000"),
                    Created = _createdAt,
                } });
            _mockGolfRoundAccessLayer.Setup(x => x.GetAllGolfRoundCourseIds(new List<Guid>() {
                new Guid("00000000-0000-0001-0000-000000000000"),
                new Guid("00000000-0000-0002-0000-000000000000")
            })).ReturnsAsync(
                new List<Guid>() {
                new Guid("00000000-0001-0000-0000-000000000000"),
                new Guid("00000000-0001-0000-0000-000000000000")
            });
            _mockCourseAccessLayer.Setup(x => x.GetCourseStats(new List<Guid>() {
                new Guid("00000000-0001-0000-0000-000000000000"),
                new Guid("00000000-0001-0000-0000-000000000000")
            })).ReturnsAsync(new Dictionary<string,int>(){
                {"Point Walter",2 }
            });
            var expected = new Dictionary<string, int>(){
                {"Point Walter",2 }
            };


            var actual = await _sut.GetAllPlayerRoundCourses(new Guid(playerId)) as ObjectResult;

            actual.StatusCode.Should().Be(200);
            actual.Value.Should().BeEquivalentTo(expected);
        }

        [TestCase("00000000-0000-0000-0000-000000000001")]
        public async Task TestDetails(string id)
        {
            _mockPlayerAccessLayer.Setup(x => x.GetPlayer(new Guid(id))).ReturnsAsync(new Player()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                UserId = new Guid("00000000-0000-0000-0002-000000000000"),
                FirstName = "Michael",
                LastName = "Nelmes",
                Handicap = new Decimal(23.4),
                Created = _createdAt,
                Modified = _modifiedAt
            });

            var expected = new Player()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                UserId = new Guid("00000000-0000-0000-0002-000000000000"),
                FirstName = "Michael",
                LastName = "Nelmes",
                Handicap = new Decimal(23.4),
                Created = _createdAt,
                Modified = _modifiedAt
            };
            var actual = await _sut.Details(new Guid(id)) as ObjectResult;

            actual.StatusCode.Should().Be(200);
            actual.Value.Should().BeEquivalentTo(expected);
        }

    }
}
