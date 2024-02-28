using milescarrental.Infrastructure.Caching;

namespace milescarrental.Infrastructure.Domain.ForeignExchange
{
    public class ConversionRatesCacheKey : ICacheKey<ConversionRatesCache>
    {
        public string CacheKey => "ConversionRatesCache";
    }
}