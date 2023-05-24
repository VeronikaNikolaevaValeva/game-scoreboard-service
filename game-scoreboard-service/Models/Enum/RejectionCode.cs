namespace game_scoreboard_service.Models.Enum
{
    /// <summary>
    /// Different categories of reasons for service call rejection
    /// </summary>
    public enum RejectionCode
    {
        General = 0,
        InsufficientPermission = 1,
        DataValidation = 2,
        NotFound = 3
    }
}
