using GCBQuotationSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace GCBQuotationSystem.Components.Services
{
	public class MasterDataServices
	{
		private QuotationSystemContext _dbContext;

		public MasterDataServices(QuotationSystemContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<RawMaterialPriceUpdate?> GetPriceUpdateByMonthAsync(DateOnly month)
		{
			return await _dbContext.RawMaterialPriceUpdates
				.Include(u => u.RawMaterialPriceDetails)
					.ThenInclude(d => d.Material)
				.Where(u => u.PriceMonth == month)
				.OrderByDescending(u => u.UpdateDate)
				.FirstOrDefaultAsync();
		}

		public async Task UpdatePriceDetailAsync(RawMaterialPriceDetail detail)
		{
			// Attach and mark the entity as modified.
			_dbContext.RawMaterialPriceDetails.Update(detail);
			await _dbContext.SaveChangesAsync();
		}


		public async Task<RawMaterialPriceUpdate> CreatePriceUpdateAsync(RawMaterialPriceUpdate newRecord)
		{
			_dbContext.RawMaterialPriceUpdates.Add(newRecord);
			await _dbContext.SaveChangesAsync();
			return newRecord;
		}

		public async Task<RawMaterialPriceUpdate?> GetLatestPriceUpdateAsync()
		{
			return await _dbContext.RawMaterialPriceUpdates
				.Include(u => u.RawMaterialPriceDetails)
					.ThenInclude(d => d.Material)
				.OrderByDescending(u => u.UpdateDate)
				.FirstOrDefaultAsync();
		}

		public async Task<List<RawMaterial>> GetAllRawMaterialsAsync()
		{
			return await _dbContext.RawMaterials.ToListAsync();
		}

		public async Task UpdatePriceUpdateAsync(RawMaterialPriceUpdate record)
		{
			_dbContext.RawMaterialPriceUpdates.Update(record);
			await _dbContext.SaveChangesAsync();
		}



		/// <summary>
		/// Returns all customers.
		/// </summary>
		public async Task<List<Customer>> GetAllCustomersAsync()
		{
			return await _dbContext.Customers.AsNoTracking().ToListAsync();
		}

		/// <summary>
		/// Returns all delivery addresses for the specified customer.
		/// </summary>
		public async Task<List<CustomerDeliveryDetail>> GetDeliveryDetailsForCustomerAsync(int custNo)
		{
			return await _dbContext.CustomerDeliveryDetails
				.Where(d => d.CustNo == custNo)
				.AsNoTracking()
				.ToListAsync();
		}

		/// <summary>
		/// Returns all product types.
		/// </summary>
		public async Task<List<ProductType>> GetProductTypesAsync()
		{
			return await _dbContext.ProductTypes.AsNoTracking().ToListAsync();
		}

		public async Task<List<Recipe>> GetRecipesAsync()
		{
			return await _dbContext.Recipes
								   .Include(r => r.RecipeIngredients)
									   .ThenInclude(ri => ri.Material)
									.Include(r => r.PackagingMaterial)
									.Include(r => r.ProductType)
									.AsNoTracking()
								   .ToListAsync();
		}


		//Customer Master Data
		public async Task<List<Customer>> GetCustomersAsync()
		{
			return await _dbContext.Customers.Where(c => c.Active == true).ToListAsync();
		}

		public async Task<Customer> AddCustomerAsync(Customer customer)
		{
			_dbContext.Customers.Add(customer);
			await _dbContext.SaveChangesAsync();
			return customer;
		}

		public async Task<List<Country>> GetAllCountriesAsync()
		{
			return await _dbContext.Countries.ToListAsync();
		}

		public async Task<List<ProductType>> GetAllProductTypesAsync()
		{
			return await _dbContext.ProductTypes.ToListAsync();
		}

		public async Task<List<Premium>> GetAllPremiumsAsync()
		{
			return await _dbContext.Premiums.ToListAsync();
		}

		public async Task<List<PackagingMaterial>> GetPackagingMaterial()
		{
			return await _dbContext.PackagingMaterials.ToListAsync();
		}

		public async Task addNewRecipeAsync(Recipe newRecipe)
		{
			await _dbContext.Recipes.AddAsync(newRecipe);
			await _dbContext.SaveChangesAsync();
		}

		public async Task<List<PackagingMaterial>> GetAllPackagingMaterialsAsync()
		{
			return await _dbContext.PackagingMaterials
							.Where(p => p.Active == true)
							.ToListAsync();
		}

		public async Task<List<DeliveryCost>> GetDeliveryCostAsync()
		{
			return await _dbContext.DeliveryCosts.ToListAsync();
		}

		public async Task<Recipe> GetRecipeByIdAsync(int recipeId)
		{
			return await _dbContext.Recipes
				.Include(r => r.RecipeIngredients)
					.ThenInclude(ri => ri.Material)
				.FirstOrDefaultAsync(r => r.RecipeId == recipeId);
		}

		public async Task UpdateRecipeAsync(Recipe recipe)
		{
			_dbContext.Recipes.Update(recipe);
			await _dbContext.SaveChangesAsync();
		}

		public async Task<List<TerminalCost>> GetTerminalCostAsync()
		{
			return await _dbContext.TerminalCosts.ToListAsync();
		}

		public async Task<TerminalCost> GetTerminalCostByIdAsync(int terminalCostId)
		{
			return await _dbContext.TerminalCosts.FindAsync(terminalCostId);
		}

		public async Task UpdateTerminalCostAsync(TerminalCost terminalCost)
		{
			_dbContext.TerminalCosts.Update(terminalCost);
			await _dbContext.SaveChangesAsync();
		}

		public async Task<TerminalCost> AddTerminalCostAsync(TerminalCost terminalCost)
		{
			await _dbContext.TerminalCosts.AddAsync(terminalCost);
			await _dbContext.SaveChangesAsync();
			return terminalCost;
		}

		public async Task<List<DateOnly>> GetExistingTerminalPeriodsAsync()
		{
			try
			{
				// Fetch all existing terminal costs from the database
				var terminalCosts = await _dbContext.TerminalCosts.ToListAsync();
				
				// Extract and return just the terminal period dates
				return terminalCosts.Select(tc => tc.TerminalPeriod).Distinct().ToList();
			}
			catch (Exception ex)
			{
				// Log the exception
				Console.WriteLine($"Error fetching existing terminal periods: {ex.Message}");
				throw;
			}
		}

		public async Task<List<Premium>> GetPremiumsAsync()
		{
			return await _dbContext.Premiums.ToListAsync();
		}

		public async Task<Premium> GetPremiumByIdAsync(int id)
		{
			return await _dbContext.Premiums.FindAsync(id);
		
		}

		public async Task AddPremiumAsync(Premium premium)
		{
			_dbContext.Premiums.Add(premium);
			await _dbContext.SaveChangesAsync();
		}

		public async Task UpdatePremiumAsync(Premium premium)
		{
			_dbContext.Premiums.Update(premium);
			await _dbContext.SaveChangesAsync();
		}

		public async Task<PackagingMaterial> GetPackagingMaterialByIdAsync(int pmId)
		{
			return await _dbContext.PackagingMaterials.FindAsync(pmId);
		}

		public async Task AddPackagingMaterialAsync(PackagingMaterial packagingMaterial)
		{
			_dbContext.PackagingMaterials.Add(packagingMaterial);
			await _dbContext.SaveChangesAsync();
		}

		public async Task UpdatePackagingMaterialAsync(PackagingMaterial packagingMaterial)
		{
			_dbContext.PackagingMaterials.Update(packagingMaterial);
			await _dbContext.SaveChangesAsync();
		}

		public async Task<DeliveryCost> GetDeliveryCostByIdAsync(int deliveryId)
		{
			return await _dbContext.DeliveryCosts.FindAsync(deliveryId);
		}

		public async Task AddDeliveryCostAsync(DeliveryCost deliveryCost)
		{
			_dbContext.DeliveryCosts.Add(deliveryCost);
			await _dbContext.SaveChangesAsync();
		}

		public async Task UpdateDeliveryCostAsync(DeliveryCost deliveryCost)
		{
			_dbContext.DeliveryCosts.Update(deliveryCost);
			await _dbContext.SaveChangesAsync();
		}

		public async Task<List<CustomerDeliveryDetail>> GetAllCustomerDeliveryDetailsAsync()
		{
			return await _dbContext.CustomerDeliveryDetails
				.Include(d => d.CustNoNavigation)
				.Include(d => d.Country)
				.ToListAsync();
		}

		public async Task<CustomerDeliveryDetail> GetCustomerDeliveryDetailByIdAsync(int deliveryId)
		{
			return await _dbContext.CustomerDeliveryDetails
				.Include(d => d.CustNoNavigation)
				.Include(d => d.Country)
				.FirstOrDefaultAsync(d => d.DeliveryId == deliveryId);
		}

		public async Task AddCustomerDeliveryDetailAsync(CustomerDeliveryDetail deliveryDetail)
		{
			_dbContext.CustomerDeliveryDetails.Add(deliveryDetail);
			await _dbContext.SaveChangesAsync();
		}

		public async Task UpdateCustomerDeliveryDetailAsync(CustomerDeliveryDetail deliveryDetail)
		{
			_dbContext.CustomerDeliveryDetails.Update(deliveryDetail);
			await _dbContext.SaveChangesAsync();
		}
	}
}
