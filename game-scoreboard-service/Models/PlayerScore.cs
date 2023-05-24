namespace game_scoreboard_service.Models
{
    public class PlayerScore : IBaseModel
    {
        public int Id { get; set; }
        public string PartitionKey { get; set; }
        public string? Nickname { get;set; } 
        public string? EmailAddress { get;set; }
        public double? OverallScore { get;set; }
        public int? AmountAnsweredQuestions { get;set; }
        public int? AmountOfPlayedGames { get;set; }
        public int? CorrectAnswerCount { get;set; }
        public int? IncorrectAnswerCount { get; set; }
        public int? NonAnsweredCount { get; set; }
    }
}
