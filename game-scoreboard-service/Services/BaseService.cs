using game_scoreboard_service.Models.Enum;

namespace game_scoreboard_service.Services
{
    public abstract class BaseService
    {
        /// <summary>
        /// Reject a service call with a response type
        /// 
        /// Not recommended for most usecases, as <code>return new(Reject(code, reason))</code> is usually a more usable solution
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="code"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        protected ServiceProduct<T> Reject<T>(RejectionCode code, string reason) =>
            new() { RejectionCode = code, RejectionReason = reason, Success = false };


        /// <summary>
        /// Reject a service call without a response type
        /// </summary>
        /// <param name="code"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        protected ServiceProduct Reject(RejectionCode code, string reason) =>
            new() { RejectionCode = code, RejectionReason = reason, Success = false };

        /// <summary>
        /// Mark a service call without a response type as successful
        /// </summary>
        /// <returns></returns>
        protected ServiceProduct Success() => new() { Success = true };
    }
}
