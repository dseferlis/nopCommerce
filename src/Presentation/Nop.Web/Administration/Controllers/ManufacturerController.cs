﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nop.Admin.Models;
using Nop.Admin.Models.Catalog;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Discounts;
using Nop.Services.Catalog;
using Nop.Services.Discounts;
using Nop.Services.ExportImport;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Security.Permissions;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.UI;

namespace Nop.Admin.Controllers
{
    [AdminAuthorize]
    public class ManufacturerController : BaseNopController
    {
        #region Fields

        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufacturerService;
        private readonly IProductService _productService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IExportManager _exportManager;
        private readonly IWorkContext _workContext;

        #endregion
        
        #region Constructors

        public ManufacturerController(ICategoryService categoryService, IManufacturerService manufacturerService,
            IProductService productService,  ILanguageService languageService,
            ILocalizationService localizationService, ILocalizedEntityService localizedEntityService,
            IExportManager exportManager, IWorkContext workContext)
        {
            this._categoryService = categoryService;
            this._manufacturerService = manufacturerService;
            this._productService = productService;
            this._languageService = languageService;
            this._localizationService = localizationService;
            this._localizedEntityService = localizedEntityService;
            this._exportManager = exportManager;
            this._workContext = workContext;
        }

        #endregion Constructors
        
        #region Utilities

