namespace DiamondShopSystem.BLL.Result
{
    public class Result
    {
        public bool Success { get; protected set; }
        public string Error { get; protected set; }

        protected Result(bool success, string error = null)
        {
            Success = success;
            Error = error;
        }

        public static Result SuccessResult()
        {
            return new Result(true);
        }

        public static Result Fail(string error)
        {
            return new Result(false, error);
        }
    }

    public class Result<T> : Result
    {
        public T Value { get; protected set; }

        protected Result(T value, bool success, string error = null) : base(success, error)
        {
            Value = value;
        }

        public new static Result<T> Success(T value)
        {
            return new Result<T>(value, true);
        }

        public new static Result<T> Fail(string error)
        {
            return new Result<T>(default(T), false, error);
        }
    }
}
