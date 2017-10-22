﻿using Globalmantics.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Globalmantics.DAL
{
	public class GlobalmanticsContext : DbContext
    {
        public GlobalmanticsContext() :
            base("GlobalmanticsContext")
        { }

        public DbSet<User> Users { get; set; }
		public DbSet<Cart> Carts { get; set; }
		public DbSet<CatalogItem> CatalogItems { get; set; }


		protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<User>()
                .Property(x => x.Email)
                .HasMaxLength(100)
                .IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(
                    new IndexAttribute("IX_U_Email") { IsUnique = true }));

			modelBuilder.Entity<CatalogItem>()
				.Property(x => x.Description)
				.HasMaxLength(100)
				.IsRequired();
			modelBuilder.Entity<CatalogItem>()
				.Property(x => x.UnitPrice)
				.HasPrecision(18, 2);
			modelBuilder.Entity<CatalogItem>()
				.Property(x => x.Sku)
				.HasMaxLength(20)
				.IsRequired()
				.HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(
					new IndexAttribute("IX_U_Sku") { IsUnique = true }));

			modelBuilder.Entity<CartItem>()
				.HasRequired(x => x.CatalogItem)
				.WithMany()
				.WillCascadeOnDelete();
        }
    }
}
