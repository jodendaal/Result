namespace Result
{
    public class Error
    {
        private string _stackTrace;

        public Error(string code, string message,string stackTrace = null)
        {
            Code = code;
            Message = message;
            _stackTrace = stackTrace;
        }

        public Error(Exception ex)
        {
            Code = ex.GetType().ToString();
            Message = ex.Message;
            Exception = ex;

        }

        public Error(Exception ex, string code)
        {
            Code = code;
            Message = ex.Message;
            Exception = ex;
        }

        public Error(Exception ex, string code, string message)
        {
            Code = code;
            Message = message;
            Exception = ex;
        }

        public string StackTrace { get => Exception != null ? Exception.StackTrace : _stackTrace;  }
        public Exception? Exception { get; init; }
        public string Code { get; }
        public string Message { get; }

        public ErrorType Type => Exception != null ? ErrorType.Exception : ErrorType.Error;
    }
}