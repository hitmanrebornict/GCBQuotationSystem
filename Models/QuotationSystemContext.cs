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

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

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
        modelBuilder.UseCollation("Latin1_General_CI_AS");

        modelBuilder.Entity<AdditionalCost>(entity =>
        {
            entity.HasKey(e => e.AdditionalCostId).HasName("PK__Addition__545DECE4D6BF64C2");

            entity.Property(e => e.AdditionalCostId).HasColumnName("AdditionalCostID");
            entity.Property(e => e.CostName).HasMaxLength(100);
            entity.Property(e => e.DefaultAmount).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryId).HasName("PK__Countrie__10D1609FC66E79B9");

            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CountryName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.CurrencyId).HasName("PK__Currency__14470B10B704E87D");

            entity.ToTable("Currency");

            entity.Property(e => e.CurrencyId).HasColumnName("CurrencyID");
            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(3)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustNo).HasName("PK__Customer__049E631AD2DA2A62");

            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CurrencyId).HasColumnName("CurrencyID");
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
                .HasConstraintName("FK__Customers__Count__0022696D");

            entity.HasOne(d => d.Currency).WithMany(p => p.Customers)
                .HasForeignKey(d => d.CurrencyId)
                .HasConstraintName("FK_Customers_Currency");
        });

        modelBuilder.Entity<CustomerDeliveryDetail>(entity =>
        {
            entity.HasKey(e => e.DeliveryId).HasName("PK__Customer__626D8FCE4CCED779");

            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.Address1).HasMaxLength(255);
            entity.Property(e => e.Address2).HasMaxLength(255);
            entity.Property(e => e.City).HasMaxLength(255);
            entity.Property(e => e.DeliveryName).HasMaxLength(255);
            entity.Property(e => e.Postcode).HasMaxLength(20);

            entity.HasOne(d => d.Country).WithMany(p => p.CustomerDeliveryDetails)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerD__Count__7E3A20FB");

            entity.HasOne(d => d.CustNoNavigation).WithMany(p => p.CustomerDeliveryDetails)
                .HasForeignKey(d => d.CustNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerD__CustN__7F2E4534");
        });

        modelBuilder.Entity<DeliveryCost>(entity =>
        {
            entity.HasKey(e => e.DeliveryId).HasName("PK__Delivery__626D8FCEA08A18A6");

            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Pallet).HasMaxLength(50);
            entity.Property(e => e.PostCode).HasMaxLength(50);
            entity.Property(e => e.ServiceHours).HasDefaultValue(1);
            entity.Property(e => e.Zone).HasDefaultValue(1);

            entity.HasOne(d => d.DeliveryAddress).WithMany(p => p.DeliveryCosts)
                .HasForeignKey(d => d.DeliveryAddressId)
                .HasConstraintName("FK_DeliveryCosts_CustomerDeliveryDetails");
        });

        modelBuilder.Entity<PackagingMaterial>(entity =>
        {
            entity.HasKey(e => e.PmId).HasName("PK__Packagin__A440734C580342AD");

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
            entity.HasKey(e => e.Id).HasName("PK__Premiums__3214EC077D86251F");

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
            entity.HasKey(e => e.ProductTypeId).HasName("PK__ProductT__A1312F6E3F080CCC");

            entity.Property(e => e.TypeName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ProductionCost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Producti__3214EC273A6D8F4B");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.ProductType).HasMaxLength(100);
            entity.Property(e => e.ProductTypeCost).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<QuotationAdditionalCost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Quotatio__3214EC07E0CF7E76");

            entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CostAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CostName).HasMaxLength(100);
            entity.Property(e => e.QuotationRecipeId).HasColumnName("QuotationRecipeID");

            entity.HasOne(d => d.QuotationRecipe).WithMany(p => p.QuotationAdditionalCosts)
                .HasForeignKey(d => d.QuotationRecipeId)
                .HasConstraintName("FK_QuotationAdditionalCosts_QuotationRecipes");
        });

        modelBuilder.Entity<QuotationDeliveryCost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Quotatio__3214EC07E82A237E");

            entity.HasIndex(e => e.QuotationRecipeId, "UQ_QuotationDeliveryCosts_QuotationRecipeId").IsUnique();

            entity.Property(e => e.CostAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DeliveryName).HasMaxLength(100);
            entity.Property(e => e.QuotationRecipeId).HasColumnName("QuotationRecipeID");

            entity.HasOne(d => d.QuotationRecipe).WithOne(p => p.QuotationDeliveryCost)
                .HasForeignKey<QuotationDeliveryCost>(d => d.QuotationRecipeId)
                .HasConstraintName("FK_QuotationDeliveryCosts_QuotationRecipes");
        });

        modelBuilder.Entity<QuotationFinancialCost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Quotatio__3214EC0733BC7871");

            entity.HasIndex(e => e.QuotationRecipeId, "UQ_QuotationFinancialCosts_QuotationRecipeId").IsUnique();

            entity.Property(e => e.InterestRate).HasColumnType("decimal(18, 4)");

            entity.HasOne(d => d.QuotationRecipe).WithOne(p => p.QuotationFinancialCost)
                .HasForeignKey<QuotationFinancialCost>(d => d.QuotationRecipeId)
                .HasConstraintName("FK_QuotationFinancialCosts_QuotationRecipes");
        });

        modelBuilder.Entity<QuotationPackagingCost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Quotatio__3214EC071D121B44");

            entity.HasIndex(e => e.QuotationRecipeId, "UQ_QuotationPackagingCosts_QuotationRecipeID").IsUnique();

            entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CostAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PackagingName).HasMaxLength(100);
            entity.Property(e => e.QuotationRecipeId).HasColumnName("QuotationRecipeID");

            entity.HasOne(d => d.QuotationRecipe).WithOne(p => p.QuotationPackagingCost)
                .HasForeignKey<QuotationPackagingCost>(d => d.QuotationRecipeId)
                .HasConstraintName("FK_QuotationPackagingCosts_QuotationRecipes");
        });

        modelBuilder.Entity<QuotationPremiumCost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Quotatio__3214EC07DFC8CF62");

            entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CostAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PremiumName).HasMaxLength(100);
            entity.Property(e => e.QuotationRecipeId).HasColumnName("QuotationRecipeID");

            entity.HasOne(d => d.QuotationRecipe).WithMany(p => p.QuotationPremiumCosts)
                .HasForeignKey(d => d.QuotationRecipeId)
                .HasConstraintName("FK_QuotationPremiumCosts_QuotationRecipes");
        });

        modelBuilder.Entity<QuotationProductionCost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Quotatio__3214EC274163C2E4");

            entity.HasIndex(e => e.QuotationRecipeId, "UQ__Quotatio__A9D97D19BDD7AC03").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ProductType).HasMaxLength(100);
            entity.Property(e => e.ProductTypeCost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.QuotationRecipeId).HasColumnName("QuotationRecipeID");

            entity.HasOne(d => d.QuotationRecipe).WithOne(p => p.QuotationProductionCost)
                .HasForeignKey<QuotationProductionCost>(d => d.QuotationRecipeId)
                .HasConstraintName("FK_QuotationProductionCosts_QuotationRecipes");
        });

        modelBuilder.Entity<QuotationRawMaterialCost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Quotatio__3214EC0784C781C7");

            entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CostAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MaterialName).HasMaxLength(100);
            entity.Property(e => e.QuotationRecipeId).HasColumnName("QuotationRecipeID");

            entity.HasOne(d => d.QuotationRecipe).WithMany(p => p.QuotationRawMaterialCosts)
                .HasForeignKey(d => d.QuotationRecipeId)
                .HasConstraintName("FK_QuotationRawMaterialCosts_QuotationRecipes");
        });

        modelBuilder.Entity<QuotationRecipe>(entity =>
        {
            entity.Property(e => e.QuotationRecipeId).HasColumnName("QuotationRecipeID");
            entity.Property(e => e.Quantity).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.QuoteId).HasColumnName("QuoteID");
            entity.Property(e => e.RecipeId).HasColumnName("RecipeID");

            entity.HasOne(d => d.Quote).WithMany(p => p.QuotationRecipes)
                .HasForeignKey(d => d.QuoteId)
                .HasConstraintName("FK_QuotationRecipes_Quotes");

            entity.HasOne(d => d.Recipe).WithMany(p => p.QuotationRecipes)
                .HasForeignKey(d => d.RecipeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Quotation__Recip__0B941C19");
        });

        modelBuilder.Entity<QuotationTerminalCost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Quotatio__3214EC0765C667FD");

            entity.HasIndex(e => e.QuotationRecipeId, "UQ_QuotationTerminalCosts_QuotationRecipeID").IsUnique();

            entity.Property(e => e.Butter).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.GhanaLiquor).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LifeGbp)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("LifeGBP");
            entity.Property(e => e.Liquor).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Powder).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.QuotationRecipeId).HasColumnName("QuotationRecipeID");
            entity.Property(e => e.TerminalName).HasMaxLength(100);

            entity.HasOne(d => d.QuotationRecipe).WithOne(p => p.QuotationTerminalCost)
                .HasForeignKey<QuotationTerminalCost>(d => d.QuotationRecipeId)
                .HasConstraintName("FK_QuotationTerminalCosts_QuotationRecipes");
        });

        modelBuilder.Entity<Quote>(entity =>
        {
            entity.HasKey(e => e.QuoteId).HasName("PK__Quotes__AF9688E15305A000");

            entity.Property(e => e.QuoteId).HasColumnName("QuoteID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CurrencyId)
                .HasDefaultValue(1)
                .HasColumnName("CurrencyID");
            entity.Property(e => e.CurrencyRates).HasColumnType("decimal(18, 6)");
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
                .HasConstraintName("FK__Quotes__Customer__0D7C648B");

            entity.HasOne(d => d.DeliveryDetail).WithMany(p => p.Quotes)
                .HasForeignKey(d => d.DeliveryDetailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Quotes__Delivery__0E7088C4");

            entity.HasOne(d => d.Status).WithMany(p => p.Quotes)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Quote_Status");
        });

        modelBuilder.Entity<QuoteStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__QuoteSta__C8EE2043FDE1956C");

            entity.ToTable("QuoteStatus");

            entity.HasIndex(e => e.StatusName, "UQ__QuoteSta__05E7698AB710ACF9").IsUnique();

            entity.Property(e => e.StatusId)
                .ValueGeneratedNever()
                .HasColumnName("StatusID");
            entity.Property(e => e.StatusName)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RawMaterial>(entity =>
        {
            entity.HasKey(e => e.MaterialId).HasName("PK__RawMater__C50613172CF9D1E9");

            entity.Property(e => e.MaterialId).HasColumnName("MaterialID");
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.MaterialName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RawMaterialPriceDetail>(entity =>
        {
            entity.HasKey(e => e.PriceDetailId).HasName("PK__RawMater__F6F0A0EF7701F571");

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
            entity.HasKey(e => e.PriceUpdateId).HasName("PK__RawMater__10609CFA7595526E");

            entity.Property(e => e.PriceUpdateId).HasColumnName("PriceUpdateID");
            entity.Property(e => e.Remark)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.RecipeId).HasName("PK__Recipes__FDD988B038C9B37D");

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
            entity.HasKey(e => e.RecipeIngredientId).HasName("PK__RecipeIn__A2C34216D7AC7A45");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 4)");
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
            entity.HasKey(e => e.TerminalCostId).HasName("PK__Terminal__12257C3F72BAEBA4");

            entity.Property(e => e.TerminalCostId).HasColumnName("TerminalCostID");
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.Butter).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.GhanaLiquor).HasColumnType("decimal(18, 2)");
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
