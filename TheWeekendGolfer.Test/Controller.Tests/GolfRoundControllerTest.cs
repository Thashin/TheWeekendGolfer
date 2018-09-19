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
using FluentAssertions.Extensions;

namespace TheWeekendGolfer.Tests
{
    [TestFixture]
    public class GolfRoundControllerTest
    {

        private Mock<ICourseAccessLayer> _mockCourseAccessLayer;
        private Mock<IHandicapAccessLayer> _mockHandicapAccessLayer;
        private Mock<IPlayerAccessLayer> _mockPlayerAccessLayer;
        private Mock<IScoreAccessLayer> _mockScoreAccessLayer;
        private Mock<IGolfRoundAccessLayer> _mockGolfRoundAccessLayer;
        private GolfRoundController _sut;
        private DateTime _createdAt;
        private DateTime _modifiedAt;


        [SetUp]
        public void Setup()
        {
            _mockCourseAccessLayer = new Mock<ICourseAccessLayer>();
            _mockHandicapAccessLayer = new Mock<IHandicapAccessLayer>();
            _mockPlayerAccessLayer = new Mock<IPlayerAccessLayer>();
            _mockScoreAccessLayer = new Mock<IScoreAccessLayer>();
            _mockGolfRoundAccessLayer = new Mock<IGolfRoundAccessLayer>();
            _sut = new GolfRoundController(_mockGolfRoundAccessLayer.Object, _mockScoreAccessLayer.Object,
                _mockPlayerAccessLayer.Object, _mockCourseAccessLayer.Object, _mockHandicapAccessLayer.Object);
            _createdAt = DateTime.Now;
            _modifiedAt = DateTime.Now;
        }



