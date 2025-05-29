using GCBQuotationSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Radzen;
using System.Diagnostics;

namespace GCBQuotationSystem.Components.Services
{
	public class MasterDataServices
	{
		private QuotationSystemContext _dbContext;
		private UserManager<IdentityUser> _userManager;

		public MasterDataServices(QuotationSystemContext dbContext, UserManager<IdentityUser> userManager)
		{
			_dbContext = dbContext;
			_userManager = userManager;
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
			return await _dbContext.Customers
				.Where(c => c.Active)
				.OrderBy(c => c.CustName)
				.AsNoTracking().ToListAsync();
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
									.Where(r => r.Active)
									.OrderBy(r => r.RecipeCode)
									.AsNoTracking()
								   .ToListAsync();
		}


		//Customer Master Data
		public async Task<List<Customer>> GetCustomersAsync()
		{
			return await _dbContext.Customers.Where(c => c.Active == true).ToListAsync();
		}

		public async Task<bool> AddCustomerAsync(Customer customer)
		{
			try
			{
				_dbContext.Customers.Add(customer);
				await _dbContext.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				// Log exception
				return false;
			}
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

		public async Task<List<ProductionCost>> GetProductionCostsAsync()
		{
			return await _dbContext.ProductionCosts
						.Where(p => p.Active)
						.ToListAsync();
		}

		public async Task<Customer?> GetCustomerByIdAsync(int custNo)
		{
			return await _dbContext.Customers
				.Include(c => c.Country)
				.FirstOrDefaultAsync(c => c.CustNo == custNo);
		}

		public async Task<bool> UpdateCustomerAsync(Customer customer)
		{
			try
			{
				_dbContext.Customers.Update(customer);
				await _dbContext.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				// Log exception
				return false;
			}
		}

		public async Task<List<AspNetUser>> GetUserListAsync()
		{
			return await _dbContext.AspNetUsers
				 .Include(r => r.Roles)
				.ToListAsync();
		}

		public async Task insertUserDataIntoDatabase(string addedName, string selectedRole, string password, string email)
		{
			try
			{
				var user = new IdentityUser { UserName = addedName };
				var result = await _userManager.CreateAsync(user, password);


				if (result.Succeeded)
				{
					await _userManager.AddToRoleAsync(user, selectedRole);
					var setEmailResult = await _userManager.SetEmailAsync(user, email);
				}
				else
				{
				}

				foreach (var error in result.Errors)
				{
					throw new Exception(error.Description);
				}



				var aspnetuser = await _dbContext.AspNetUsers.FirstOrDefaultAsync(u => u.UserName == addedName);

				if (aspnetuser != null)
				{
					aspnetuser.Active = true;

					_dbContext.AspNetUsers.Update(aspnetuser);
					await _dbContext.SaveChangesAsync();
				}


			}

			catch (Exception ex)
			{
			}
		}
	}
}
