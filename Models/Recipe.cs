using System;
using System.Collections.Generic;

namespace GCBQuotationSystem.Models;

public partial class Recipe
{
    public int RecipeId { get; set; }

    public string? RecipeCode { get; set; }

    public string? RecipeDesc { get; set; }

    public int? ProductTypeId { get; set; }

    public int PackagingMaterialId { get; set; }

    public bool Active { get; set; }

    public bool Certified { get; set; }

    public virtual PackagingMaterial PackagingMaterial { get; set; } = null!;

    public virtual ProductType? ProductType { get; set; }

    public virtual ICollection<QuotationRecipe> QuotationRecipes { get; set; } = new List<QuotationRecipe>();

    public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
}
