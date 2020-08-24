namespace Domain.Generic
{
    public abstract class BaseDto
    {
        public string Slug { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}
