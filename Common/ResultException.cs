using System.Text.Json.Serialization;
using WebApi_TimeScale.Data.Entity;

namespace TimeScaleApi.Common
{
    public class ResultException
    {
        [JsonIgnore]
        public bool IsSuccess { get; set; }
        [JsonIgnore]
        public string? Error { get; set; } 

        protected ResultException(bool isSuccess, string error) 
        {
            IsSuccess = isSuccess; 
            Error = error;
        }

        public static ResultException Success() => new(true, null);
        public static ResultException Failure(string? error) => new(false, error);

    }

    public class ResultException<T> : ResultException
    {
        [JsonIgnore]
        public T? Value { get; set; }

        private ResultException(bool isSuccess, string? error, T? value): base(isSuccess, error) 
        {
            Value = value;
        }

        public static ResultException<T> Success(T value) => new(true, null, value);
        public static ResultException<T> Failure(string error) => new(false, error,default);

        
    }
}
