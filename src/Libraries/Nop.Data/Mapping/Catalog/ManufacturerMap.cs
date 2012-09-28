using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Catalog;

namespace Nop.Data.Mapping.Catalog
{
    public partial class ManufacturerMap : EntityTypeConfiguration<Manufacturer>
    {
        public ManufacturerMap()
        {
            this.ToTable("Manufacturer");
            this.HasKey(m => m.Id);
            this.Property(m => m.Name).IsRequired().HasMaxLength(400);
            this.Property(m => m.Description).IsMaxLength();
            this.Property(m => m.MetaKeywords).HasMaxLength(400);
            this.Property(m => m.MetaDescription);
            this.Property(m => m.MetaTitle).HasMaxLength(400);
            this.Property(m => m.PriceRanges).HasMaxLength(400);
            this.Property(m => m.PageSizeOptions).HasMaxLength(200);
        }
    }
}