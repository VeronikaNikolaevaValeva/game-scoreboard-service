using game_scoreboard_service.Models.Responses;

namespace game_scoreboard_service.Hubs.Clients
{
    /// <summary>
    /// This is a class that implements the (SignalR) Hub class.
    /// It implements methods that will be called on connected clients from the server.
    /// </summary>
    public interface IScoreboardClient
    {
        /// <summary>
        /// Receives a list of user score response entity models 
        /// to all clients that are listening to the ReceiveUserScores event.
        /// </summary>
        /// <returns>List of UserScoreResponse Entity Models.</returns>
        Task ReceiveListOfUserScores(ScoreBoardResponse userScoreResponses);

    }
}
