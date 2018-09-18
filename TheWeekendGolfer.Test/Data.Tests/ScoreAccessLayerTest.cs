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
    public class ScoreAccessLayerTest
    {
        private GolfDbContext _context;
        private ScoreAccessLayer _sut;
        private DateTime _createdAt;


        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<GolfDbContext>()
                  .UseInMemoryDatabase(Guid.NewGuid().ToString())
                  .Options;
            _context = new GolfDbContext(options);
            _createdAt = DateTime.Now;
            var scores = new List<Score>()
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
                    PlayerId = new Guid("00000000-0000-0000-0002-000000000000"),
                    Value=63,
                    GolfRoundId = new Guid("00000000-0000-0001-0000-000000000000"),
                    Created = _createdAt,
                },
                new Score(){
                    Id = new Guid("00000000-0000-0000-0000-000000000003"),
                    PlayerId = new Guid("00000000-0000-0000-0001-000000000000"),
                    Value=40,
                    GolfRoundId = new Guid("00000000-0000-0002-0000-000000000000"),
                    Created = _createdAt,
                },
                new Score(){
                    Id = new Guid("00000000-0000-0000-0000-000000000004"),
                    PlayerId = new Guid("00000000-0000-0000-0002-000000000000"),
                    Value=57,
                    GolfRoundId = new Guid("00000000-0000-0003-0000-000000000000"),
                    Created = _createdAt,
                }
            }.AsQueryable();

            _context.Scores.AddRange(scores);
            _context.SaveChanges();
            _sut = new ScoreAccessLayer(_context);
        }

        [TestCase("00000000-0000-0000-0000-000000000004")]
        [TestCase("00000000-0000-0000-0000-000000000002")]
        public async Task TestGetScore(string id)
        {
            var actual = await _sut.GetScore(new Guid(id));

            actual.Should().BeOfType<Score>();
        }

        [TestCase("00000000-0000-0000-0000-000000000005")]
        public async Task TestGetScoreException(string id)
        {
            Func<Task> action = async () => await _sut.GetScore(new Guid(id));

            action.Should().Throw<Exception>();
        }

        [TestCase]
        public async Task TestGetAllScores()
        {
            var expected = new List<Score>()
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
                    PlayerId = new Guid("00000000-0000-0000-0002-000000000000"),
                    Value=63,
                    GolfRoundId = new Guid("00000000-0000-0001-0000-000000000000"),
                    Created = _createdAt,
                },
                new Score(){
                    Id = new Guid("00000000-0000-0000-0000-000000000003"),
                    PlayerId = new Guid("00000000-0000-0000-0001-000000000000"),
                    Value=40,
                    GolfRoundId = new Guid("00000000-0000-0002-0000-000000000000"),
                    Created = _createdAt,
                },
                new Score(){
                    Id = new Guid("00000000-0000-0000-0000-000000000004"),
                    PlayerId = new Guid("00000000-0000-0000-0002-000000000000"),
                    Value=57,
                    GolfRoundId = new Guid("00000000-0000-0003-0000-000000000000"),
                    Created = _createdAt,
                }
            };

            var actual = await _sut.GetAllScores();

            actual.Should().BeEquivalentTo(expected);

        }

        [TestCase("00000000-0000-0000-0001-000000000000")]
        public async Task TestGetAllPlayerScores(string playerId)
        {
            var expected = new List<Score>()
            {
                new Score(){
                    Id = new Guid("00000000-0000-0000-0000-000000000001"),
                    PlayerId = new Guid("00000000-0000-0000-0001-000000000000"),
                    Value=45,
                    GolfRoundId = new Guid("00000000-0000-0001-0000-000000000000"),
                    Created = _createdAt,
                },

                new Score(){
                    Id = new Guid("00000000-0000-0000-0000-000000000003"),
                    PlayerId = new Guid("00000000-0000-0000-0001-000000000000"),
                    Value=40,
                    GolfRoundId = new Guid("00000000-0000-0002-0000-000000000000"),
                    Created = _createdAt,
                }
            };

            var actual = await _sut.GetAllPlayerScores(new Guid(playerId));

            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("00000000-0000-0000-0000-000000000005")]
        public async Task TestGetAllPlayerScoresException(string id)
        {
            Func<Task> action = async () => await _sut.GetAllPlayerScores(new Guid(id));

            action.Should().Throw<Exception>();
        }


        [TestCase]
        public async Task TestAddScore()
        {
            var expected = _context.Scores.Count() + 1;

            await _sut.AddScore(
                new Score(){
                    Id = new Guid("00000000-0000-0000-0000-000000000005"),
                    PlayerId = new Guid("00000000-0000-0000-0003-000000000000"),
                    Value=57,
                    GolfRoundId = new Guid("00000000-0000-0003-0000-000000000000"),
                    Created = _createdAt,
                }
            );

            var actual = _context.Scores.Count();

            actual.Should().Equals(expected);
        }


        [TestCase]
        public async Task TestAddScores()
        {
            var expected = _context.Scores.Count() + 2;

            var scores = new List<Score>()
            {
                new Score(){
                    Id = new Guid("00000000-0000-0000-0000-000000000006"),
                    PlayerId = new Guid("00000000-0000-0000-0004-000000000000"),
                    Value=35,
                    GolfRoundId = new Guid("00000000-0000-0001-0000-000000000000"),
                    Created = _createdAt,
                },

                new Score(){
                    Id = new Guid("00000000-0000-0000-0000-000000000007"),
                    PlayerId = new Guid("00000000-0000-0000-0002-000000000000"),
                    Value=44,
                    GolfRoundId = new Guid("00000000-0000-0004-0000-000000000000"),
                    Created = _createdAt,
                }
            };
            await _sut.AddScores(scores);

            var actual = _context.Scores.Count();

            actual.Should().Equals(expected);
        }

    }
}
