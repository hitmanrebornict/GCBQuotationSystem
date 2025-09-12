using System;
using System.Collections.Generic;

namespace GCBQuotationSystem.Models;

public partial class TerminalCost
{
    public int TerminalCostId { get; set; }

    public string PeriodName { get; set; } = null!;

    public DateOnly TerminalPeriod { get; set; }

    public decimal LifeGbp { get; set; }

    public decimal Liquor { get; set; }

    public decimal Butter { get; set; }

    public decimal Powder { get; set; }

    public bool Active { get; set; }

    public decimal GhanaLiquor { get; set; }
}
