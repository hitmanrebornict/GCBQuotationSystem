using System;
using System.Collections.Generic;

namespace GCBQuotationSystem.Models;

public partial class ProductType
{
    public int ProductTypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}
