﻿using NUnit.Framework;
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
    public class CourseAccessLayerTest
    {
        private GolfDbContext _context;
        private CourseAccessLayer _sut;
        private DateTime _createdAt;


        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<GolfDbContext>()
                  .UseInMemoryDatabase(Guid.NewGuid().ToString())
                  .Options;
            _context = new GolfDbContext(options);
            _createdAt = DateTime.Now;
            var courses = new List<Course>()
            {
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
            }.AsQueryable();

            _context.Courses.AddRange(courses);
            _context.SaveChanges();
            _sut = new CourseAccessLayer(_context);
        }

        [TestCase("00000000-0000-0000-0000-000000000001")]
        [TestCase("00000000-0000-0000-0000-000000000002")]
        public async Task TestGetCourse(string id)
        {
            var actual = await _sut.GetCourse(new Guid(id));

            actual.Should().BeOfType<Course>();
        }

        [TestCase("00000000-0000-0000-0000-000000000005")]
        public async Task TestGetCourseException(string id)
        {
            Func<Task> action = async () => await _sut.GetCourse(new Guid(id));

            action.Should().Throw<Exception>();
        }

        [TestCase]
        public async Task TestGetAllCourses()
        {
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

            var actual = await _sut.GetAllCourses();

            actual.Should().BeEquivalentTo(expected);

        }

        [TestCase]
        public async Task TestGetCourseNames()
        {
            var expected = new List<string>(){
                    "Wembley Golf Course",
                    "Point Walter"
            };

            var actual = await _sut.GetCourseNames();

            actual.Should().BeEquivalentTo(expected);
        }


        [TestCase("Point Walter")]
        public async Task TestGetCourseTees(string courseName)
        {
            var expected = new List<string>(){
                    "Blue Men",
                    "Red Women"
            };

            var actual = await _sut.GetCourseTees(courseName);

            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("Joint Smalter")]
        public async Task TestGetCourseTeesException(string courseName)
        {
            Func<Task> action = async () => await _sut.GetCourseTees(courseName);

            action.Should().Throw<Exception>();
        }


        [TestCase("Point Walter","Blue Men")]
        public async Task TestGetCourseHoles(string courseName, string courseTee)
        {
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
                },
            };

            var actual = await _sut.GetCourseHoles(courseName,courseTee);

            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("Joint Smalter", "Blue Men")]
        [TestCase("Point Walter", "Green Men")]
        public async Task TestGetCourseHolesException(string courseName, string courseTee)
        {
            Func<Task> action = async() => await _sut.GetCourseHoles(courseName,courseTee);
            action.Should().Throw<Exception>();
        }

        [TestCase]
        public async Task TestAddCourse()
        {
            var expected = _context.Courses.Count() + 1;

            await _sut.AddCourse(new Course()
            {
                Id = new Guid(),
                Name = "Test Course",
                Created = DateTime.Now,
                Holes = "1-9",
                Location = "Western Australia",
                Par = 36,
                ScratchRating = 37,
                Slope = 128,
                TeeName = "Blue Men"
            });

            var actual = _context.Courses;

            actual.Should().HaveCount(expected);
        }


        [TestCase]
        public async Task TestGetCourseStats()
        {
            var courseIds = new List<Guid>() {
                new Guid("00000000-0000-0000-0000-000000000001"),
                new Guid("00000000-0000-0000-0000-000000000004"),
                new Guid("00000000-0000-0000-0000-000000000001"),
                new Guid("00000000-0000-0000-0000-000000000003"),
                new Guid("00000000-0000-0000-0000-000000000002")
            };

            var expected = new Dictionary<string, int>();
            expected.Add("Point Walter", 3);
            expected.Add("Wembley Golf Course", 2);

            var actual = await _sut.GetCourseStats(courseIds);

            actual.Should().BeEquivalentTo(expected);

        }

    }
}
