namespace game_scoreboard_service.Models
{
    public interface IBaseModel
    {
        public int Id { get; set; }
        public string PartitionKey { get; set; }
    }
}
