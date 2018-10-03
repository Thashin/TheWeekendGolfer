using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWeekendGolfer.Models;

namespace TheWeekendGolfer.Data
{
    /// <summary>
    /// This is used to access the Score table which tracks a players score for a particular golf round
    /// </summary>
    public interface IScoreAccessLayer
    {
        /// <summary>
        /// Retrieve a score
        /// </summary>
        /// <param name="id">The Guid of the score</param>
        /// <returns>Score with the Guid of id</returns>
        Score GetScore(Guid id);

        /// <summary>
        /// Retrieves all scores in the Score table
        /// </summary>
        /// <returns>All scores in the Score table</returns>
        IEnumerable<Score> GetAllScores();

        /// <summary>
        /// Retrieve all scores for a player
        /// </summary>
        /// <param name="playerId">The id for the player whose scores you would like to retrieve</param>
        /// <returns>A list of scores</returns>
        IEnumerable<Score> GetAllPlayerScores(Guid playerId);


        /// <summary>
        /// Adds a score to the Score Table.
        /// A score cannot be added if there already exists 4 scores with the same golfRoundId.
        /// A score cannot be added if there already exists a score with the same golfRoundId and PlayerId.
        /// </summary>
        /// <param name="score"> The Score to be added</param>
        /// <returns>Whether the score was added successfully or not</returns>
        Task<Boolean> AddScore(Score score);

        /// <summary>
        /// Adds a list of scores to the Score Table.
        /// </summary>
        /// <param name="scores"> The list of Scores to be added</param>
        /// <returns>Whether the score was added successfully or not</returns>
        Task<Boolean> AddScores(IEnumerable<Score> scores);
        
    }
}
