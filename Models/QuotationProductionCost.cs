using System;
using System.Collections.Generic;

namespace GCBQuotationSystem.Models;

public partial class QuotationProductionCost
{
    public int Id { get; set; }

    public int QuotationRecipeId { get; set; }

    public string ProductType { get; set; } = null!;

    public decimal ProductTypeCost { get; set; }

    public virtual QuotationRecipe QuotationRecipe { get; set; } = null!;
}
