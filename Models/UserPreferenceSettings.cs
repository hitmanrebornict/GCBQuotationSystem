using System.Globalization;

namespace GCBQuotationSystem.Models
{
	public class UserPreferenceSettings
	{
		public string Currency { get; set; } = "GBP";  // Default value
		public string WeightUnit { get; set; } = "per tonne"; // Default value

		public CultureInfo CultureInfo { get; set; } = new CultureInfo("en-GB");

		public decimal WeightRatio { get; set; } 

		// Options for Currency and Weight Units
		public List<string> AvailableCurrencies { get; set; } = new List<string> {  "EUR", "GBP" };
		public List<string> AvailableWeightUnits { get; set; } = new List<string> { "per tonne", "per kg", "per 100 kg" };

		public string PrintedWeightUnit { get; set; } = null;
		public void UpdateCultureBasedOnCurrency()
		{
			CultureInfo = Currency switch
			{
				"EUR" => new CultureInfo("de-DE"), // or "fr-FR", etc.
				"GBP" => new CultureInfo("en-GB"),
				_ => CultureInfo.InvariantCulture
			};

			CultureInfo.DefaultThreadCurrentCulture = CultureInfo;
			CultureInfo.DefaultThreadCurrentUICulture = CultureInfo;
		}

		public decimal GetWeightRatio()
		{
			switch (WeightUnit)
			{
				case "per tonne":
					WeightRatio = 1.0m;   // 1 tonne = 1.0
					break;

				case "per kg":
					WeightRatio = 0.001m; // 1 kg = 0.001 tonne
					break;

				case "per 100 kg":
					WeightRatio = 0.1m;  // 100 kg = 0.1 tonne
					break;

				default:
					throw new ArgumentException("Invalid weight unit selected.");
			}

			return WeightRatio;
		}
	}
}
