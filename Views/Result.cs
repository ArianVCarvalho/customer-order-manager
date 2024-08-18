namespace Frenet.ShipManagement.Views
{
    public class Result<T>
    {
        public bool IsSuccess { get; private set; }
        public T Value { get; private set; }
        public string ErrorMessage { get; private set; }
        public int StatusCode { get; private set; }

        private Result(bool success, T value, int statusCode, string errorMessage)
        {
            IsSuccess = success;
            Value = value;
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
        }

        public static Result<T> Success(T value)
        {
            return new Result<T>(true, value, 200, string.Empty);
        }

        public static Result<T> Failure(int statusCode, string errorMessage)
        {
            return new Result<T>(false, default, statusCode, errorMessage);
        }
    }
}
