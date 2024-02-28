using System;
using System.Collections.Generic;

namespace milescarrental.Application.Models
{
    public class ApiResponseList<T>
    {

        public ApiRequestResponse ApiResponse { get; set; }
        public List<T> List { get; set; }

        public ApiResponseList()
        {
        }
    }
}
