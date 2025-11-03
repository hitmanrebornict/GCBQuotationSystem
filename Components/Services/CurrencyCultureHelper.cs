using System.Globalization;

namespace GCBQuotationSystem.Components.Services
{
    public static class CurrencyCultureHelper
    {
        /// <summary>
        /// Gets the appropriate CultureInfo based on the currency code
        /// </summary>
        /// <param name="currencyCode">The currency code (e.g., "GBP", "EUR")</param>
        /// <returns>CultureInfo appropriate for the currency</returns>
        public static CultureInfo GetCultureForCurrency(string currencyCode)
        {
            return currencyCode?.ToUpper() switch
            {
                "EUR" => new CultureInfo("de-DE"), // German culture for EUR
                "GBP" => new CultureInfo("en-GB"), // British culture for GBP
                "USD" => new CultureInfo("en-US"), // US culture for USD
                "CAD" => new CultureInfo("en-CA"),
                _ => new CultureInfo("en-GB") // Default to British culture
            };
        }

        /// <summary>
        /// Gets the currency symbol for a given currency code
        /// </summary>
        /// <param name="currencyCode">The currency code</param>
        /// <returns>The currency symbol</returns>
        public static string GetCurrencySymbol(string currencyCode)
        {
            var culture = GetCultureForCurrency(currencyCode);
            return culture.NumberFormat.CurrencySymbol;
        }

        /// <summary>
        /// Formats a decimal value as currency using the appropriate culture
        /// </summary>
        /// <param name="value">The decimal value to format</param>
        /// <param name="currencyCode">The currency code</param>
        /// <returns>Formatted currency string</returns>
        public static string FormatCurrency(decimal value, string currencyCode)
        {
            var culture = GetCultureForCurrency(currencyCode);
            return value.ToString("C0", culture);
        }
    }
}

