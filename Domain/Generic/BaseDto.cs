namespace Domain.Generic
{
    public record BaseDto
    {
        public string Slug { get; init; }
        public string CreatedAt { get; init; }
        public string UpdatedAt { get; init; }
    }
}
