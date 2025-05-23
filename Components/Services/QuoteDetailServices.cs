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

		public async Task updateQuoteInDatabase(Quote selectedQuote, List<QuotationRecipe> selectedQuotationRecipe)
		{
			// Clear the change tracker to avoid conflicts with previously tracked entities
			_dbContext.ChangeTracker.Clear();
			
			// First, retrieve the existing quote from the database to ensure we're updating, not inserting
			var existingQuote = await _dbContext.Quotes.FindAsync(selectedQuote.QuoteId);
			if (existingQuote != null)
			{
				// Update properties of the existing quote without changing its identity
				_dbContext.Entry(existingQuote).CurrentValues.SetValues(selectedQuote);
				
				// Don't set the QuotationRecipes directly to avoid identity issues
				// We'll handle the recipes separately
			}
			else
			{
				// Only add as new if it truly doesn't exist
				// Remove any explicit ID values that might cause identity insert issues
				selectedQuote.QuoteId = 0;
				_dbContext.Quotes.Add(selectedQuote);
			}
			
			// Process each quotation recipe and its related entities
			foreach (var recipe in selectedQuotationRecipe)
			{
				// Check if the recipe already exists
				var existingRecipe = await _dbContext.QuotationRecipes
					.FindAsync(recipe.QuotationRecipeId);
				
				if (existingRecipe != null)
				{
					// Update existing recipe without changing its identity
					_dbContext.Entry(existingRecipe).CurrentValues.SetValues(recipe);
					
					// Handle related collections - we need to process each item individually
					if (recipe.QuotationRawMaterialCosts != null)
					{
						foreach (var materialCost in recipe.QuotationRawMaterialCosts)
						{
							var existingMaterialCost = await _dbContext.Set<QuotationRawMaterialCost>()
								.FindAsync(materialCost.QuotationRecipeId);
								
							if (existingMaterialCost != null)
							{
								_dbContext.Entry(existingMaterialCost).CurrentValues.SetValues(materialCost);
							}
							else
							{
								// Ensure no explicit ID is set for new items
								materialCost.QuotationRecipeId = 0;
								materialCost.QuotationRecipeId = existingRecipe.QuotationRecipeId;
								_dbContext.Set<QuotationRawMaterialCost>().Add(materialCost);
							}
						}
					}
					
					if (recipe.QuotationPremiumCosts != null)
					{
						foreach (var premiumCost in recipe.QuotationPremiumCosts)
						{
							var existingPremiumCost = await _dbContext.Set<QuotationPremiumCost>()
								.FindAsync(premiumCost.QuotationRecipeId);
								
							if (existingPremiumCost != null)
							{
								_dbContext.Entry(existingPremiumCost).CurrentValues.SetValues(premiumCost);
							}
							else
							{
								// Ensure no explicit ID is set for new items
								premiumCost.QuotationRecipeId = existingRecipe.QuotationRecipeId;
								_dbContext.Set<QuotationPremiumCost>().Add(premiumCost);
							}
						}
					}
					
					if (recipe.QuotationAdditionalCosts != null)
					{
						foreach (var additionalCost in recipe.QuotationAdditionalCosts)
						{
							var existingAdditionalCost = await _dbContext.Set<QuotationAdditionalCost>()
								.FindAsync(additionalCost.QuotationRecipeId);
								
							if (existingAdditionalCost != null)
							{
								_dbContext.Entry(existingAdditionalCost).CurrentValues.SetValues(additionalCost);
							}
							else
							{
								// Ensure no explicit ID is set for new items
								additionalCost.QuotationRecipeId = existingRecipe.QuotationRecipeId;
								_dbContext.Set<QuotationAdditionalCost>().Add(additionalCost);
							}
						}
					}
					
					// Handle single navigation properties
					if (recipe.QuotationPackagingCost != null)
					{
						var existingPackagingCost = await _dbContext.Set<QuotationPackagingCost>()
								.FindAsync(recipe.QuotationPackagingCost.QuotationRecipeId);
							
						if (existingPackagingCost != null)
						{
							_dbContext.Entry(existingPackagingCost).CurrentValues.SetValues(recipe.QuotationPackagingCost);
						}
						else
						{
							recipe.QuotationPackagingCost.QuotationRecipeId = existingRecipe.QuotationRecipeId;
							_dbContext.Set<QuotationPackagingCost>().Add(recipe.QuotationPackagingCost);
						}
					}

					if (recipe.QuotationDeliveryCost != null)
					{
						var existingDeliveryCost = await _dbContext.Set<QuotationDeliveryCost>()
							.FindAsync(recipe.QuotationDeliveryCost.QuotationRecipeId);

						if (existingDeliveryCost != null)
						{
							_dbContext.Entry(existingDeliveryCost).CurrentValues.SetValues(recipe.QuotationDeliveryCost);
						}
						else
						{
							recipe.QuotationDeliveryCost.QuotationRecipeId = existingRecipe.QuotationRecipeId;
							_dbContext.Set<QuotationDeliveryCost>().Add(recipe.QuotationDeliveryCost);
						}
					}

					if (recipe.QuotationTerminalCost != null)
					{
						var existingTerminalCost = await _dbContext.Set<QuotationTerminalCost>()
							.FindAsync(recipe.QuotationTerminalCost.QuotationRecipeId);
							
						if (existingTerminalCost != null)
						{
							_dbContext.Entry(existingTerminalCost).CurrentValues.SetValues(recipe.QuotationTerminalCost);
						}
						else
						{
							recipe.QuotationTerminalCost.QuotationRecipeId = existingRecipe.QuotationRecipeId;
							_dbContext.Set<QuotationTerminalCost>().Add(recipe.QuotationTerminalCost);
						}
					}
				}
				else
				{
					// This is a new recipe, ensure no explicit IDs are set
					recipe.QuotationRecipeId = 0;
					
					// Reset IDs for all related entities to avoid identity insert issues
					if (recipe.QuotationRawMaterialCosts != null)
					{
						foreach (var cost in recipe.QuotationRawMaterialCosts)
						{
							cost.QuotationRecipeId = 0;
						}
					}
					
					if (recipe.QuotationPremiumCosts != null)
					{
						foreach (var cost in recipe.QuotationPremiumCosts)
						{
							cost.QuotationRecipeId = 0;
						}
					}
					
					if (recipe.QuotationAdditionalCosts != null)
					{
						foreach (var cost in recipe.QuotationAdditionalCosts)
						{
							cost.QuotationRecipeId = 0;
						}
					}
					
					if (recipe.QuotationPackagingCost != null)
					{
						recipe.QuotationPackagingCost.QuotationRecipeId = 0;
					}

					if (recipe.QuotationDeliveryCost != null)
					{
						recipe.QuotationDeliveryCost.QuotationRecipeId = 0;
					}

					if (recipe.QuotationTerminalCost != null)
					{
						recipe.QuotationTerminalCost.QuotationRecipeId = 0;
					}
					
					// Set the QuoteId to link to the parent quote
					recipe.QuoteId = selectedQuote.QuoteId;
					_dbContext.QuotationRecipes.Add(recipe);
				}
			}
			
			// Save all changes to the database
			await _dbContext.SaveChangesAsync();
		}

		//public async Task updateQuoteInDatabase(Quote selectedQuote, List<QuotationRecipe> selectedQuotationRecipe)
		//{

		//	// Retrieve the quote with its associated quotation recipes
		//	var quote = await _dbContext.Quotes
		//								.Include(q => q.QuotationRecipes)
		//								.FirstOrDefaultAsync(q => q.QuoteId == selectedQuote.QuoteId);

		//	if (quote == null)
		//		throw new Exception("Quote not found");

		//	// Step 1: Remove all existing QuotationRecipes from the Quote
		//	quote.QuotationRecipes.Clear();  // This clears the existing list (doesn't delete them from DB)

		//	// Step 2: Add the new list of QuotationRecipes
		//	foreach (var newRecipe in selectedQuotationRecipe)
		//	{
		//		newRecipe.QuoteId = selectedQuote.QuoteId;  // Ensure the foreign key is set to the correct QuoteId
		//		_dbContext.QuotationRecipes.Add(newRecipe);  // Add new recipe to the context
		//	}

		//	// Step 3: Mark the Quote entity as updated (this ensures EF Core tracks the changes)
		//	_dbContext.Quotes.Update(quote);  // This ensures Quote itself is updated

		//	// Step 4: Save changes to the database
		//	await _dbContext.SaveChangesAsync();
		//}


		private void DetachAssociatedCosts(QuotationRecipe quotationRecipe)
		{
			// Detach costs that might be already tracked to avoid conflict
			foreach (var premiumCost in quotationRecipe.QuotationPremiumCosts)
			{
				var premiumCostEntry = _dbContext.Entry(premiumCost);
				if (premiumCostEntry.State != EntityState.Detached)
				{
					premiumCostEntry.State = EntityState.Detached;
				}
			}

			foreach (var rawMaterialCost in quotationRecipe.QuotationRawMaterialCosts)
			{
				var rawMaterialCostEntry = _dbContext.Entry(rawMaterialCost);
				if (rawMaterialCostEntry.State != EntityState.Detached)
				{
					rawMaterialCostEntry.State = EntityState.Detached;
				}
			}

			var packagingCostEntry = _dbContext.Entry(quotationRecipe.QuotationPackagingCost);
			if (packagingCostEntry.State != EntityState.Detached)
			{
				packagingCostEntry.State = EntityState.Detached;
			}

			var deliveryCostEntry = _dbContext.Entry(quotationRecipe.QuotationDeliveryCost);
			if (deliveryCostEntry.State != EntityState.Detached)
			{
				deliveryCostEntry.State = EntityState.Detached;
			}

			foreach (var additionalCost in quotationRecipe.QuotationAdditionalCosts)
			{
				var additionalCostEntry = _dbContext.Entry(additionalCost);
				if (additionalCostEntry.State != EntityState.Detached)
				{
					additionalCostEntry.State = EntityState.Detached;
				}
			}
		}

		private void AttachCosts(QuotationRecipe quotationRecipe)
		{
			// Attach the cost entities to the context if they exist
			if (quotationRecipe.QuotationTerminalCost != null)
				_dbContext.Attach(quotationRecipe.QuotationTerminalCost);

			if (quotationRecipe.QuotationPackagingCost != null)
				_dbContext.Attach(quotationRecipe.QuotationPackagingCost);

			if (quotationRecipe.QuotationDeliveryCost != null)
				_dbContext.Attach(quotationRecipe.QuotationDeliveryCost);

			// For new or modified raw material costs, set them to modified
			foreach (var rawMaterialCost in quotationRecipe.QuotationRawMaterialCosts)
			{
				_dbContext.Entry(rawMaterialCost).State = EntityState.Modified; // Update if it's modified
			}

			// For new or modified additional costs, set them to modified
			foreach (var additionalCost in quotationRecipe.QuotationAdditionalCosts)
			{
				_dbContext.Entry(additionalCost).State = EntityState.Modified; // Update if it's modified
			}

			// For new or modified premium costs, set them to modified
			foreach (var premiumCost in quotationRecipe.QuotationPremiumCosts)
			{
				_dbContext.Entry(premiumCost).State = EntityState.Modified; // Update if it's modified
			}
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
				
				.Include(q => q.Status)

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