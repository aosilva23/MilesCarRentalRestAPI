using System.Collections.Generic;
using milescarrental.Domain.ForeignExchange;

namespace milescarrental.Infrastructure.Domain.ForeignExchange
{
    public class ConversionRatesCache
    {
        public List<ConversionRate> Rates { get; }

        public ConversionRatesCache(List<ConversionRate> rates)
        {
            this.Rates = rates;
        }
    }
}