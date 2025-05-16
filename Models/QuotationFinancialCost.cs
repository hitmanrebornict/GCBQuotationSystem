using System;
using System.Collections.Generic;

namespace GCBQuotationSystem.Models;

public partial class QuotationFinancialCost
{
    public int Id { get; set; }

    public int QuotationRecipeId { get; set; }

    public decimal FinancialCostAmount { get; set; }

    public decimal FinanceDays { get; set; }

    public virtual QuotationRecipe QuotationRecipe { get; set; } = null!;
}
