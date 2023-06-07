using game_scoreboard_service.Models.Messaging;

namespace game_scoreboard_service.Messaging.Interfaces
{
    /// <summary>
    /// Service that recieves messages from the game logic service
    /// </summary>
    public interface IMessagingSubscriber
    {
        /// <summary>
        /// Listens up to a queue and recieve messages when a new user is registered into the game.
        /// </summary>
        /// <returns>The recieved data from the queue - emty entity model if no data was recieved.</returns>
        NewRegisteredUser NewRegisteredUser();

        /// <summary>
        /// Listens up to a queue and recieve messages when any of the existing users have played a geame - updated their score.
        /// </summary>
        /// <returns>The recieved data from the queue - emty entity model if no data was recieved.</returns>
        UpdatedUserScore UpdateUserScore();

        /// <summary>
        /// Listens up to a queue and recieve messages when any of the existing users daata has to be deleted.
        /// </summary>
        /// <returns>The recieved data from the queue - emty string if no data was recieved.</returns>
        string DeleteUserData();

        /// <summary>
        /// Notifies the game logic service if the deletion of the user was successful. 
        /// </summary>
        void DeletedUserData(bool deletionResult);
    }
}
