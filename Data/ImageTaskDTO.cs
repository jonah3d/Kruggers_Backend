namespace Kruggers_Backend.Data
{
    public class ImageTaskDTO
    {
        public ConsumerDTO Consumer { get; set; }
        public CreatorDTO Creator { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? ImageUrl { get; set; }
        public StatusDTO Status { get; set; }
    }
}
