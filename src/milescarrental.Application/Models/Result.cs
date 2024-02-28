using System;
using System.Diagnostics.CodeAnalysis;

namespace milescarrental.Application.Models
{
    public partial class Result : IEquatable<Result>
    {
        public Result()
        {
        }

        public string result { get; set; }


        public bool Equals([AllowNull] Result other)
        {
            throw new NotImplementedException();
        }
    }
}
