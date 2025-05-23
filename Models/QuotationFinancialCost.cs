using System;
using System.Collections.Generic;

namespace GCBQuotationSystem.Models;

public partial class QuotationFinancialCost
{
    public int Id { get; set; }

    public int QuotationRecipeId { get; set; }

    public decimal InterestRate { get; set; }

    public int FinanceDays { get; set; }

    public virtual QuotationRecipe QuotationRecipe { get; set; } = null!;
}
