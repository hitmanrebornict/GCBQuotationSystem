﻿
using GCBQuotationSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace GCBQuotationSystem.Components.Services
{
	public class QuoteStatisticsResult
	{
		public int TotalQuotes { get; set; }
		public int CompletedQuotes { get; set; }
		public int InProgressQuotes { get; set; }
	}

	public class QuoteDataServices
	{
		private readonly QuotationSystemContext _dbContext;
		public QuoteDataServices(QuotationSystemContext dbContext)
		{
			_dbContext = dbContext;
		}

		// This will hold the recipeCostMatrix data temporarily
		public List<RecipeCostMatrix> RecipeCostMatrix { get; set; } = new List<RecipeCostMatrix>();

		public List<RecipeTotalCost> OriginalRecipeTotalList { get; set; } = new List<RecipeTotalCost>();

		// You can also add other data you want to pass to the next page
		public List<RecipeTotalCost> RecipeTotalCostList { get; set; } = new List<RecipeTotalCost>();



		public Quote newQuotation { get; set; }

		public decimal CalculateMassTotal(Decimal liquor, Decimal LifeGBP)
		{

			return LifeGBP * liquor;
		}

		public decimal CalculateButterTotal(Decimal butter, Decimal LifeGBP)
		{

			return LifeGBP * butter;
		}

		public async Task<List<RecipeCostMatrix>> PrepareCostMatrix(List<RecipeTotalCost> recipeTotalCostList)
		{

			List<RecipeCostMatrix> recipeCostMatrix = new List<RecipeCostMatrix>();
			// Reshape data: Group by RecipeCode and PeriodMonth
			var groupedByRecipe = recipeTotalCostList
				.GroupBy(x => x.QuotationRecipe1.Recipe.RecipeCode)
				.ToList();

			// For each RecipeCode, get its PeriodMonths and Cost
			foreach (var recipeGroup in groupedByRecipe)
			{
				var recipeCostRow = new RecipeCostMatrix
				{
					RecipeCode = recipeGroup.Key,
					PeriodCosts = recipeGroup
							.GroupBy(x => x.QuotationRecipe1.PeriodMonth)
							.ToDictionary(g => g.Key.ToString("MMMM yyyy"), g => g.Sum(x => x.TotalCost)),
					//PeriodQuantities = recipeGroup
					//		.GroupBy(x => x.QuotationRecipe1.PeriodMonth)
					//		.ToDictionary(g => g.Key.ToString("MMMM yyyy"), g => g.Sum(x => x.QuotationRecipe1.Quantity)),

				};
				recipeCostMatrix.Add(recipeCostRow);
			}

			return recipeCostMatrix;
		}


		public async Task<List<RecipeCostMatrix>> PrepareQuarterCostMatrix(List<RecipeTotalCost> recipeTotalCostList)
		{
			List<RecipeCostMatrix> recipeCostMatrix = new List<RecipeCostMatrix>();

			// Reshape data: Group by RecipeCode and Quarter
			var groupedByRecipe = recipeTotalCostList
				.GroupBy(x => x.QuotationRecipe1.Recipe.RecipeCode)
				.ToList();

			// For each RecipeCode, get its Quarters and Highest Cost in that Quarter
			foreach (var recipeGroup in groupedByRecipe)
			{
				var recipeCostRow = new RecipeCostMatrix
				{
					RecipeCode = recipeGroup.Key,
					PeriodCosts = recipeGroup
						.GroupBy(x => x.Quarter) // Group by quarter
						.ToDictionary(
							g => g.Key,
							g => g.Max(x => x.TotalCost) // Get the highest cost in the quarter
						),
					//PeriodQuantities = recipeGroup
					//	.GroupBy(x => x.Quarter) // Group by quarter
					//	.ToDictionary(
					//		g => g.Key,
					//		g => g.Sum(x => x.QuotationRecipe1.Quantity) // Sum quantities per quarter
					//	),
				};

				recipeCostMatrix.Add(recipeCostRow);
			}

			return recipeCostMatrix;
		}

		public async Task<List<RecipeCostMatrix>> PrepareTerminalCostMatrix(List<RecipeTotalCost> recipeTotalCostList)
		{
			List<RecipeCostMatrix> recipeCostMatrix = new List<RecipeCostMatrix>();

			// Reshape data: Group by RecipeCode and Terminal
			var groupedByRecipe = recipeTotalCostList
				.GroupBy(x => x.QuotationRecipe1.Recipe.RecipeCode)
				.ToList();

			// For each RecipeCode, get its Terminals and Highest Cost in that Terminal
			foreach (var recipeGroup in groupedByRecipe)
			{
				var recipeCostRow = new RecipeCostMatrix
				{
					RecipeCode = recipeGroup.Key,
					PeriodCosts = recipeGroup
						.GroupBy(x => x.QuotationRecipe1.QuotationTerminalCost.TerminalName)
						.ToDictionary(
							g => g.Key,
							g => g.Max(x => x.TotalCost) // Get the highest cost in the terminal
						),
				};

				recipeCostMatrix.Add(recipeCostRow);
			}

			return recipeCostMatrix;
		}

		public async Task<List<RecipeCostMatrix>> PrepareAverageCostMatrix(List<RecipeTotalCost> recipeTotalCostList)
		{
			List<RecipeCostMatrix> recipeCostMatrix = new List<RecipeCostMatrix>();

			// Reshape data: Group by RecipeCode and Period
			var groupedByRecipe = recipeTotalCostList
				.GroupBy(x => x.QuotationRecipe1.Recipe.RecipeCode)
				.ToList();

			// For each RecipeCode, get its Periods and Average Cost in that Period
			foreach (var recipeGroup in groupedByRecipe)
			{
				var recipeCostRow = new RecipeCostMatrix
				{
					RecipeCode = recipeGroup.Key,
					PeriodCosts = recipeGroup
						.GroupBy(x => x.QuotationRecipe1.PeriodMonth.ToString("MMMM yyyy"))
						.ToDictionary(
							g => g.Key,
							g => g.Average(x => x.TotalCost) // Get the average cost in the period
						),
				};

				recipeCostMatrix.Add(recipeCostRow);
			}

			return recipeCostMatrix;
		}

		public async Task<List<RecipeCostMatrix>> PrepareAverageQuarterCostMatrix(List<RecipeTotalCost> recipeTotalCostList)
		{
			List<RecipeCostMatrix> recipeCostMatrix = new List<RecipeCostMatrix>();

			// Reshape data: Group by RecipeCode and Quarter
			var groupedByRecipe = recipeTotalCostList
				.GroupBy(x => x.QuotationRecipe1.Recipe.RecipeCode)
				.ToList();

			// For each RecipeCode, get its Quarters and Average Cost in that Quarter
			foreach (var recipeGroup in groupedByRecipe)
			{
				var recipeCostRow = new RecipeCostMatrix
				{
					RecipeCode = recipeGroup.Key,
					PeriodCosts = recipeGroup
						.GroupBy(x => x.Quarter) // Group by quarter
						.ToDictionary(
							g => g.Key,
							g => g.Average(x => x.TotalCost) // Get the average cost in the quarter
						),
				};

				recipeCostMatrix.Add(recipeCostRow);
			}

			return recipeCostMatrix;
		}

		public async Task<List<RecipeCostMatrix>> PrepareAverageTerminalCostMatrix(List<RecipeTotalCost> recipeTotalCostList)
		{
			List<RecipeCostMatrix> recipeCostMatrix = new List<RecipeCostMatrix>();

			// Reshape data: Group by RecipeCode and Terminal
			var groupedByRecipe = recipeTotalCostList
				.GroupBy(x => x.QuotationRecipe1.Recipe.RecipeCode)
				.ToList();

			// For each RecipeCode, get its Terminals and Average Cost in that Terminal
			foreach (var recipeGroup in groupedByRecipe)
			{
				var recipeCostRow = new RecipeCostMatrix
				{
					RecipeCode = recipeGroup.Key,
					PeriodCosts = recipeGroup
						.GroupBy(x => x.QuotationRecipe1.QuotationTerminalCost.TerminalName)
						.ToDictionary(
							g => g.Key,
							g => g.Average(x => x.TotalCost) // Get the average cost in the terminal
						),
				};

				recipeCostMatrix.Add(recipeCostRow);
			}

			return recipeCostMatrix;
		}


		public List<RecipeTotalCost> ConvertRecipeCostListToPreferredUnit(List<RecipeTotalCost> originalList, decimal weightRatio)
		{
		
			return originalList.Select(original =>
			{
				var originalRecipe = original.QuotationRecipe1;

				var convertedRecipe = new QuotationRecipe
				{
					Recipe = originalRecipe.Recipe,
					RecipeId = originalRecipe.RecipeId,
					PeriodMonth = originalRecipe.PeriodMonth,
					Quantity = originalRecipe.Quantity,
					QuoteId = originalRecipe.QuoteId,
					QuotationRecipeId = originalRecipe.QuotationRecipeId,

					QuotationTerminalCost = originalRecipe.QuotationTerminalCost != null
						? new QuotationTerminalCost
						{
							TerminalName = originalRecipe.QuotationTerminalCost.TerminalName,
							TerminalPeriod = originalRecipe.QuotationTerminalCost.TerminalPeriod,
							LifeGbp = originalRecipe.QuotationTerminalCost.LifeGbp,
							Liquor = originalRecipe.QuotationTerminalCost.Liquor,
							Butter = originalRecipe.QuotationTerminalCost.Butter,
							Powder = originalRecipe.QuotationTerminalCost.Powder,

						}
						: null,

					QuotationPremiumCosts = originalRecipe.QuotationPremiumCosts?
						.Select(p => new QuotationPremiumCost
						{
							PremiumName = p.PremiumName,
							Cost = p.Cost,
							CostAmount = p.CostAmount * weightRatio
						}).ToList() ?? new List<QuotationPremiumCost>(),

					QuotationRawMaterialCosts = originalRecipe.QuotationRawMaterialCosts?
						.Select(r => new QuotationRawMaterialCost
						{
							MaterialName = r.MaterialName,
							Cost = r.Cost,
							CostAmount = r.CostAmount * weightRatio
						}).ToList() ?? new List<QuotationRawMaterialCost>(),

					QuotationPackagingCost = originalRecipe.QuotationPackagingCost != null
						? new QuotationPackagingCost
						{
							PackagingName = originalRecipe.QuotationPackagingCost.PackagingName,
							Cost = originalRecipe.QuotationPackagingCost.Cost,
							CostAmount = originalRecipe.QuotationPackagingCost.CostAmount * weightRatio
						}
						: null,

					QuotationDeliveryCost = originalRecipe.QuotationDeliveryCost != null
						? new QuotationDeliveryCost
						{
							DeliveryName = originalRecipe.QuotationDeliveryCost.DeliveryName,
							//Cost = originalRecipe.QuotationDeliveryCost.Cost,
							CostAmount = originalRecipe.QuotationDeliveryCost.CostAmount
						}
						: null,

					QuotationProductionCost = originalRecipe.QuotationProductionCost != null
						? new QuotationProductionCost
						{
							ProductType = originalRecipe.QuotationProductionCost.ProductType,
							ProductTypeCost = originalRecipe.QuotationProductionCost.ProductTypeCost
						}
						: null,

					QuotationFinancialCost = originalRecipe.QuotationFinancialCost != null
						? new QuotationFinancialCost
						{
							InterestRate = originalRecipe.QuotationFinancialCost.InterestRate,
							FinanceDays = originalRecipe.QuotationFinancialCost.FinanceDays
						}
						: null,

					QuotationAdditionalCosts = originalRecipe.QuotationAdditionalCosts?
						.Select(a => new QuotationAdditionalCost
						{
							CostName = a.CostName,
							Cost = a.Cost,
							CostAmount = a.CostAmount * weightRatio
						}).ToList() ?? new List<QuotationAdditionalCost>()

					
				};


				var totalCost = CalculateTotalCost(convertedRecipe);


				Console.WriteLine("Total Cost: " + totalCost);

				return new RecipeTotalCost
				{
					QuotationRecipe1 = convertedRecipe,
					Quarter = original.Quarter,
					RawMaterialPriceDetailList = original.RawMaterialPriceDetailList,
					TotalCost = totalCost
				};
			}).ToList();
		}


		public List<RecipeTotalCost> RevertToOriginalUnit(List<RecipeTotalCost> convertedList, decimal weightRatio)
		{

			// Calculate the inverse ratio to convert back
			decimal inverseRatio = 1 / weightRatio;

			// Use the same conversion logic but with the inverse ratio
			return ConvertRecipeCostListToPreferredUnit(convertedList, inverseRatio);
		}

		public List<RecipeTotalCost> RevertToOriginalUnitCompletedPage(List<RecipeTotalCost> convertedList, decimal weightRatio)
		{

			// Calculate the inverse ratio to convert back
			decimal inverseRatio = 1 / weightRatio;

			// Use the same conversion logic but with the inverse ratio
			return ConvertRecipeCostListToPreferredUnit(convertedList, inverseRatio);
		}

		public List<RecipeTotalCost> ConvertRecipeCostListToPreferredUnitCompletedPage(List<RecipeTotalCost> originalList, decimal weightRatio)
		{
			Console.WriteLine("Testing");
			return originalList.Select(original =>
			{
				var originalRecipe = original.QuotationRecipe1;

				var convertedRecipe = new QuotationRecipe
				{
					QuotationRecipeId = originalRecipe.QuotationRecipeId,
					Recipe = originalRecipe.Recipe,
					RecipeId = originalRecipe.RecipeId,
					PeriodMonth = originalRecipe.PeriodMonth,
					Quantity = originalRecipe.Quantity,
					QuoteId = originalRecipe.QuoteId,

					QuotationTerminalCost = originalRecipe.QuotationTerminalCost != null
						? new QuotationTerminalCost
						{
							Id = originalRecipe.QuotationTerminalCost.Id,
							TerminalName = originalRecipe.QuotationTerminalCost.TerminalName,
							TerminalPeriod = originalRecipe.QuotationTerminalCost.TerminalPeriod,
							LifeGbp = originalRecipe.QuotationTerminalCost.LifeGbp,
							Liquor = originalRecipe.QuotationTerminalCost.Liquor,
							Butter = originalRecipe.QuotationTerminalCost.Butter,
							Powder = originalRecipe.QuotationTerminalCost.Powder,
							QuotationRecipeId = originalRecipe.QuotationTerminalCost.QuotationRecipeId
						}
						: null,

					QuotationPremiumCosts = originalRecipe.QuotationPremiumCosts?
						.Select(p => new QuotationPremiumCost
						{
							Id = p.Id,
							PremiumName = p.PremiumName,
							Cost = p.Cost,
							CostAmount = p.CostAmount * weightRatio,
							QuotationRecipeId = p.QuotationRecipeId
						}).ToList() ?? new List<QuotationPremiumCost>(),

					QuotationRawMaterialCosts = originalRecipe.QuotationRawMaterialCosts?
						.Select(r => new QuotationRawMaterialCost
						{
							Id = r.Id,
							MaterialName = r.MaterialName,
							Cost = r.Cost,
							CostAmount = r.CostAmount * weightRatio,
							QuotationRecipeId = r.QuotationRecipeId
						}).ToList() ?? new List<QuotationRawMaterialCost>(),

					QuotationPackagingCost = originalRecipe.QuotationPackagingCost != null
						? new QuotationPackagingCost
						{
							Id = originalRecipe.QuotationPackagingCost.Id,
							PackagingName = originalRecipe.QuotationPackagingCost.PackagingName,
							Cost = originalRecipe.QuotationPackagingCost.Cost,
							CostAmount = originalRecipe.QuotationPackagingCost.CostAmount * weightRatio,
							QuotationRecipeId = originalRecipe.QuotationPackagingCost.QuotationRecipeId
						}
						: null,

					QuotationDeliveryCost = originalRecipe.QuotationDeliveryCost != null
						? new QuotationDeliveryCost
						{
							Id = originalRecipe.QuotationDeliveryCost.Id,
							DeliveryName = originalRecipe.QuotationDeliveryCost.DeliveryName,
							CostAmount = originalRecipe.QuotationDeliveryCost.CostAmount,
							QuotationRecipeId = originalRecipe.QuotationDeliveryCost.QuotationRecipeId
						}
						: null,

					QuotationAdditionalCosts = originalRecipe.QuotationAdditionalCosts?
						.Select(a => new QuotationAdditionalCost
						{
							Id = a.Id,
							CostName = a.CostName,
							Cost = a.Cost,
							CostAmount = a.CostAmount * weightRatio,
							QuotationRecipeId = a.QuotationRecipeId
						}).ToList() ?? new List<QuotationAdditionalCost>()
				};

				var totalCost = (convertedRecipe.QuotationPremiumCosts?.Sum(x => x.CostAmount) ?? 0)
					+ (convertedRecipe.QuotationRawMaterialCosts?.Sum(x => x.CostAmount) ?? 0)
					+ (convertedRecipe.QuotationPackagingCost?.CostAmount ?? 0)
					+ (convertedRecipe.QuotationDeliveryCost?.CostAmount ?? 0)
					+ (convertedRecipe.QuotationAdditionalCosts?.Sum(x => x.CostAmount) ?? 0);

				Console.WriteLine("Total Cost: " + totalCost);

				return new RecipeTotalCost
				{
					QuotationRecipe1 = convertedRecipe,
					Quarter = original.Quarter,
					RawMaterialPriceDetailList = original.RawMaterialPriceDetailList,
					TotalCost = totalCost
				};
			}).ToList();
		}



		/// <summary>
		/// Clears all data related to recipe costs and calculations.
		/// This includes RecipeTotalCostList, RecipeCostMatrix, and any other related data.
		/// </summary>
		public void ClearAllRecipeData()
		{
			// Clear the main recipe cost list
			RecipeTotalCostList = new List<RecipeTotalCost>();

			// Clear the cost matrix
			RecipeCostMatrix = new List<RecipeCostMatrix>();



		}

		public decimal CalculateTotalCost(QuotationRecipe quotationRecipe) {

			// Calculate total raw material cost first
			var totalRawMaterialCost = quotationRecipe.QuotationRawMaterialCosts.Sum(x => x.CostAmount);

			// Find and recalculate Yield cost in QuotationAdditionalCosts
			var yieldCost = quotationRecipe.QuotationAdditionalCosts.FirstOrDefault(x => x.CostName == "Yield");
			if (yieldCost != null)
			{
				yieldCost.CostAmount = yieldCost.Cost / 100 * totalRawMaterialCost;
			}

			// Calculate total cost excluding ORD Rebate first
			var totalCostExcludingORDRebate = (quotationRecipe.QuotationPremiumCosts?.Sum(x => x.CostAmount) ?? 0) +
				totalRawMaterialCost +
				(quotationRecipe.QuotationPackagingCost?.CostAmount ?? 0) +
				(quotationRecipe.QuotationDeliveryCost?.CostAmount ?? 0) +
				(quotationRecipe.QuotationAdditionalCosts?.Where(x => x.CostName != "ORD Rebate").Sum(x => x.CostAmount) ?? 0) +
				(quotationRecipe.QuotationProductionCost?.ProductTypeCost ?? 0);

			// Calculate ORD Rebate based on total cost excluding itself (acts as a discount)
			var ordRebateCost = quotationRecipe.QuotationAdditionalCosts.FirstOrDefault(x => x.CostName == "ORD Rebate");
			if (ordRebateCost != null)
			{	
				ordRebateCost.CostAmount = (ordRebateCost.Cost / 100 * totalCostExcludingORDRebate);
			}

			// Calculate final total cost including ORD Rebate
			var localTotalCost = totalCostExcludingORDRebate + (ordRebateCost?.CostAmount ?? 0);

			if(quotationRecipe.QuotationFinancialCost != null){
				decimal financialCost = localTotalCost * quotationRecipe.QuotationFinancialCost.InterestRate / 100 * quotationRecipe.QuotationFinancialCost.FinanceDays / 365;
				localTotalCost += financialCost;
			}

			return localTotalCost;
		}

		public decimal CalculatePowderTotal(Decimal powder, Decimal LifeGBP)
		{
			return LifeGBP * powder;
		}

		public async Task<QuoteStatisticsResult> GetQuoteStatisticsAsync()
		{
			var quotes = await _dbContext.Quotes.Include(q => q.Status).ToListAsync();
			int total = quotes.Count;
			int completed = quotes.Count(q => q.Status.StatusName == "Completed");
			int inProgress = quotes.Count(q => q.Status.StatusName == "Created" || q.Status.StatusName == "Confirmed");
			return new QuoteStatisticsResult
			{
				TotalQuotes = total,
				CompletedQuotes = completed,
				InProgressQuotes = inProgress
			};
		}
	}
}
