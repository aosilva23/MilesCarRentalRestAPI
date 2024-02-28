
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
    /// 
    /// </summary>
    [DataContract]
    public partial class ApiRequestResponse : IEquatable<ApiRequestResponse>
    { 
        /// <summary>
        /// Gets or Sets Code
        /// </summary>
        [DataMember(Name="code")]
        public long? Code { get; set; }

        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        [DataMember(Name="type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or Sets Message
        /// </summary>
        [DataMember(Name="message")]
        public string Message { get; set; }

        /// <summary>
        /// Gets or Sets SystemMessage
        /// </summary>
        [DataMember(Name="systemMessage")]
        public string SystemMessage { get; set; }

        /// <summary>
        /// Gets or Sets StackTrace
        /// </summary>
        [DataMember(Name="stackTrace")]
        public string StackTrace { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ApiRequestResponse {\n");
            sb.Append("  Code: ").Append(Code).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  Message: ").Append(Message).Append("\n");
            sb.Append("  SystemMessage: ").Append(SystemMessage).Append("\n");
            sb.Append("  StackTrace: ").Append(StackTrace).Append("\n");
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
            return obj.GetType() == GetType() && Equals((ApiRequestResponse)obj);
        }

        /// <summary>
        /// Returns true if ApiRequestResponse instances are equal
        /// </summary>
        /// <param name="other">Instance of ApiRequestResponse to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ApiRequestResponse other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Code == other.Code ||
                    Code != null &&
                    Code.Equals(other.Code)
                ) && 
                (
                    Type == other.Type ||
                    Type != null &&
                    Type.Equals(other.Type)
                ) && 
                (
                    Message == other.Message ||
                    Message != null &&
                    Message.Equals(other.Message)
                ) && 
                (
                    SystemMessage == other.SystemMessage ||
                    SystemMessage != null &&
                    SystemMessage.Equals(other.SystemMessage)
                ) && 
                (
                    StackTrace == other.StackTrace ||
                    StackTrace != null &&
                    StackTrace.Equals(other.StackTrace)
                );
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
                    if (Code != null)
                    hashCode = hashCode * 59 + Code.GetHashCode();
                    if (Type != null)
                    hashCode = hashCode * 59 + Type.GetHashCode();
                    if (Message != null)
                    hashCode = hashCode * 59 + Message.GetHashCode();
                    if (SystemMessage != null)
                    hashCode = hashCode * 59 + SystemMessage.GetHashCode();
                    if (StackTrace != null)
                    hashCode = hashCode * 59 + StackTrace.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(ApiRequestResponse left, ApiRequestResponse right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ApiRequestResponse left, ApiRequestResponse right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
