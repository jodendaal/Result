using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace Result
{
    public class Result<TResult>
    {
        private Result(TResult? result)
        {
            Value = result;
        }

        private Result(Exception exception)
        {
            Error = new Error(exception);
        }

        private Result(Exception exception, string code)
        {
            Error = new Error(exception, code);
        }

        private Result(Exception exception,string code, string errorMessaage)
        {
            Error = new Error(exception, code, errorMessaage);
        }
        private Result(Error error)
        {
            Error = error;
        }

        private Result(string code, string errorMessage,string stackTrace = null)
        {
            Error = new Error(code, errorMessage, stackTrace);
        }

        private Result(Dictionary<string, string[]> validationErrors)
        {
            ValidationErrors = new ReadOnlyDictionary<string, string[]>(validationErrors);
        }

        private Result(ReadOnlyDictionary<string, string[]> validationErrors)
        {
            ValidationErrors = new ReadOnlyDictionary<string, string[]>(validationErrors);
        }

        public static Result<TResult> Failure(Exception ex)
        {
            return new Result<TResult>(ex);
        }

        public static Result<TResult> Failure(Exception ex,string code)
        {
            return new Result<TResult>(ex,code);
        }

        public static Result<TResult> Failure(Error error)
        {
            return new Result<TResult>(error);
        }

        public static Result<TResult> Failure(Exception ex,string code, string message)
        {
            return new Result<TResult>(ex, code,message);
        }

        public static Result<TResult> Failure( string code, string message,[CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumberAttribute = 0)
        {
            var stackTrace = $"at {callerMemberName} in {callerFilePath}:line {callerLineNumberAttribute}";
            return new Result<TResult>(code, message, stackTrace);
        }

        public static Result<TResult> Validation(Dictionary<string, string[]> validationErrors)
        {
            return new Result<TResult>(validationErrors);
        }

        public static Result<TResult> Validation(ReadOnlyDictionary<string, string[]> validationErrors)
        {
            return new Result<TResult>(validationErrors);
        }

        public static Result<TResult> From<TFrom>(Result<TFrom> from)
        {
            switch (from.ErrorType)
            {
                case FailureType.Error:
                    switch (from.Error!.Type)
                    {
                        case Result.ErrorType.Error:
                            return Result<TResult>.Failure(from.Error!);
                        case Result.ErrorType.Exception:
                            return Result<TResult>.Failure(from.Error.Exception!);
                    }
                    break;
                case FailureType.Validation:
                    return Result<TResult>.Validation(from.ValidationErrors!);
            }

            throw new Exception("Only error and validation results area ble to be converted");
        }

        public static Result<TResult> Validation(string field, string[] fieldValidationErrors)
        {
            var validationError = new Dictionary<string, string[]>
            {
                { field, fieldValidationErrors }
            };
            return new Result<TResult>(validationError);
        }

        public static Result<TResult> Validation(string field, string validationError)
        {
            var validationErrors = new Dictionary<string, string[]>
            {
                { field, new string[] { validationError } }
            };
            return new Result<TResult>(validationErrors);
        }

        public T Match<T>(Func<TResult?, T> success, Func<Error?, T> error, Func<ReadOnlyDictionary<string, string[]>?, T> validation)
        {
            return this.IsSuccess ? success(this.Value) : (Error != null ? error(this.Error) : validation(this.ValidationErrors));
        }

        public ReadOnlyDictionary<string, string[]>? ValidationErrors { get; init; } 

        public TResult? Value { get; init; }
        public Error? Error { get; init; }
        public bool IsSuccess => Error == null && ValidationErrors == null;

        public FailureType ErrorType => Error != null ? FailureType.Error : (ValidationErrors != null ? FailureType.Validation : FailureType.None);

        public static implicit operator Result<TResult>(TResult? result) => new Result<TResult>(result);
        public static implicit operator TResult(Result<TResult> result) => result;
    }
}