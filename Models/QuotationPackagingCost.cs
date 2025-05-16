using System;
using System.Collections.Generic;

namespace GCBQuotationSystem.Models;

public partial class QuotationPackagingCost
{
    public int Id { get; set; }

    public int QuotationRecipeId { get; set; }

    public decimal Cost { get; set; }

    public string PackagingName { get; set; } = null!;

    public decimal CostAmount { get; set; }

    public virtual QuotationRecipe QuotationRecipe { get; set; } = null!;
}
