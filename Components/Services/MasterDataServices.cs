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
			return await _dbContext.Customers.ToListAsync();
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
	}
}
