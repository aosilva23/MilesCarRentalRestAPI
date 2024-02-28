using System;
using System.Collections.Generic;

namespace milescarrental.Application.Models
{
    public class ApiResponsePagination<T>
    {
        public ApiRequestResponse ApiResponse { get; set; }
        public ApiPagination ApiPagination { get; set; }
        public List<T> List { get; set; }

        public ApiResponsePagination()
        {
        }
    }
}
