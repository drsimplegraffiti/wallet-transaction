// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;

// namespace OctApp.Dto.Response
// {
//     public class ApiResponse<T>
//     {
//         public T Data { get; set; } = default!;
//         public bool Success { get; set; } = true;
//         public string Message { get; set; } = string.Empty;

//         public int StatusCode { get; set; } = 200;


//     }

//     // Error response
//      public class ErrorResponse
//     {
//         public string Message { get; set; } = string.Empty;
//         public bool Success { get; set; } = false;
//         public string? Error { get; set; } = string.Empty;
//         public string? Description { get; set; } = string.Empty;

//         public int StatusCode { get; set; } = 500;


//     }


    
    
// }


using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OctApp.Dto.Response
{
    public class ApiResponse<T>
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T Data { get; set; } = default!;
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; } = 200;

        // Error response
        public class ErrorResponse
        {
            public string Message { get; set; } = string.Empty;
            public bool Success { get; set; } = false;
            public string? Error { get; set; } = string.Empty;
            public string? Description { get; set; } = string.Empty;
            public int StatusCode { get; set; } = 500;
        }

        // Unauthorized response
        public static ApiResponse<T> UnauthorizedResponse()
        {
            return new ApiResponse<T>
            {
                Success = false,
                StatusCode = 401,
                Message = "Unauthorized"
            };
        }

        // Not found response
        public static ApiResponse<T> NotFoundResponse(string message)
        {
            return new ApiResponse<T>
            {
                Success = false,
                StatusCode = 404,
                Message = message
            };
        }

        // BadRequest response
        public static ApiResponse<T> BadRequestResponse(string message)
        {
            return new ApiResponse<T>
            {
                Success = false,
                StatusCode = 400,
                Message = message
            };
        }

        // InternalServerError response
        public static ApiResponse<T> InternalServerErrorResponse(string message)
        {
            return new ApiResponse<T>
            {
                Success = false,
                StatusCode = 500,
                Message = message
            };
        }

        //conflict response
        public static ApiResponse<T> ConflictResponse(string message)
        {
            return new ApiResponse<T>
            {
                Success = false,
                StatusCode = 409,
                Message = message
            };
        }

        // Forbidden response
        public static ApiResponse<T> ForbiddenResponse(string message)
        {
            return new ApiResponse<T>
            {
                Success = false,
                StatusCode = 403,
                Message = message
            };
        }

        // Created response
        public static ApiResponse<T> CreatedResponse(T data, string message)
        {
            return new ApiResponse<T>
            {
                Success = true,
                StatusCode = 201,
                Message = message,
                Data = data
            };
        }

        // Ok response
        public static ApiResponse<T> OkResponse(T data, string message)
        {
            return new ApiResponse<T>
            {
                Success = true,
                StatusCode = 200,
                Message = message,
                Data = data
            };
        }

        // Accepted response
        public static ApiResponse<T> AcceptedResponse(T data, string message)
        {
            return new ApiResponse<T>
            {
                Success = true,
                StatusCode = 202,
                Message = message,
                Data = data
            };
        }

        // NoContent response
        public static ApiResponse<T> NoContentResponse(string message)
        {
            return new ApiResponse<T>
            {
                Success = true,
                StatusCode = 204,
                Message = message
            };
        }


    }
}
