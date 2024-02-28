
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace milescarrental.Application.Models
{ 
    /// <summary>
    /// It allows to perform the requested operation on the content * &#x60;1&#x60; INITIAL PAGE * &#x60;2&#x60; NEXT PAGE * &#x60;3&#x60; PREVIOUS PAGE * &#x60;4&#x60; LAST PAGE * &#x60;5&#x60; MOVE TO PAGE SENDING * &#x60;6&#x60; CURRENT PAGE 
    /// </summary>
    [DataContract]
    public partial class ApiPaginationAction : IEquatable<ApiPaginationAction>
    { 
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ApiPaginationAction {\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((ApiPaginationAction)obj);
        }

        /// <summary>
        /// Returns true if ApiPaginationAction instances are equal
        /// </summary>
        /// <param name="other">Instance of ApiPaginationAction to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ApiPaginationAction other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return false;
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(ApiPaginationAction left, ApiPaginationAction right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ApiPaginationAction left, ApiPaginationAction right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
