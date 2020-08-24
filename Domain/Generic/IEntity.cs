namespace Domain.Generic
{
    public interface IEntity<R> where R : BaseDto
    {
        public R ToDto();
    }
}
