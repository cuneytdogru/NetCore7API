using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NetCore7API.Domain.Results
{
    public interface IResult
    {
        public bool IsSuccess { get; }

        public bool IsFailure { get; }

        public IEnumerable<Error> Errors { get; }

        public Error? Error { get; }
    }

    public interface IResult<T> : IResult
    {
        public T Value { get; }
    }

    public abstract class BaseResult : IResult
    {
        internal BaseResult()
        {
            IsSuccess = true;
            Errors = new List<Error>() { Error.None };
        }

        internal BaseResult(Error error)
        {
            if (error == Error.None)
                throw new ArgumentException("Invalid error");

            IsSuccess = false;
            Errors = new List<Error>() { error };
        }

        internal BaseResult(IEnumerable<Error> errors)
        {
            if (errors.Any(x => x == Error.None))
                throw new ArgumentException("Invalid error");

            IsSuccess = false;
            Errors = errors;
        }

        public bool IsSuccess { get; init; }

        public bool IsFailure => !IsSuccess;

        public IEnumerable<Error> Errors { get; init; } = Enumerable.Empty<Error>();

        public Error? Error => Errors.FirstOrDefault();
    }

    public class Result : BaseResult, IResult
    {
        internal Result() : base()
        {
        }

        private Result(Error error) : base(error)
        {
        }

        private Result(IEnumerable<Error> errors) : base(errors)
        {
        }

        public static Result Success() => new();

        public static Result Failure(Error error) => new Result(error);

        public static Result Failure(IEnumerable<Error> errors) => new Result(errors);
    }

    public class Result<T> : BaseResult, IResult, IResult<T>
    {
        private readonly T? _value;

        private Result(T value) : base()
        {
            Value = value;
        }

        private Result(Error error) : base(error)
        {
        }

        private Result(IEnumerable<Error> errors) : base(errors)
        {
        }

        public T Value
        {
            get
            {
                if (IsFailure)
                    throw new InvalidOperationException("No value for failure");

                return _value!;
            }

            private init => _value = value;
        }

        public static Result<T> Success(T value) => new Result<T>(value);

        public static Result<T> Failure(Error error) => new Result<T>(error);

        public static Result<T> Failure(IEnumerable<Error> errors) => new Result<T>(errors);
    }
}