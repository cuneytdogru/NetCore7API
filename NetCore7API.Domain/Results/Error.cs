using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.Results
{
    public sealed record Error(string Code, string[] Messages, string Key)
    {
        public string? Message => Messages?.FirstOrDefault();

        public Error(string Code, string Message) : this(Code, new string[] { Message }, string.Empty) { }

        public Error(string Code, string Message, string Key) : this(Code, new string[] { Message }, Key) { }

        public static readonly Error None = new(string.Empty, string.Empty, string.Empty);

        public static Error NotFound() => new(nameof(NotFound), "Record not found.", string.Empty);

        public static Error NotFound(string message) => new(nameof(NotFound), message, string.Empty);

        public static Error ValidationError(string key, string message) => new(nameof(ValidationError), message, key);

        public static Error ValidationError(string key, string[] messages) => new(nameof(ValidationError), messages, key);

        public static IEnumerable<Error> ValidationErrors(IDictionary<string, string[]> keyValue) => keyValue.Select(x => ValidationError(x.Key, x.Value));
    }
}