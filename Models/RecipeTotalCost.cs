namespace GCBQuotationSystem.Models
{
	public class RecipeTotalCost
	{
	
        public QuotationRecipe QuotationRecipe1 { get; set; }
        public string Quarter { get; set; }
        public decimal TotalCost { get; set; }
		
		public RecipeIngredient RecipeIngredientList { get; set; }

		public List<RawMaterialPriceDetail> RawMaterialPriceDetailList { get; set; }

	}
}
