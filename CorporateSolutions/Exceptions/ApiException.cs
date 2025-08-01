namespace CorporateSolutions.Exceptions
{
    public class ApiException : Exception
    {
        public string Code { get; private set; }

        public ApiException(string code, string message) : base(message)
        {
            Code = code;
        }
    }
}