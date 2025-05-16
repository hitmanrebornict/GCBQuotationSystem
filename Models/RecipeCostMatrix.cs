using Microsoft.CodeAnalysis.CSharp;

namespace GCBQuotationSystem.Models
{
	public class RecipeCostMatrix
	{
		public string RecipeCode { get; set; }
        public Dictionary<string, decimal> PeriodCosts { get; set; } = new Dictionary<string, decimal>();

		public Dictionary<string, decimal> PeriodQuantities { get; set; } = new();

	}


}
