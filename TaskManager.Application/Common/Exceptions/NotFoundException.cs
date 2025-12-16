
namespace TaskManager.Application.Common.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string message) : base(message) { }

        public static NotFoundException For<T>(object id)
        {
            return new NotFoundException($"{typeof(T).Name} with id '{id}' was not found.");
        }
    }
}