        [TestCase]
        public async Task TestIndex()
        {
            _mockPlayerAccessLayer.Setup(x => x.GetAllPlayers()).ReturnsAsync(new List<Player>(){
                new Player(){
                    Id = new Guid("00000000-0000-0000-0001-000000000000"),
                    UserId = new Guid("00000000-0000-0000-0001-000000000000"),
                    FirstName = "Thashin",
                    LastName = "Naidoo",
                    Handicap = new Decimal(19.3),
                },
                new Player(){
                    Id = new Guid("00000000-0000-0000-0002-000000000000"),
                    UserId = new Guid("00000000-0000-0000-0002-000000000000"),
                    FirstName = "Michael",
                    LastName = "Nelmes",
                    Handicap = new Decimal(24.8),
                }
            });

            _mockCourseAccessLayer.Setup(x => x.GetAllCourses()).ReturnsAsync(new List<Course>(){
                new Course(){
                    Id = new Guid("00000000-0000-0000-0001-000000000000"),
                    Name = "Wembley Golf Course",
                    Holes = "18",
                    Location = "Western Australia",
                    Par = 72,
                    ScratchRating = 70,
                    Slope = 120,
                    TeeName = "Blue Men"
                },
                new Course(){
                    Id = new Guid("00000000-0000-0000-0002-000000000000"),
                    Name = "Point Walter",
                    Holes = "1-9",
                    Location = "Western Australia",
                    Par = 35,
                    ScratchRating = 34,
                    Slope = 115,
                    TeeName = "Blue Men"
                },
                new Course(){
                    Id = new Guid("00000000-0000-0000-0000-000000000003"),
                    Name = "Point Walter",
                    Holes = "18",
                    Location = "Western Australia",
                    Par = 35,
                    ScratchRating = 34,
                    Slope = 115,
                    TeeName = "Blue Men"
                },
                new Course(){
                    Id = new Guid("00000000-0000-0000-0000-000000000004"),
                    Name = "Point Walter",
                    Holes = "1-9",
                    Location = "Western Australia",
                    Par = 35,
                    ScratchRating = 34,
                    Slope = 117,
                    TeeName = "Red Women"
                }
            });

            _mockScoreAccessLayer.Setup(x => x.GetAllScores()).ReturnsAsync(new List<Score>()
            {
                new Score(){
                    Id = new Guid("00000000-0000-0000-0000-000000000001"),
                    PlayerId = new Guid("00000000-0000-0000-0001-000000000000"),
                    Value=45,
                    GolfRoundId = new Guid("00000000-0000-0000-0000-000000000001"),
                },
                new Score(){
                    Id = new Guid("00000000-0000-0000-0000-000000000002"),
                    PlayerId = new Guid("00000000-0000-0000-0002-000000000000"),
                    Value=63,
                    GolfRoundId = new Guid("00000000-0000-0000-0000-000000000002"),
                },
                new Score(){
                    Id = new Guid("00000000-0000-0000-0000-000000000003"),
                    PlayerId = new Guid("00000000-0000-0000-0001-000000000000"),
                    Value=40,
                    GolfRoundId = new Guid("00000000-0000-0000-0000-000000000003"),
                },
                new Score(){
                    Id = new Guid("00000000-0000-0000-0000-000000000004"),
                    PlayerId = new Guid("00000000-0000-0000-0002-000000000000"),
                    Value=57,
                    GolfRoundId = new Guid("00000000-0000-0000-0000-000000000003"),
                }
            });

            _mockGolfRoundAccessLayer.Setup(x => x.GetAllGolfRounds()).ReturnsAsync(new List<GolfRound>() {
                new GolfRound(){
                    Id = new Guid("00000000-0000-0000-0000-000000000001"),
                    Date = _createdAt,
                    CourseId = new Guid("00000000-0000-0000-0001-000000000000"),

                },
                new GolfRound(){
                    Id = new Guid("00000000-0000-0000-0000-000000000002"),
                    Date = _createdAt,
                    CourseId = new Guid("00000000-0000-0000-0002-000000000000"),

                },
                new GolfRound(){
                    Id = new Guid("00000000-0000-0000-0000-000000000003"),
                    Date = _createdAt,
                    CourseId = new Guid("00000000-0000-0000-0001-000000000000"),

                },
                new GolfRound(){
                    Id = new Guid("00000000-0000-0000-0000-000000000004"),
                    Date = _createdAt,
                    CourseId = new Guid("00000000-0000-0000-0003-000000000000"),

                }
            });

            var expected = new List<GolfRoundViewModel>()
            {
                new GolfRoundViewModel
                {
                   Course = new Course(){
                    Id = new Guid("00000000-0000-0000-0000-000000000000"),
                    Name = "Wembley Golf Course",
                    Holes = "18",
                    Par = 72,
                    ScratchRating = 70,
                    Slope = 120,
                    TeeName = "Blue Men"
                },
                Date = _createdAt,
                players = new List<PlayerViewModel>()
                {
                    new PlayerViewModel()
                    {
                        player = new Player(){
                    Id = new Guid("00000000-0000-0000-0000-000000000000"),
                    UserId = new Guid("00000000-0000-0000-0000-000000000000"),
                    FirstName = "Thashin",
                    LastName = "Naidoo",
                    Handicap = new Decimal(19.3),
                },
                        score = 45
                    }
                }
                },
                new GolfRoundViewModel
                {
                   Course = new Course(){
                    Id = new Guid("00000000-0000-0000-0000-000000000000"),
                    Name = "Point Walter",
                    Holes = "1-9",
                    Par = 35,
                    ScratchRating = 34,
                    Slope = 115,
                    TeeName = "Blue Men"
                },
                Date = _createdAt,
                players = new List<PlayerViewModel>()
                {
                    new PlayerViewModel()
                    {
                        player =  new Player()
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000000"),
                            UserId = new Guid("00000000-0000-0000-0000-000000000000"),
                            FirstName = "Michael",
                            LastName = "Nelmes",
                            Handicap = new Decimal(24.8),
                        },
                        score = 63
                    }
                }
                },
                new GolfRoundViewModel
                {
                   Course = new Course(){
                    Id = new Guid("00000000-0000-0000-0000-000000000000"),
                    Name = "Wembley Golf Course",
                    Holes = "18",
                    Par = 72,
                    ScratchRating = 70,
                    Slope = 120,
                    TeeName = "Blue Men"
                },
                Date = _createdAt,
                players = new List<PlayerViewModel>()
                {
                    new PlayerViewModel()
                    {
                        player =  new Player()
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000000"),
                            UserId = new Guid("00000000-0000-0000-0000-000000000000"),
                            FirstName = "Michael",
                            LastName = "Nelmes",
                            Handicap = new Decimal(24.8),
                        },
                        score = 57
                    },
                    new PlayerViewModel()
                    {
                        player = new Player()
                        {
                    Id = new Guid("00000000-0000-0000-0000-000000000000"),
                    UserId = new Guid("00000000-0000-0000-0000-000000000000"),
                    FirstName = "Thashin",
                    LastName = "Naidoo",
                    Handicap = new Decimal(19.3),
                },
                        score = 40
                    }
                }
                }

            };