        [NonAction]
        public void UpdateLocales(Manufacturer manufacturer, ManufacturerModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(manufacturer,
                                                               x => x.Name,
                                                               localized.Name,
                                                               localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(manufacturer,
                                                           x => x.Description,
                                                           localized.Description,
                                                           localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(manufacturer,
                                                           x => x.MetaKeywords,
                                                           localized.MetaKeywords,
                                                           localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(manufacturer,
                                                           x => x.MetaDescription,
                                                           localized.MetaDescription,
                                                           localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(manufacturer,
                                                           x => x.MetaTitle,
                                                           localized.MetaTitle,
                                                           localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(manufacturer,
                                                           x => x.SeName,
                                                           localized.SeName,
                                                           localized.LanguageId);
            }
        }
        
        #endregion
        
        #region List

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            var manufacturers = _manufacturerService.GetAllManufacturers(0, 10, true);
            var gridModel = new GridModel<ManufacturerModel>
            {
                Data = manufacturers.Select(x =>x.ToModel()),
                Total = manufacturers.TotalCount
            };
            return View(gridModel);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult List(GridCommand command)
        {
            var manufacturers = _manufacturerService.GetAllManufacturers(command.Page - 1, command.PageSize, true);
            var gridModel = new GridModel<ManufacturerModel>
            {
                Data = manufacturers.Select(x => x.ToModel()),
                Total = manufacturers.TotalCount
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        #endregion

        #region Create / Edit / Delete

        public ActionResult Create()
        {
            var model = new ManufacturerModel();
            //locales
            AddLocales(_languageService, model.Locales);
            //default values
            model.PageSize = 4;
            model.Published = true;
            return View(model);
        }

        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        public ActionResult Create(ManufacturerModel model, bool continueEditing)
        {
            //decode description
            model.Description = HttpUtility.HtmlDecode(model.Description);
            foreach (var localized in model.Locales)
                localized.Description = HttpUtility.HtmlDecode(localized.Description);

            if (ModelState.IsValid)
            {
                var manufacturer = model.ToEntity();
                manufacturer.CreatedOnUtc = DateTime.UtcNow;
                manufacturer.UpdatedOnUtc = DateTime.UtcNow;
                _manufacturerService.InsertManufacturer(manufacturer);
                //locales
                UpdateLocales(manufacturer, model);

                SuccessNotification(_localizationService.GetResource("Admin.Catalog.Manufacturers.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = manufacturer.Id }) : RedirectToAction("List");
            }

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var manufacturer = _manufacturerService.GetManufacturerById(id);
            if (manufacturer == null || manufacturer.Deleted)
                throw new ArgumentException("No manufacturer found with the specified id", "id");
            var model = manufacturer.ToModel();
            //locales
            AddLocales(_languageService, model.Locales, (locale, languageId) =>
            {
                locale.Name = manufacturer.GetLocalized(x => x.Name, languageId, false, false);
                locale.Description = manufacturer.GetLocalized(x => x.Description, languageId, false, false);
                locale.MetaKeywords = manufacturer.GetLocalized(x => x.MetaKeywords, languageId, false, false);
                locale.MetaDescription = manufacturer.GetLocalized(x => x.MetaDescription, languageId, false, false);
                locale.MetaTitle = manufacturer.GetLocalized(x => x.MetaTitle, languageId, false, false);
                locale.SeName = manufacturer.GetLocalized(x => x.SeName, languageId, false, false);
            });

            return View(model);
        }

        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        public ActionResult Edit(ManufacturerModel model, bool continueEditing)
        {
            var manufacturer = _manufacturerService.GetManufacturerById(model.Id);
            if (manufacturer == null || manufacturer.Deleted)
                throw new ArgumentException("No manufacturer found with the specified id");

            //decode description
            model.Description = HttpUtility.HtmlDecode(model.Description);
            foreach (var localized in model.Locales)
                localized.Description = HttpUtility.HtmlDecode(localized.Description);


            if (ModelState.IsValid)
            {
                manufacturer = model.ToEntity(manufacturer);
                manufacturer.UpdatedOnUtc = DateTime.UtcNow;
                _manufacturerService.UpdateManufacturer(manufacturer);
                //locales
                UpdateLocales(manufacturer, model);

                SuccessNotification(_localizationService.GetResource("Admin.Catalog.Manufacturers.Updated"));
                return continueEditing ? RedirectToAction("Edit", manufacturer.Id) : RedirectToAction("List");
            }

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var manufacturer = _manufacturerService.GetManufacturerById(id);
            _manufacturerService.DeleteManufacturer(manufacturer);

            SuccessNotification(_localizationService.GetResource("Admin.Catalog.Manufacturers.Deleted"));
            return RedirectToAction("List");
        }
        
        #endregion

        #region Export/Import

        public ActionResult Export()
        {
            var fileName = string.Format("manufacturers_{0}.xml", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
            var manufacturers = _manufacturerService.GetAllManufacturers(true);
            var xml = _exportManager.ExportManufacturersToXml(manufacturers);
            return new XmlDownloadResult(xml, fileName);
            //TODO why return file has such a weird extension (Chrome)
        }

        #endregion

        #region Products

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult ProductList(GridCommand command, int manufacturerId)
        {
            var productManufacturers = _manufacturerService.GetProductManufacturersByManufacturerId(manufacturerId, true);
            var productManufacturersModel = productManufacturers
                .Select(x =>
                {
                    return new ManufacturerModel.ManufacturerProductModel()
                    {
                        Id = x.Id,
                        ManufacturerId = x.ManufacturerId,
                        ProductId = x.ProductId,
                        ProductName = _productService.GetProductById(x.ProductId).Name,
                        IsFeaturedProduct = x.IsFeaturedProduct,
                        DisplayOrder1 = x.DisplayOrder
                    };
                })
                .ToList();

            var model = new GridModel<ManufacturerModel.ManufacturerProductModel>
            {
                Data = productManufacturersModel,
                Total = productManufacturersModel.Count
            };

            return new JsonResult
            {
                Data = model
            };
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult ProductUpdate(GridCommand command, ManufacturerModel.ManufacturerProductModel model)
        {
            var productManufacturer = _manufacturerService.GetProductManufacturerById(model.Id);
            if (productManufacturer == null)
                throw new ArgumentException("No product manufacturer mapping found with the specified id");

            productManufacturer.IsFeaturedProduct = model.IsFeaturedProduct;
            productManufacturer.DisplayOrder = model.DisplayOrder1;
            _manufacturerService.UpdateProductManufacturer(productManufacturer);

            return ProductList(command, model.ManufacturerId);
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult ProductDelete(int id, GridCommand command)
        {
            var productManufacturer = _manufacturerService.GetProductManufacturerById(id);
            if (productManufacturer == null)
                throw new ArgumentException("No product manufacturer mapping found with the specified id");

            var manufacturerId = productManufacturer.ManufacturerId;
            _manufacturerService.DeleteProductManufacturer(productManufacturer);

            return ProductList(command, manufacturerId);
        }

        public ActionResult ProductAddPopup(int manufacturerId)
        {
            var products = _productService.SearchProducts(0, 0, null, null, null, 0, 0, string.Empty, false,
                _workContext.WorkingLanguage.Id, new List<int>(),
                ProductSortingEnum.Position, 0, 10, true);

            var model = new ManufacturerModel.AddManufacturerProductModel();
            model.Products = new GridModel<ProductModel>
            {
                Data = products.Select(x => x.ToModel()),
                Total = products.TotalCount
            };
            //categories
            model.AvailableCategories.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            foreach (var c in _categoryService.GetAllCategories(true))
                model.AvailableCategories.Add(new SelectListItem() { Text = c.GetCategoryNameWithPrefix(_categoryService), Value = c.Id.ToString() });

            //manufacturers
            model.AvailableManufacturers.Add(new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            foreach (var m in _manufacturerService.GetAllManufacturers(true))
                model.AvailableManufacturers.Add(new SelectListItem() { Text = m.Name, Value = m.Id.ToString() });

            return View(model);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult ProductAddPopupList(GridCommand command, ManufacturerModel.AddManufacturerProductModel model)
        {
            var gridModel = new GridModel();
            var products = _productService.SearchProducts(model.SearchCategoryId,
                model.SearchManufacturerId, null, null, null, 0, 0, model.SearchProductName, false,
                _workContext.WorkingLanguage.Id, new List<int>(),
                ProductSortingEnum.Position, command.Page - 1, command.PageSize, true);
            gridModel.Data = products.Select(x => x.ToModel());
            gridModel.Total = products.TotalCount;
            return new JsonResult
            {
                Data = gridModel
            };
        }
        
        [HttpPost]
        [FormValueRequired("save")]
        public ActionResult ProductAddPopup(string btnId, ManufacturerModel.AddManufacturerProductModel model)
        {
            if (model.SelectedProductIds != null)
            {
                foreach (int id in model.SelectedProductIds)
                {
                    var product = _productService.GetProductById(id);
                    if (product != null)
                    {
                        var existingProductmanufacturers = _manufacturerService.GetProductManufacturersByManufacturerId(model.ManufacturerId);
                        if (existingProductmanufacturers.FindProductManufacturer(id, model.ManufacturerId) == null)
                        {
                            _manufacturerService.InsertProductManufacturer(
                                new ProductManufacturer()
                                {
                                    ManufacturerId = model.ManufacturerId,
                                    ProductId = id,
                                    IsFeaturedProduct = false,
                                    DisplayOrder = 1
                                });
                        }
                    }
                }
            }

            ViewBag.RefreshPage = true;
            ViewBag.btnId = btnId;
            model.Products = new GridModel<ProductModel>();
            return View(model);
        }

        #endregion
    }
}
