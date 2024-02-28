using System;
using System.Collections.Generic;
namespace milescarrental.Application.Models
{
    public class Product
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public IList<Price> PriceList { get; set; }
    }
}
