//------------------------------------------------------------------------------
// The contents of this file are subject to the nopCommerce Public License Version 1.0 ("License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at  http://www.nopCommerce.com/License.aspx. 
// 
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. 
// See the License for the specific language governing rights and limitations under the License.
// 
// The Original Code is nopCommerce.
// The Initial Developer of the Original Code is NopSolutions.
// All Rights Reserved.
// 
// Contributor(s): _______. 
//------------------------------------------------------------------------------

using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using Nop.Core.Domain;
using Nop.Core;
using Nop.Data.Mapping;
using System.Data.Entity.Infrastructure;

namespace Nop.Data
{
    /// <summary>
    /// Object context
    /// </summary>
    public class NopObjectContext : DbContext, IDbContext
    {
        public NopObjectContext(string connectionStringName) : base(connectionStringName)
        {
            //((IObjectContextAdapter) this).ObjectContext.ContextOptions.LazyLoadingEnabled = true;
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<LocaleStringResource> LocaleStringResources { get; set; }
        public DbSet<LocalizedCategory> LocalizedCategories { get; set; }
        public DbSet<LocalizedManufacturer> LocalizedManufacturers { get; set; }
        public DbSet<LocalizedProductAttribute> LocalizedProductAttributes { get; set; }
        public DbSet<LocalizedProduct> LocalizedProducts { get; set; }
        public DbSet<LocalizedProductVariantAttributeValue> LocalizedProductVariantAttributeValues { get; set; }
        public DbSet<LocalizedProductVariant> LocalizedProductVariants { get; set; }
        public DbSet<Log> Log { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductManufacturer> ProductManufacturers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductPicture> ProductPictures { get; set; }
        public DbSet<ProductVariantAttribute> ProductVariantAttributes { get; set; }
        public DbSet<ProductVariantAttributeCombination> ProductVariantAttributeCombinations { get; set; }
        public DbSet<ProductVariantAttributeValue> ProductVariantAttributeValues { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<Setting> Settings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //dynamically load all configuration
            System.Type configType = typeof(LanguageMap);   //any of your configuration classes here
            var typesToRegister = Assembly.GetAssembly(configType).GetTypes()
            .Where(type => type.Namespace != null && type.Namespace.Equals(configType.Namespace))
            .Where(type => type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
            //...or do it manually below. For example,
            //modelBuilder.Configurations.Add(new LanguageMap());


            base.OnModelCreating(modelBuilder);
        }

        public string CreateDatabaseScript() {
            return ((IObjectContextAdapter)this).ObjectContext.CreateDatabaseScript();
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity  {
            return base.Set<TEntity>();
        }
    }
}