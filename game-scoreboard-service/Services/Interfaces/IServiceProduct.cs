using game_scoreboard_service.Models.Enum;

namespace game_scoreboard_service.Services.Interfaces
{
    /// <summary>
    /// Wrapper around a resultless service call.
    /// 
    /// Contains if the service call was successfully processed or 
    /// rejected, and a reason for rejection.
    /// </summary>
    public interface IServiceProduct
    {
        /// <summary>
        /// Reason for rejection
        /// </summary>
        RejectionCode? RejectionCode { get; set; }
        /// <summary>
        /// Code for rejection
        /// </summary>
        string? RejectionReason { get; set; }
        /// <summary>
        /// Marks if a call was processed to the end successfully, or if it was rejected at some point
        /// </summary>
        bool Success { get; set; }
    }
}
