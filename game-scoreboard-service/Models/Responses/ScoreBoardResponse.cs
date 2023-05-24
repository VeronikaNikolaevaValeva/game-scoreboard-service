namespace game_scoreboard_service.Models.Responses
{
    public class ScoreBoardResponse
    {
        public List<UserScoreResponse>? UserScoreResponses { get; set; }
        public int? TotalUserScores { get; set; }
        public int? PageUserScores { get; set; }
    }
}
