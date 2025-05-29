using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GCBQuotationSystem.Models;

public partial class QuotationSystemContext : DbContext
{
    public QuotationSystemContext(DbContextOptions<QuotationSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AdditionalCost> AdditionalCosts { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerDeliveryDetail> CustomerDeliveryDetails { get; set; }

    public virtual DbSet<DeliveryCost> DeliveryCosts { get; set; }

    public virtual DbSet<PackagingMaterial> PackagingMaterials { get; set; }

    public virtual DbSet<Premium> Premiums { get; set; }

    public virtual DbSet<ProductType> ProductTypes { get; set; }

    public virtual DbSet<ProductionCost> ProductionCosts { get; set; }

    public virtual DbSet<QuotationAdditionalCost> QuotationAdditionalCosts { get; set; }

    public virtual DbSet<QuotationDeliveryCost> QuotationDeliveryCosts { get; set; }

    public virtual DbSet<QuotationFinancialCost> QuotationFinancialCosts { get; set; }

    public virtual DbSet<QuotationPackagingCost> QuotationPackagingCosts { get; set; }

    public virtual DbSet<QuotationPremiumCost> QuotationPremiumCosts { get; set; }

    public virtual DbSet<QuotationProductionCost> QuotationProductionCosts { get; set; }

    public virtual DbSet<QuotationRawMaterialCost> QuotationRawMaterialCosts { get; set; }

    public virtual DbSet<QuotationRecipe> QuotationRecipes { get; set; }

    public virtual DbSet<QuotationTerminalCost> QuotationTerminalCosts { get; set; }

    public virtual DbSet<Quote> Quotes { get; set; }

    public virtual DbSet<QuoteStatus> QuoteStatuses { get; set; }

    public virtual DbSet<RawMaterial> RawMaterials { get; set; }

    public virtual DbSet<RawMaterialPriceDetail> RawMaterialPriceDetails { get; set; }

    public virtual DbSet<RawMaterialPriceUpdate> RawMaterialPriceUpdates { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<RecipeIngredient> RecipeIngredients { get; set; }

    public virtual DbSet<TerminalCost> TerminalCosts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdditionalCost>(entity =>
        {
            entity.HasKey(e => e.AdditionalCostId).HasName("PK__Addition__545DECE498C4DBF2");

            entity.Property(e => e.AdditionalCostId).HasColumnName("AdditionalCostID");
            entity.Property(e => e.CostName).HasMaxLength(100);
            entity.Property(e => e.DefaultAmount).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryId).HasName("PK__Country__10D1609F665E00FF");

            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CountryName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.CurrencyId).HasName("PK__Currency__14470B10374CC2D9");

            entity.ToTable("Currency");

            entity.Property(e => e.CurrencyId).HasColumnName("CurrencyID");
            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(3)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustNo).HasName("PK__Customer__049E631A4CB9A698");

            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CustName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.InvoiceAddress1)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.InvoiceAddress2)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.InvoiceCity)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.InvoicePostcode)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Country).WithMany(p => p.Customers)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK__Customers__Count__3C69FB99");
        });

        modelBuilder.Entity<CustomerDeliveryDetail>(entity =>
        {
            entity.HasKey(e => e.DeliveryId).HasName("PK__Customer__626D8FCE4AD3BC56");

            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.Address1).HasMaxLength(255);
            entity.Property(e => e.Address2).HasMaxLength(255);
            entity.Property(e => e.City).HasMaxLength(255);
            entity.Property(e => e.DeliveryName).HasMaxLength(255);
            entity.Property(e => e.Postcode).HasMaxLength(20);

            entity.HasOne(d => d.Country).WithMany(p => p.CustomerDeliveryDetails)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerD__Count__4E88ABD4");

            entity.HasOne(d => d.CustNoNavigation).WithMany(p => p.CustomerDeliveryDetails)
                .HasForeignKey(d => d.CustNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerD__CustN__4D94879B");
        });

        modelBuilder.Entity<DeliveryCost>(entity =>
        {
            entity.HasKey(e => e.DeliveryId).HasName("PK__Delivery__626D8FCE691C4BC8");

            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Pallet).HasMaxLength(50);
            entity.Property(e => e.PostCode).HasMaxLength(50);
        });

        modelBuilder.Entity<PackagingMaterial>(entity =>
        {
            entity.HasKey(e => e.PmId).HasName("PK__Packagin__A440734CB8F1EBED");

            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.Cost100kgEuro).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CostGbp100kg)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("CostGBP100kg");
            entity.Property(e => e.CostGbpton)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("CostGBPTon");
            entity.Property(e => e.CostPerTon).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ExchangeRate).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Packaging).HasMaxLength(255);
            entity.Property(e => e.Product).HasMaxLength(255);
            entity.Property(e => e.Type).HasMaxLength(255);
            entity.Property(e => e.Weight).HasMaxLength(255);
        });

        modelBuilder.Entity<Premium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Premiums__3214EC07FFFA0E53");

            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.PremiumCost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PremiumName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PremiumType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.HasKey(e => e.ProductTypeId).HasName("PK__ProductT__A1312F6EA539CD49");

            entity.Property(e => e.TypeName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ProductionCost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Producti__3214EC273FE3C8BE");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.ProductType).HasMaxLength(100);
            entity.Property(e => e.ProductTypeCost).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<QuotationAdditionalCost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Quotatio__3214EC071112384B");

            entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CostAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CostName).HasMaxLength(100);
            entity.Property(e => e.QuotationRecipeId).HasColumnName("QuotationRecipeID");

            entity.HasOne(d => d.QuotationRecipe).WithMany(p => p.QuotationAdditionalCosts)
                .HasForeignKey(d => d.QuotationRecipeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuotationAdditionalCosts_QuotationRecipe");
        });

        modelBuilder.Entity<QuotationDeliveryCost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Quotatio__3214EC07F6DAF157");

            entity.HasIndex(e => e.QuotationRecipeId, "UQ_QuotationDeliveryCosts_QuotationRecipeId").IsUnique();

            entity.Property(e => e.CostAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DeliveryName).HasMaxLength(100);
            entity.Property(e => e.QuotationRecipeId).HasColumnName("QuotationRecipeID");

            entity.HasOne(d => d.QuotationRecipe).WithOne(p => p.QuotationDeliveryCost)
                .HasForeignKey<QuotationDeliveryCost>(d => d.QuotationRecipeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuotationDeliveryCosts_QuotationRecipeId");
        });

        modelBuilder.Entity<QuotationFinancialCost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Quotatio__3214EC07A09FB243");

            entity.HasIndex(e => e.QuotationRecipeId, "UQ_QuotationFinancialCosts_QuotationRecipeId").IsUnique();

            entity.Property(e => e.InterestRate).HasColumnType("decimal(18, 4)");

            entity.HasOne(d => d.QuotationRecipe).WithOne(p => p.QuotationFinancialCost)
                .HasForeignKey<QuotationFinancialCost>(d => d.QuotationRecipeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuotationFinancialCosts_QuotationRecipeId");
        });

        modelBuilder.Entity<QuotationPackagingCost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Quotatio__3214EC074C793155");

            entity.HasIndex(e => e.QuotationRecipeId, "UQ_QuotationPackagingCosts_QuotationRecipeID").IsUnique();

            entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CostAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PackagingName).HasMaxLength(100);
            entity.Property(e => e.QuotationRecipeId).HasColumnName("QuotationRecipeID");

            entity.HasOne(d => d.QuotationRecipe).WithOne(p => p.QuotationPackagingCost)
                .HasForeignKey<QuotationPackagingCost>(d => d.QuotationRecipeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuotationPackagingCosts_QuotationRecipes");
        });

        modelBuilder.Entity<QuotationPremiumCost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Quotatio__3214EC077FDBA665");

            entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CostAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PremiumName).HasMaxLength(100);
            entity.Property(e => e.QuotationRecipeId).HasColumnName("QuotationRecipeID");

            entity.HasOne(d => d.QuotationRecipe).WithMany(p => p.QuotationPremiumCosts)
                .HasForeignKey(d => d.QuotationRecipeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuotationPremiumCosts_QuotationRecipe");
        });

        modelBuilder.Entity<QuotationProductionCost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Quotatio__3214EC2755F2E09B");

            entity.HasIndex(e => e.QuotationRecipeId, "UQ__Quotatio__A9D97D19D454CB4F").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ProductType).HasMaxLength(100);
            entity.Property(e => e.ProductTypeCost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.QuotationRecipeId).HasColumnName("QuotationRecipeID");

            entity.HasOne(d => d.QuotationRecipe).WithOne(p => p.QuotationProductionCost)
                .HasForeignKey<QuotationProductionCost>(d => d.QuotationRecipeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuotationProductionCosts_QuotationRecipes");
        });

        modelBuilder.Entity<QuotationRawMaterialCost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Quotatio__3214EC0730B3456F");

            entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CostAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MaterialName).HasMaxLength(100);
            entity.Property(e => e.QuotationRecipeId).HasColumnName("QuotationRecipeID");

            entity.HasOne(d => d.QuotationRecipe).WithMany(p => p.QuotationRawMaterialCosts)
                .HasForeignKey(d => d.QuotationRecipeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuotationRawMaterialCosts_QuotationRecipe");
        });

        modelBuilder.Entity<QuotationRecipe>(entity =>
        {
            entity.Property(e => e.QuotationRecipeId).HasColumnName("QuotationRecipeID");
            entity.Property(e => e.Quantity).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.QuoteId).HasColumnName("QuoteID");
            entity.Property(e => e.RecipeId).HasColumnName("RecipeID");

            entity.HasOne(d => d.Quote).WithMany(p => p.QuotationRecipes)
                .HasForeignKey(d => d.QuoteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Quotation__Quote__10566F31");

            entity.HasOne(d => d.Recipe).WithMany(p => p.QuotationRecipes)
                .HasForeignKey(d => d.RecipeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Quotation__Recip__114A936A");
        });

        modelBuilder.Entity<QuotationTerminalCost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Quotatio__3214EC072A3E4903");

            entity.HasIndex(e => e.QuotationRecipeId, "UQ_QuotationTerminalCosts_QuotationRecipeID").IsUnique();

            entity.Property(e => e.Butter).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LifeGbp)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("LifeGBP");
            entity.Property(e => e.Liquor).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Powder).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.QuotationRecipeId).HasColumnName("QuotationRecipeID");
            entity.Property(e => e.TerminalName).HasMaxLength(100);

            entity.HasOne(d => d.QuotationRecipe).WithOne(p => p.QuotationTerminalCost)
                .HasForeignKey<QuotationTerminalCost>(d => d.QuotationRecipeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuotationTerminalCosts_QuotationRecipes");
        });

        modelBuilder.Entity<Quote>(entity =>
        {
            entity.HasKey(e => e.QuoteId).HasName("PK__Quotes__AF9688E10BCEDB4F");

            entity.Property(e => e.QuoteId).HasColumnName("QuoteID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CurrencyId)
                .HasDefaultValue(1)
                .HasColumnName("CurrencyID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.DeliveryDetailId).HasColumnName("DeliveryDetailID");
            entity.Property(e => e.StatusId)
                .HasDefaultValue(1)
                .HasColumnName("StatusID");

            entity.HasOne(d => d.Currency).WithMany(p => p.Quotes)
                .HasForeignKey(d => d.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Quotes_Currency");

            entity.HasOne(d => d.Customer).WithMany(p => p.Quotes)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Quotes__Customer__5BE2A6F2");

            entity.HasOne(d => d.DeliveryDetail).WithMany(p => p.Quotes)
                .HasForeignKey(d => d.DeliveryDetailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Quotes__Delivery__5CD6CB2B");

            entity.HasOne(d => d.Status).WithMany(p => p.Quotes)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Quote_Status");
        });

        modelBuilder.Entity<QuoteStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__QuoteSta__C8EE20434F4703EE");

            entity.ToTable("QuoteStatus");

            entity.HasIndex(e => e.StatusName, "UQ__QuoteSta__05E7698A0FA35F9C").IsUnique();

            entity.Property(e => e.StatusId)
                .ValueGeneratedNever()
                .HasColumnName("StatusID");
            entity.Property(e => e.StatusName)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RawMaterial>(entity =>
        {
            entity.HasKey(e => e.MaterialId).HasName("PK__RawMater__C506131781D23765");

            entity.Property(e => e.MaterialId).HasColumnName("MaterialID");
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.MaterialName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RawMaterialPriceDetail>(entity =>
        {
            entity.HasKey(e => e.PriceDetailId).HasName("PK__RawMater__F6F0A0EFA3F4055F");

            entity.Property(e => e.PriceDetailId).HasColumnName("PriceDetailID");
            entity.Property(e => e.MaterialId).HasColumnName("MaterialID");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PriceUpdateId).HasColumnName("PriceUpdateID");

            entity.HasOne(d => d.Material).WithMany(p => p.RawMaterialPriceDetails)
                .HasForeignKey(d => d.MaterialId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RawMaterial");

            entity.HasOne(d => d.PriceUpdate).WithMany(p => p.RawMaterialPriceDetails)
                .HasForeignKey(d => d.PriceUpdateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PriceUpdate");
        });

        modelBuilder.Entity<RawMaterialPriceUpdate>(entity =>
        {
            entity.HasKey(e => e.PriceUpdateId).HasName("PK__RawMater__10609CFA2F7B6085");

            entity.Property(e => e.PriceUpdateId).HasColumnName("PriceUpdateID");
            entity.Property(e => e.Remark)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.RecipeId).HasName("PK__Recipes__FDD988B01569D400");

            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.RecipeCode).HasMaxLength(255);
            entity.Property(e => e.RecipeDesc).HasMaxLength(255);

            entity.HasOne(d => d.PackagingMaterial).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.PackagingMaterialId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Recipes_PackagingMaterials");

            entity.HasOne(d => d.ProductType).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.ProductTypeId)
                .HasConstraintName("FK_Recipes_ProductType");
        });

        modelBuilder.Entity<RecipeIngredient>(entity =>
        {
            entity.HasKey(e => e.RecipeIngredientId).HasName("PK__RecipeIn__A2C34216E04C782E");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MaterialId).HasColumnName("MaterialID");

            entity.HasOne(d => d.Material).WithMany(p => p.RecipeIngredients)
                .HasForeignKey(d => d.MaterialId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RecipeIngredients_RawMaterials");

            entity.HasOne(d => d.Recipe).WithMany(p => p.RecipeIngredients)
                .HasForeignKey(d => d.RecipeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RecipeIngredients_Recipes");
        });

        modelBuilder.Entity<TerminalCost>(entity =>
        {
            entity.HasKey(e => e.TerminalCostId).HasName("PK__Terminal__12257C3F438D3701");

            entity.Property(e => e.TerminalCostId).HasColumnName("TerminalCostID");
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.Butter).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LifeGbp)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("LifeGBP");
            entity.Property(e => e.Liquor).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PeriodName).HasMaxLength(100);
            entity.Property(e => e.Powder).HasColumnType("decimal(18, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
