
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
    /// Corresponde al filtro que será aplicado al sistema, se deben usar caracteres comodines % ? dado que se expondrá en un like de base de datos
    /// </summary>
    [DataContract]
    public partial class ApiPaginationFilter : IEquatable<ApiPaginationFilter>
    { 
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ApiPaginationFilter {\n");
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
            return obj.GetType() == GetType() && Equals((ApiPaginationFilter)obj);
        }

        /// <summary>
        /// Returns true if ApiPaginationFilter instances are equal
        /// </summary>
        /// <param name="other">Instance of ApiPaginationFilter to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ApiPaginationFilter other)
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

        public static bool operator ==(ApiPaginationFilter left, ApiPaginationFilter right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ApiPaginationFilter left, ApiPaginationFilter right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}