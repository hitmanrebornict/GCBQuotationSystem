using System;
using System.Collections.Generic;

namespace GCBQuotationSystem.Models;

public partial class RecipeIngredient
{
    public int RecipeIngredientId { get; set; }

    public int RecipeId { get; set; }

    public int MaterialId { get; set; }

    public decimal Amount { get; set; }

    public virtual RawMaterial Material { get; set; } = null!;

    public virtual Recipe Recipe { get; set; } = null!;
}
