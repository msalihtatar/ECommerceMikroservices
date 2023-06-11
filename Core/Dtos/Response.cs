using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Core.Dtos
{
    public class Response<T> 
    {
        public T Data { get; set; }
        [JsonIgnore]
        public int StatusCode { get; set; }
        [JsonIgnore]
        public bool IsSuccess { get; set; }
        public List<string> ErrorMsg { get; set; }

        public static Response<T> Success(T data, int StatusCode)
        {
            return new Response<T>
            {
                Data = data,
                StatusCode = StatusCode,
                IsSuccess = true
            };
        }

        public static Response<T> Success(int StatusCode)
        {
            return new Response<T>
            {
                Data = default(T),
                StatusCode = StatusCode,
                IsSuccess = true
            };
        }

        public static Response<T> Fail(List<string> ErrorList, int StatusCode)
        {
            return new Response<T>
            {
                ErrorMsg = ErrorList,
                StatusCode = StatusCode,
                IsSuccess = false
            };
        }

        public static Response<T> Fail(string ErrorList, int StatusCode)
        {
            return new Response<T>
            {
                ErrorMsg = new List<string>() { ErrorList },
                StatusCode = StatusCode,
                IsSuccess = false
            };
        }
    }
}
