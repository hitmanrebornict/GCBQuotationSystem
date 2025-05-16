using System;
using System.Collections.Generic;

namespace GCBQuotationSystem.Models;

public partial class QuotationTerminalCost
{
    public int Id { get; set; }

    public string TerminalName { get; set; } = null!;

    public DateOnly TerminalPeriod { get; set; }

    public decimal LifeGbp { get; set; }

    public decimal Liquor { get; set; }

    public decimal Butter { get; set; }

    public decimal Powder { get; set; }

    public int QuotationRecipeId { get; set; }

    public virtual QuotationRecipe QuotationRecipe { get; set; } = null!;
}
