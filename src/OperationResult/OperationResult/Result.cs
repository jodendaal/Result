namespace OperationResult
{
    public class Result<TResult>
    {
        private Result(TResult? result)
        {
            Value = result;
        }

        private Result(Exception exception, string? errorMessaage = null)
        {
            Exception = exception;
            ErrorMessage = errorMessaage ?? exception.Message;
        }

        private Result(Dictionary<string, string[]> validationErrors)
        {
            ValidationErrors = validationErrors;
        }

        public static Result<TResult> Failure(Exception ex)
        {
            return new Result<TResult>(ex);
        }

        public static Result<TResult> Validation(Dictionary<string, string[]> validationErrors)
        {
            return new Result<TResult>(validationErrors);
        }

        public static Result<TResult> Validation(string field, string[] fieldValidationErrors)
        {
            var validationError = new Dictionary<string, string[]>();
            validationError.Add(field, fieldValidationErrors);
            return new Result<TResult>(validationError);
        }

        public static Result<TResult> Validation(string field, string validationError)
        {
            var validationErrors = new Dictionary<string, string[]>();
            validationErrors.Add(field, new string[] { validationError });
            return new Result<TResult>(validationErrors);
        }


        public Dictionary<string, string[]> ValidationErrors { get; set; } = new();

        public TResult? Value { get; set; }
        public string ErrorMessage { get; set; }
        public Exception Exception { get; set; }
        public bool IsSuccess => Exception == null && ValidationErrors.Count() == 0;

        public ErrorType ErrorType => Exception != null ? ErrorType.Exception : (ValidationErrors.Count() > 0 ? ErrorType.Validation : ErrorType.None);

        public static implicit operator Result<TResult>(TResult? result) => new Result<TResult>(result);
    }

    public enum ErrorType
    {
        None,
        Exception,
        Validation
    }
}