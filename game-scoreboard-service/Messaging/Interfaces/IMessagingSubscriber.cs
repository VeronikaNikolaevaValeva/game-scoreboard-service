using game_scoreboard_service.Models.Messaging;

namespace game_scoreboard_service.Messaging.Interfaces
{
    /// <summary>
    /// Service that recieves messages from the game logic service
    /// </summary>
    public interface IMessagingSubscriber
    {
        /// <summary>
        /// Listen up to a queue and recieve messages when a new user is registered into the game.
        /// </summary>
        /// <returns>The recieved data from the queue - emty entity model is no data was recieved.</returns>
        NewRegisteredUser NewRegisteredUser();

        /// <summary>
        /// Listen up to a queue and recieve messages when any of the existing users have played a geame - updated their score
        /// </summary>
        /// <returns>The recieved data from the queue - emty entity model is no data was recieved.</returns>
        UpdatedUserScore UpdateUserScore();
    }
}