            var actual = await _sut.Index() as ObjectResult;

            actual.StatusCode.Should().Be(200);
            actual.Value.Should().BeEquivalentTo(expected, options => options.Using<DateTime>(ctx => ctx.Subject.Should().BeWithin(2.Hours()).After(_createdAt)).WhenTypeIs<DateTime>());

        }


        [TestCase]
        public async Task TestCreateGolfRound()
        {
            var testAddGolfRound = new AddGolfRound()
            {
                Date = _createdAt,
                CourseId = new Guid("00000000-0000-0000-0001-000000000000"),
                Scores = new List<Score>()
                {
                                    new Score(){
                    Id = new Guid("00000000-0000-0000-0000-000000000003"),
                    PlayerId = new Guid("00000000-0000-0000-0001-000000000000"),
                    Value=40,
                    GolfRoundId = new Guid("00000000-0000-0000-0000-000000000001"),
                },
                new Score(){
                    Id = new Guid("00000000-0000-0000-0000-000000000004"),
                    PlayerId = new Guid("00000000-0000-0000-0002-000000000000"),
                    Value=57,
                    GolfRoundId = new Guid("00000000-0000-0000-0000-000000000001"),
                }
                }
            };

            _mockGolfRoundAccessLayer.Setup(x => x.AddGolfRound(new GolfRound()
            {
                CourseId = new Guid("00000000-0000-0000-0001-000000000000"),
                Date = _createdAt,
            })).ReturnsAsync(new Guid("00000000-0000-0000-0000-000000000001"));

            _mockScoreAccessLayer.Setup(x => x.AddScore(It.IsAny<Score>())).ReturnsAsync(true);

            _mockCourseAccessLayer.Setup(x => x.GetCourse(new Guid("00000000-0000-0000-0001-000000000000")))
                .ReturnsAsync(new Course()
                {
                    Id = new Guid("00000000-0000-0000-0001-000000000000"),
                    Name = "Point Walter",
                    Holes = "1-9",
                    Location = "Western Australia",
                    Par = 35,
                    ScratchRating = 34,
                    Slope = 115,
                    TeeName = "Blue Men"
                });

            _mockHandicapAccessLayer.Setup(x => x.GetOrderedHandicaps(new Guid("00000000-0000-0000-0001-000000000000"))).ReturnsAsync(
                new List<Handicap>()
                {
                    new Handicap()
                    {
                        CurrentHandicap = 20,
                        Value = 25,
                    }
                }
                );

            _mockHandicapAccessLayer.Setup(x => x.AddHandicap(It.IsAny<Handicap>())).ReturnsAsync(true);
            _mockPlayerAccessLayer.Setup(x => x.GetPlayer(It.IsAny<Guid>())).ReturnsAsync(
                new Player() { }
                );
            _mockPlayerAccessLayer.Setup(x => x.UpdatePlayer(It.IsAny<Player>())).ReturnsAsync(true);
            var actual = await _sut.Create(testAddGolfRound) as ObjectResult;

            actual.StatusCode.Should().Be(200);
        }

        /*
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


        */
    }
}
