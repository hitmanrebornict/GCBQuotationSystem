using System;
using System.Collections.Generic;

namespace GCBQuotationSystem.Models;

public partial class QuotationRecipe
{
    public int QuotationRecipeId { get; set; }

    public int QuoteId { get; set; }

    public int RecipeId { get; set; }

    public DateOnly PeriodMonth { get; set; }

    public decimal Quantity { get; set; }

    public virtual ICollection<QuotationAdditionalCost> QuotationAdditionalCosts { get; set; } = new List<QuotationAdditionalCost>();

    public virtual QuotationDeliveryCost? QuotationDeliveryCost { get; set; }

    public virtual QuotationFinancialCost? QuotationFinancialCost { get; set; }

    public virtual QuotationPackagingCost? QuotationPackagingCost { get; set; }

    public virtual ICollection<QuotationPremiumCost> QuotationPremiumCosts { get; set; } = new List<QuotationPremiumCost>();

    public virtual QuotationProductionCost? QuotationProductionCost { get; set; }

    public virtual ICollection<QuotationRawMaterialCost> QuotationRawMaterialCosts { get; set; } = new List<QuotationRawMaterialCost>();

    public virtual QuotationTerminalCost? QuotationTerminalCost { get; set; }

    public virtual Quote Quote { get; set; } = null!;

    public virtual Recipe Recipe { get; set; } = null!;
}
