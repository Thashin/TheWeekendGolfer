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

namespace TheWeekendGolfer.Tests
{
    [TestFixture]
    public class CourseControllerTest
    {

        private Mock<ICourseAccessLayer> _mockCourseAccessLayer;
        private CourseController _sut;
        private DateTime _createdAt;


        [SetUp]
        public void Setup()
        {
            _mockCourseAccessLayer = new Mock<ICourseAccessLayer>();
            _sut = new CourseController(_mockCourseAccessLayer.Object);
            _createdAt = DateTime.Now;
        }

        [TestCase]
        public void TestGetCourseNames()
        {
            _mockCourseAccessLayer.Setup(x => x.GetCourseNames()).Returns(new List<string>(){
                    "Wembley Golf Course",
                    "Point Walter"
            });
            var expected = new List<string>(){
                    "Wembley Golf Course",
                    "Point Walter"
            };

            var actual = _sut.GetCourseNames() as ObjectResult;

            actual.StatusCode.Should().Be(200);
            actual.Value.Should().BeEquivalentTo(expected);
        }


        [TestCase]
        public void TestIndex()
        {
            _mockCourseAccessLayer.Setup(x => x.GetAllCourses()).Returns(new List<Course>(){
                new Course(){
                    Id = new Guid("00000000-0000-0000-0000-000000000001"),
                    Name = "Wembley Golf Course",
                    Created =_createdAt,
                    Holes = "18",
                    Location = "Western Australia",
                    Par = 72,
                    ScratchRating = 70,
                    Slope = 120,
                    TeeName = "Blue Men"
                },
                new Course(){
                    Id = new Guid("00000000-0000-0000-0000-000000000002"),
                    Name = "Point Walter",
                    Created = _createdAt,
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
                    Created = _createdAt,
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
                    Created = _createdAt,
                    Holes = "1-9",
                    Location = "Western Australia",
                    Par = 35,
                    ScratchRating = 34,
                    Slope = 117,
                    TeeName = "Red Women"
                }
            });

            var expected = new List<Course>(){
                new Course(){
                    Id = new Guid("00000000-0000-0000-0000-000000000001"),
                    Name = "Wembley Golf Course",
                    Created =_createdAt,
                    Holes = "18",
                    Location = "Western Australia",
                    Par = 72,
                    ScratchRating = 70,
                    Slope = 120,
                    TeeName = "Blue Men"
                },
                new Course(){
                    Id = new Guid("00000000-0000-0000-0000-000000000002"),
                    Name = "Point Walter",
                    Created = _createdAt,
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
                    Created = _createdAt,
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
                    Created = _createdAt,
                    Holes = "1-9",
                    Location = "Western Australia",
                    Par = 35,
                    ScratchRating = 34,
                    Slope = 117,
                    TeeName = "Red Women"
                }
            };

            var actual = _sut.Index() as ObjectResult;

            actual.StatusCode.Should().Be(200);
            actual.Value.Should().BeEquivalentTo(expected);

        }

        [TestCase("Point Walter", "Blue Men")]
        public void TestGetCourseDetails(string courseName, string tee)
        {
            _mockCourseAccessLayer.Setup(x => x.GetCourseHoles(courseName,tee)).Returns(new List<Course>(){
                new Course(){
                    Id = new Guid("00000000-0000-0000-0000-000000000002"),
                    Name = "Point Walter",
                    Created = _createdAt,
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
                    Created = _createdAt,
                    Holes = "18",
                    Location = "Western Australia",
                    Par = 35,
                    ScratchRating = 34,
                    Slope = 115,
                    TeeName = "Blue Men"
                },
            });

            var expected = new List<Course>(){
                new Course(){
                    Id = new Guid("00000000-0000-0000-0000-000000000002"),
                    Name = "Point Walter",
                    Created = _createdAt,
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
                    Created = _createdAt,
                    Holes = "18",
                    Location = "Western Australia",
                    Par = 35,
                    ScratchRating = 34,
                    Slope = 115,
                    TeeName = "Blue Men"
                }
            };

            var actual = _sut.GetCourseDetails(courseName, tee) as ObjectResult;

            actual.StatusCode.Should().Be(200);
            actual.Value.Should().BeEquivalentTo(expected);
        }

        [TestCase("Point Walter")]
        public void TestGetCourseTeeDetails(string courseName)
        {
            _mockCourseAccessLayer.Setup(x => x.GetCourseTees(courseName)).Returns(new List<string>(){
                "Blue Men",
                "Red Women"
            });
            var expected = new List<string>(){
                "Blue Men",
                "Red Women"
            };


            var actual = _sut.GetCourseDetails(courseName, null) as ObjectResult;

            actual.StatusCode.Should().Be(200);
            actual.Value.Should().BeEquivalentTo(expected);
        }

        [TestCase("00000000-0000-0000-0000-000000000001")]
        public void TestDetails(string id)
        {
            _mockCourseAccessLayer.Setup(x => x.GetCourse(new Guid(id))).Returns(new Course()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                Name = "Point Walter",
                Created = _createdAt,
                Holes = "1-9",
                Location = "Western Australia",
                Par = 35,
                ScratchRating = 34,
                Slope = 115,
                TeeName = "Blue Men"
            });

            var expected = new Course()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                Name = "Point Walter",
                Created = _createdAt,
                Holes = "1-9",
                Location = "Western Australia",
                Par = 35,
                ScratchRating = 34,
                Slope = 115,
                TeeName = "Blue Men"
            };
            var actual = _sut.Details(new Guid(id)) as ObjectResult;

            actual.StatusCode.Should().Be(200);
            actual.Value.Should().BeEquivalentTo(expected);
        }

    }
}
