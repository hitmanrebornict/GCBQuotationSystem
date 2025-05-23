using GCBQuotationSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace GCBQuotationSystem.Components.Services
{
	public class GenerateQuoteServices
	{
		private QuotationSystemContext _dbContext;

		public GenerateQuoteServices(QuotationSystemContext dbContext)
		{
			_dbContext = dbContext;
		}


		public async Task<List<RawMaterialPriceDetail>> GetRawMaterialCostByRecipeID(DateOnly month)
		{
			return await _dbContext.RawMaterialPriceDetails
							.Include(r => r.Material)
						.Where(r => r.PriceUpdate.PriceMonth.Month == month.Month && r.PriceUpdate.PriceMonth.Year == month.Year)
						.ToListAsync();
		}

		public async Task<TerminalCost> GetTerminalCostByDate(DateOnly dateOnlyMonth)
		{
			return await _dbContext.TerminalCosts
				.Where(t => t.TerminalPeriod >= dateOnlyMonth)
				.OrderBy(t => t.TerminalPeriod)
				.FirstOrDefaultAsync();
		}

		public async Task<List<AdditionalCost>> GetAdditionalCosts()
		{
			return await _dbContext.AdditionalCosts
				.ToListAsync();
		}

	
		public string GetQuarter(DateOnly date)
		{
			int quarter = (date.Month - 1) / 3 + 1;
			return $"Q{quarter}/{date.Year}";
		}
	}
}
