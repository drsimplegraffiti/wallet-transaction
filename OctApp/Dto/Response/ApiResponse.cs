using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OctApp.Dto.Response
{
    public class ApiResponse<T>
    {
        public ApiResponse(int statusCode, T data = default!, string message = null!)
        {
            StatusCode = statusCode;
            Data = data;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode) ?? "Unknown Status Code";
        }

        public int StatusCode { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                200 => "Success",
                201 => "Created",
                202 => "Accepted",
                203 => "Non-Authoritative Information",
                204 => "No Content",
                400 => "Bad Request",
                401 => "Unauthorized",
                403 => "Forbidden",
                404 => "Resource Not Found",
                409 => "Resource Conflict",
                500 => "Internal Server Error",
                501 => "Not Implemented",
                502 => "Bad Gateway",
                _ => "Unknown Status Code"
            };
        }
    }
}
