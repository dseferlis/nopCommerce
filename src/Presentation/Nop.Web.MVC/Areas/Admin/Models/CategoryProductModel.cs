﻿using System;
using System.ComponentModel.DataAnnotations;
using Nop.Core.Domain.Catalog;
using Nop.Web.MVC.Areas.Admin.Validators;
using FluentValidation.Attributes;
namespace Nop.Web.MVC.Areas.Admin.Models
{
    [Validator(typeof(CategoryProductValidator))]
    public class CategoryProductModel : IEquatable<CategoryProductModel>
    {
        public CategoryProductModel()
        {
            
        }

        public CategoryProductModel(ProductCategory productCategory)
        {
            Id = productCategory.Id;
            ProductId = productCategory.ProductId;
            CategoryId = productCategory.CategoryId;
            IsFeaturedProduct = productCategory.IsFeaturedProduct;
            DisplayOrder = productCategory.DisplayOrder;
        }

        [ScaffoldColumn(false)]
        public int Id { get; set; }
        [UIHint("ProductSelector")]
        public int ProductId { get; set; }
        [ScaffoldColumn(false)]
        public int CategoryId { get; set; }
        public bool IsFeaturedProduct { get; set; }
        public int DisplayOrder { get; set; }

        public override bool Equals(object obj)
        {
            var productCategory = obj as ProductCategory;
            if (productCategory != null)
            {
                return productCategory.ProductId.Equals(ProductId);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return ProductId.GetHashCode();
        }

        public bool Equals(CategoryProductModel other)
        {
            return other.ProductId.Equals(ProductId);
        }
    }
}