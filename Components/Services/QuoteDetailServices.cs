using DocumentFormat.OpenXml.InkML;
using GCBQuotationSystem.Components.Pages.MasterDataPages;
using GCBQuotationSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using Newtonsoft.Json;

namespace GCBQuotationSystem.Components.Services
{
	public class QuoteDetailServices
	{
		private QuotationSystemContext _dbContext;

		public QuoteDetailServices(QuotationSystemContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task addQuoteIntoDatabase(Quote selectedQuote, List<QuotationRecipe> selectedQuotationRecipe, string currencyCode, string quoteRemark, string customerRemark)
		{

			// Find the matching currency from the database based on the provided currency code
			var currency = await _dbContext.Currencies.FirstOrDefaultAsync(c => c.CurrencyCode == currencyCode);
			if (currency != null)
			{
				selectedQuote.Currency = currency;
				selectedQuote.CurrencyId = currency.CurrencyId;
			}
			else
			{
				// Fallback if the currency code doesn't exist in the database
				selectedQuote.Currency.CurrencyCode = currencyCode;
			}


			foreach (var recipe in selectedQuotationRecipe)
			{
				if (recipe.QuotationTerminalCost != null)
					_dbContext.Attach(recipe.QuotationTerminalCost);

				if (recipe.QuotationPackagingCost != null)
					_dbContext.Attach(recipe.QuotationPackagingCost);

				if (recipe.QuotationDeliveryCost != null)
					_dbContext.Attach(recipe.QuotationDeliveryCost);

				if (recipe.QuotationFinancialCost != null)
					_dbContext.Attach(recipe.QuotationFinancialCost);

				if (recipe.QuotationProductionCost != null)
					_dbContext.Attach(recipe.QuotationProductionCost);

				// Ensure raw material and premium costs are added as new (if they are new entries)
				foreach (var rawMaterialCost in recipe.QuotationRawMaterialCosts)
				{
					// Reset the ID to ensure it's treated as a new entry
					_dbContext.Entry(rawMaterialCost).State = EntityState.Added;
				}

				// Ensure raw material and premium costs are added as new (if they are new entries)
				foreach (var additionalCost in recipe.QuotationAdditionalCosts)
				{
					_dbContext.Entry(additionalCost).State = EntityState.Added;
				}

				foreach (var premiumCost in recipe.QuotationPremiumCosts)
				{
					_dbContext.Entry(premiumCost).State = EntityState.Added;
				}

				//// Mark the QuotationRecipe as new
				_dbContext.Entry(recipe).State = EntityState.Added;
			}


			selectedQuote.QuotationRecipes = selectedQuotationRecipe;
			selectedQuote.Remark = quoteRemark;
			selectedQuote.CustomerRemark = customerRemark;

			await _dbContext.Quotes.AddAsync(selectedQuote);
			await _dbContext.SaveChangesAsync();
		}

		//public async Task updateQuoteInDatabase(Quote selectedQuote, List<QuotationRecipe> selectedQuotationRecipe)
		//{

		//	// Detach any previously tracked entities to avoid the tracking conflict.
		//	foreach (var recipe in selectedQuotationRecipe)
		//	{
		//		var existingRecipeEntry = _dbContext.Entry(recipe.Recipe);
		//		if (existingRecipeEntry.State != EntityState.Detached)
		//		{
		//			existingRecipeEntry.State = EntityState.Detached;
		//		}

		//		// Now, attach the updated Recipe entity to the context and mark it as modified.
		//		_dbContext.Entry(recipe.Recipe).State = EntityState.Modified;
		//	}

		//	// Update the QuotationRecipe entities
		//	_dbContext.QuotationRecipes.UpdateRange(selectedQuotationRecipe);

		//	// Save all changes to the database
		//	await _dbContext.SaveChangesAsync();
		//}



		//public async Task updateQuoteInDatabase(Quote selectedQuote, List<QuotationRecipe> selectedQuotationRecipe)
		//{
		//	selectedQuote.Remark = "abc";
		//	selectedQuote.CustomerRemark = "123";

		//	selectedQuote.QuotationRecipes.FirstOrDefault().QuotationPackagingCost.CostAmount = 999999;


		//	_dbContext.Quotes.Update(selectedQuote);
		//	// Save all changes to the database
		//	await _dbContext.SaveChangesAsync();
		//}

		public async Task updateQuoteInDatabase(Quote selectedQuote, List<QuotationRecipe> selectedQuotationRecipe)
		{
			foreach (var updatedRecipe in selectedQuotationRecipe)
			{
				var trackedRecipe = selectedQuote.QuotationRecipes
					.FirstOrDefault(r => r.QuotationRecipeId == updatedRecipe.QuotationRecipeId);

				if (trackedRecipe != null)
				{
					// === Basic Properties ===
					trackedRecipe.Quantity = updatedRecipe.Quantity;
					trackedRecipe.PeriodMonth = updatedRecipe.PeriodMonth;

					// === QuotationPackagingCost ===
					if (trackedRecipe.QuotationPackagingCost != null && updatedRecipe.QuotationPackagingCost != null)
					{
						trackedRecipe.QuotationPackagingCost.PackagingName = updatedRecipe.QuotationPackagingCost.PackagingName;
						trackedRecipe.QuotationPackagingCost.Cost = updatedRecipe.QuotationPackagingCost.Cost;
						trackedRecipe.QuotationPackagingCost.CostAmount = updatedRecipe.QuotationPackagingCost.CostAmount;
					}

					// === QuotationTerminalCost ===
					if (trackedRecipe.QuotationTerminalCost != null && updatedRecipe.QuotationTerminalCost != null)
					{
						trackedRecipe.QuotationTerminalCost.TerminalName = updatedRecipe.QuotationTerminalCost.TerminalName;
						trackedRecipe.QuotationTerminalCost.TerminalPeriod = updatedRecipe.QuotationTerminalCost.TerminalPeriod;
						trackedRecipe.QuotationTerminalCost.LifeGbp = updatedRecipe.QuotationTerminalCost.LifeGbp;
						trackedRecipe.QuotationTerminalCost.Liquor = updatedRecipe.QuotationTerminalCost.Liquor;
						trackedRecipe.QuotationTerminalCost.Butter = updatedRecipe.QuotationTerminalCost.Butter;
						trackedRecipe.QuotationTerminalCost.Powder = updatedRecipe.QuotationTerminalCost.Powder;
					}

					_dbContext.QuotationRawMaterialCosts.RemoveRange(trackedRecipe.QuotationRawMaterialCosts);

					//// === QuotationRawMaterialCosts ===
					trackedRecipe.QuotationRawMaterialCosts = updatedRecipe.QuotationRawMaterialCosts
						.Select(r => new QuotationRawMaterialCost
						{
							MaterialName = r.MaterialName,
							Cost = r.Cost,
							CostAmount = r.CostAmount
						}).ToList();

					_dbContext.QuotationPremiumCosts.RemoveRange(trackedRecipe.QuotationPremiumCosts);

					// === QuotationPremiumCosts ===
					trackedRecipe.QuotationPremiumCosts = updatedRecipe.QuotationPremiumCosts
						.Select(p => new QuotationPremiumCost
						{
							PremiumName = p.PremiumName,
							Cost = p.Cost,
							CostAmount = p.CostAmount
						}).ToList();

					_dbContext.QuotationAdditionalCosts.RemoveRange(trackedRecipe.QuotationAdditionalCosts);

					// === QuotationAdditionalCosts ===
					trackedRecipe.QuotationAdditionalCosts = updatedRecipe.QuotationAdditionalCosts
						.Select(a => new QuotationAdditionalCost
						{
							QuotationRecipeId = a.QuotationRecipeId,
							CostName = a.CostName,
							Cost = a.Cost,
							CostAmount = a.CostAmount
						}).ToList();

					// === QuotationDeliveryCost ===
					if (trackedRecipe.QuotationDeliveryCost != null && updatedRecipe.QuotationDeliveryCost != null)
					{
						trackedRecipe.QuotationDeliveryCost.DeliveryName = updatedRecipe.QuotationDeliveryCost.DeliveryName;
						trackedRecipe.QuotationDeliveryCost.CostAmount = updatedRecipe.QuotationDeliveryCost.CostAmount;
					}

					// === QuotationProductionCost ===
					if (trackedRecipe.QuotationProductionCost != null && updatedRecipe.QuotationProductionCost != null)
					{
						trackedRecipe.QuotationProductionCost.ProductType = updatedRecipe.QuotationProductionCost.ProductType;
						trackedRecipe.QuotationProductionCost.ProductTypeCost = updatedRecipe.QuotationProductionCost.ProductTypeCost;
					}

					// === QuotationFinancialCost ===
					if (trackedRecipe.QuotationFinancialCost != null && updatedRecipe.QuotationFinancialCost != null)
					{
						trackedRecipe.QuotationFinancialCost.InterestRate = updatedRecipe.QuotationFinancialCost.InterestRate;
						trackedRecipe.QuotationFinancialCost.FinanceDays = updatedRecipe.QuotationFinancialCost.FinanceDays;
					}
				}
			}
			_dbContext.Quotes.Update(selectedQuote);
			await _dbContext.SaveChangesAsync();
		}



		public async Task<Quote> duplicateQuoteIntoDatabase(Quote selectedQuote, List<QuotationRecipe> selectedQuotationRecipe)
		{
			var duplicatedQuote = new Quote
			{
				// Copy scalar properties from selectedQuote (not the ID)
				CustomerId = selectedQuote.CustomerId,
				DeliveryDetailId = selectedQuote.DeliveryDetailId,
				CreatedAt = DateTime.Now
				// etc.
			};

			List<QuotationRecipe> duplicatedRecipes = new List<QuotationRecipe>();

			foreach (var recipe in selectedQuotationRecipe)
			{
				var newRecipe = new QuotationRecipe
				{
					// Copy scalar properties
					RecipeId = recipe.RecipeId,
					PeriodMonth = recipe.PeriodMonth,
					Quantity = recipe.Quantity,
					// etc.

					// Clone child collections
					QuotationRawMaterialCosts = recipe.QuotationRawMaterialCosts.Select(rm => new QuotationRawMaterialCost
					{
						Cost = rm.Cost,
						CostAmount = rm.CostAmount,
						MaterialName = rm.MaterialName
						// Copy other fields but not Id
					}).ToList(),

					QuotationAdditionalCosts = recipe.QuotationAdditionalCosts.Select(ac => new QuotationAdditionalCost
					{
						Cost = ac.Cost,
						CostAmount = ac.CostAmount,
						CostName = ac.CostName
						// Copy other fields but not Id
					}).ToList(),

					QuotationPremiumCosts = recipe.QuotationPremiumCosts.Select(pc => new QuotationPremiumCost
					{
						PremiumName =pc.PremiumName,
						Cost = pc.Cost,
						CostAmount = pc.CostAmount,
						// Copy other fields but not Id
					}).ToList(),

					QuotationTerminalCost = new QuotationTerminalCost
					{
						TerminalName = recipe.QuotationTerminalCost.TerminalName,
						TerminalPeriod = recipe.QuotationTerminalCost.TerminalPeriod,
						LifeGbp = recipe.QuotationTerminalCost.LifeGbp,
						Liquor = recipe.QuotationTerminalCost.Liquor,
						Butter = recipe.QuotationTerminalCost.Butter,
						Powder = recipe.QuotationTerminalCost.Powder,
						GhanaLiquor = recipe.QuotationTerminalCost.GhanaLiquor,

						// Copy other fields but not Id
					},

					QuotationDeliveryCost = new QuotationDeliveryCost
					{
						DeliveryName = recipe.QuotationDeliveryCost.DeliveryName,
						CostAmount = recipe.QuotationDeliveryCost.CostAmount
					},

					QuotationPackagingCost = new QuotationPackagingCost
					{
						PackagingName = recipe.QuotationPackagingCost.PackagingName,
						Cost = recipe.QuotationPackagingCost.Cost,
						CostAmount = recipe.QuotationPackagingCost.CostAmount
					}

					
				};

				duplicatedRecipes.Add(newRecipe);
			}

			duplicatedQuote.QuotationRecipes = duplicatedRecipes;

			// Save the new quote and its full hierarchy
			await _dbContext.Quotes.AddAsync(duplicatedQuote);
			await _dbContext.SaveChangesAsync();

			return duplicatedQuote;
		}


		public async Task<List<Quote>> GetQuotesAsync()
		{
			return await _dbContext.Quotes
							.Include(q => q.Customer)
							.Include(q => q.Status)
							.Include(q => q.QuotationRecipes)
								.ThenInclude(q => q.Recipe)
							.OrderByDescending(q => q.QuoteId)
							.AsNoTracking().ToListAsync();
		}

		public async Task<List<Quote>> GetActiveQuotesAsync()
		{
			return await _dbContext.Quotes
							.Include(q => q.Customer)
							.Include(q => q.Status)
							.Include(q => q.QuotationRecipes)
								.ThenInclude(q => q.Recipe)
							.Where(q => q.StatusId == 1 || q.StatusId == 2)
							.OrderByDescending(q => q.QuoteId)
							.AsNoTracking().ToListAsync();
		}

		public async Task<List<Quote>> GetArchivedQuotesAsync()
		{
			return await _dbContext.Quotes
							.Include(q => q.Customer)
							.Include(q => q.Status)
							.Include(q => q.QuotationRecipes)
								.ThenInclude(q => q.Recipe)
							.Where(q => q.StatusId == 3 || q.StatusId == 4)
							.OrderByDescending(q => q.QuoteId)
							.AsNoTracking().ToListAsync();
		}


		public async Task<List<RecipeIngredient>> GetRecipeIngredients(int selectedRecipeID)
		{
			return await _dbContext.RecipeIngredients
							.Include(r => r.Material)
							.Where(r => r.RecipeId == selectedRecipeID)
							.ToListAsync();
							
		}

		public async Task<Quote> GetQuoteAsync(int quoteID)
		{
			return await _dbContext.Quotes
				.Include(q => q.QuotationRecipes)
					.ThenInclude(qr => qr.QuotationTerminalCost)

				.Include(q => q.QuotationRecipes)
					.ThenInclude(qr => qr.QuotationRawMaterialCosts)

				.Include(q => q.QuotationRecipes)
					.ThenInclude(qr => qr.QuotationPackagingCost)

				.Include(q => q.QuotationRecipes)
					.ThenInclude(qr => qr.QuotationDeliveryCost)

				.Include(q => q.QuotationRecipes)
					.ThenInclude(qr => qr.QuotationPremiumCosts)

				.Include(q => q.QuotationRecipes)
					.ThenInclude(qr => qr.QuotationAdditionalCosts)

				.Include(q => q.QuotationRecipes)
					.ThenInclude(qr => qr.QuotationFinancialCost)

				.Include(q => q.QuotationRecipes)
					.ThenInclude(qr => qr.Recipe)
						.ThenInclude(r => r.RecipeIngredients)
							.ThenInclude(ri => ri.Material)

				.Include(q => q.Customer)
					.ThenInclude(q => q.CustomerDeliveryDetails)
				
				.Include(q => q.Status)

				.Include(q => q.QuotationRecipes)
					.ThenInclude(qr => qr.QuotationProductionCost)

					.Include(q => q.Currency)

				.Where(q => q.QuoteId == quoteID)
				.AsSplitQuery()
				.FirstOrDefaultAsync();
		}

		public async Task PostQuote(Quote selectedQuote)
		{
			selectedQuote.StatusId = 2;
			await _dbContext.SaveChangesAsync();
		}

		public async Task CompleteQuote(Quote selectedQuote)
		{
			selectedQuote.StatusId = 3;
			await _dbContext.SaveChangesAsync();
		}

		public async Task ArchiveQuote(Quote selectedQuote)
		{
			selectedQuote.StatusId = 4;
			await _dbContext.SaveChangesAsync();
		}

		public static T DeepClone<T>(T source)
		{
			var json = JsonConvert.SerializeObject(source, new JsonSerializerSettings
			{
				PreserveReferencesHandling = PreserveReferencesHandling.Objects,
				TypeNameHandling = TypeNameHandling.Auto
			});

			return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
			{
				PreserveReferencesHandling = PreserveReferencesHandling.Objects,
				TypeNameHandling = TypeNameHandling.Auto
			})!;
		}
	}
}