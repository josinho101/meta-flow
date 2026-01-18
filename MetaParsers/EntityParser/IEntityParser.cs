using Models.Entity;

namespace MetaParsers.EntityParser
{
    public interface IEntityParser<T>
    {
        public Entity Parse(T input);
    }
}
