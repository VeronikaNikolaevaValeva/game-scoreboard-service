namespace game_scoreboard_service.Models.Responses
{
    public class UserScoreResponse
    {
        public string? Nickname { get; set; }
        public int? ChartPosition { get; set; }
        public double? OverallScore { get; set; }
        public int? AmountAnsweredQuestions { get; set; }
        public int? AmountOfPlayedGames { get; set; }
        public int? CorrectAnswerCount { get; set; }
        public int? IncorrectAnswerCount { get; set; }
        public int? NonAnsweredCount { get; set; }
    }
}
