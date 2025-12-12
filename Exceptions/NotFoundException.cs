namespace MovieStream.Api.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string name, object key) : base($"The ({name}) type entity can not be found with this identifier: ({key}).")
        {}
    }
}
