using System;
using System.Collections.Generic;

namespace GCBQuotationSystem.Models;

public partial class RawMaterial
{
    public int MaterialId { get; set; }

    public string MaterialName { get; set; } = null!;

    public bool Active { get; set; }

    public virtual ICollection<RawMaterialPriceDetail> RawMaterialPriceDetails { get; set; } = new List<RawMaterialPriceDetail>();

    public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
}
