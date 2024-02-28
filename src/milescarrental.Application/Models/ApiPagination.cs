
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
    public partial class ApiPagination : IEquatable<ApiPagination>
    { 
        /// <summary>
        /// Gets or Sets Action
        /// </summary>
        [DataMember(Name="action")]
        public ApiPaginationAction Action { get; set; }

        /// <summary>
        /// Gets or Sets Limit
        /// </summary>
        [DataMember(Name="limit")]
        public int Limit { get; set; }

        /// <summary>
        /// Gets or Sets Offset
        /// </summary>
        [DataMember(Name="offset")]
        public ApiPaginationOffset Offset { get; set; }

        /// <summary>
        /// Gets or Sets MoveToPage
        /// </summary>
        [DataMember(Name="moveToPage")]
        public ApiPaginationMoveToPage MoveToPage { get; set; }

        /// <summary>
        /// Gets or Sets CurrentPage
        /// </summary>
        [DataMember(Name="currentPage")]
        public int CurrentPage { get; set; }

        /// <summary>
        /// Gets or Sets TotalPages
        /// </summary>
        [DataMember(Name="totalPages")]
        public ApiPaginationTotalPages TotalPages { get; set; }

        /// <summary>
        /// Gets or Sets OrderColumn
        /// </summary>
        [DataMember(Name="orderColumn")]
        public int OrderColumn { get; set; }

        /// <summary>
        /// Gets or Sets OrderDirection
        /// </summary>
        [DataMember(Name="orderDirection")]
        public int OrderDirection { get; set; }

        /// <summary>
        /// Gets or Sets Filter
        /// </summary>
        [DataMember(Name="filter")]
        public ApiPaginationFilter Filter { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ApiPagination {\n");
            sb.Append("  Action: ").Append(Action).Append("\n");
            sb.Append("  Limit: ").Append(Limit).Append("\n");
            sb.Append("  Offset: ").Append(Offset).Append("\n");
            sb.Append("  MoveToPage: ").Append(MoveToPage).Append("\n");
            sb.Append("  CurrentPage: ").Append(CurrentPage).Append("\n");
            sb.Append("  TotalPages: ").Append(TotalPages).Append("\n");
            sb.Append("  OrderColumn: ").Append(OrderColumn).Append("\n");
            sb.Append("  OrderDirection: ").Append(OrderDirection).Append("\n");
            sb.Append("  Filter: ").Append(Filter).Append("\n");
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
            return obj.GetType() == GetType() && Equals((ApiPagination)obj);
        }

        /// <summary>
        /// Returns true if ApiPagination instances are equal
        /// </summary>
        /// <param name="other">Instance of ApiPagination to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ApiPagination other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Action == other.Action ||
                    Action != null &&
                    Action.Equals(other.Action)
                ) && 
                (
                    Limit == other.Limit ||
                    Limit != null &&
                    Limit.Equals(other.Limit)
                ) && 
                (
                    Offset == other.Offset ||
                    Offset != null &&
                    Offset.Equals(other.Offset)
                ) && 
                (
                    MoveToPage == other.MoveToPage ||
                    MoveToPage != null &&
                    MoveToPage.Equals(other.MoveToPage)
                ) && 
                (
                    CurrentPage == other.CurrentPage ||
                    CurrentPage != null &&
                    CurrentPage.Equals(other.CurrentPage)
                ) && 
                (
                    TotalPages == other.TotalPages ||
                    TotalPages != null &&
                    TotalPages.Equals(other.TotalPages)
                ) && 
                (
                    OrderColumn == other.OrderColumn ||
                    OrderColumn != null &&
                    OrderColumn.Equals(other.OrderColumn)
                ) && 
                (
                    OrderDirection == other.OrderDirection ||
                    OrderDirection != null &&
                    OrderDirection.Equals(other.OrderDirection)
                ) && 
                (
                    Filter == other.Filter ||
                    Filter != null &&
                    Filter.Equals(other.Filter)
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
                    if (Action != null)
                    hashCode = hashCode * 59 + Action.GetHashCode();
                    if (Limit != null)
                    hashCode = hashCode * 59 + Limit.GetHashCode();
                    if (Offset != null)
                    hashCode = hashCode * 59 + Offset.GetHashCode();
                    if (MoveToPage != null)
                    hashCode = hashCode * 59 + MoveToPage.GetHashCode();
                    if (CurrentPage != null)
                    hashCode = hashCode * 59 + CurrentPage.GetHashCode();
                    if (TotalPages != null)
                    hashCode = hashCode * 59 + TotalPages.GetHashCode();
                    if (OrderColumn != null)
                    hashCode = hashCode * 59 + OrderColumn.GetHashCode();
                    if (OrderDirection != null)
                    hashCode = hashCode * 59 + OrderDirection.GetHashCode();
                    if (Filter != null)
                    hashCode = hashCode * 59 + Filter.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(ApiPagination left, ApiPagination right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ApiPagination left, ApiPagination right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
