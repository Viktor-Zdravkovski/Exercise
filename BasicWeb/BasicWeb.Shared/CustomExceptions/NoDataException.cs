namespace BasicWeb.Shared.CustomExceptions
{
    public class NoDataException : Exception
    {
        public NoDataException(string message) : base(message)
        {
        }
    }
}
