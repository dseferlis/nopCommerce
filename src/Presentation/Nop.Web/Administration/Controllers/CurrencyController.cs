﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nop.Admin;
using Nop.Admin.Models;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Localization;
using Nop.Services.Configuration;
using Nop.Web.Framework.Controllers;
using Nop.Services.Directory;
using Telerik.Web.Mvc;

namespace Nop.Admin.Controllers
{
    [AdminAuthorize]
    public class CurrencyController : Controller
    {
        private ICurrencyService _currencyService;
        private CurrencySettings _currencySettings;
        private ISettingService _settingService;
        public CurrencyController(ICurrencyService currencyService, CurrencySettings currencySettings, ISettingService settingService)
        {
            _currencyService = currencyService;
            _currencySettings = currencySettings;
            _settingService = settingService;
        }

        #region Methods
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List(bool liveRates=false)
        {
            var currenciesModel = _currencyService.GetAllCurrencies(true).Select(x => x.ToModel()).ToList(); ;
            foreach (var currency in currenciesModel)
                currency.IsPrimaryExchangeRateCurrency = currency.Id == _currencySettings.PrimaryExchangeRateCurrencyId ? true : false;
            foreach (var currency in currenciesModel)
                currency.IsPrimaryStoreCurrency = currency.Id == _currencySettings.PrimaryStoreCurrencyId ? true : false;
            if (liveRates) ViewBag.Rates = _currencyService.GetCurrencyLiveRates("EUR");//_currencyService.GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId).CurrencyCode);
            ViewBag.ExchangeRateProviders = new SelectList(_currencyService.LoadAllExchangeRateProviders(), "SystemName", "FriendlyName", _currencySettings.ActiveExchangeRateProviderSystemName); ;
            ViewBag.AutoUpdateEnabled = _currencySettings.AutoUpdateEnabled;
            var gridModel = new GridModel<CurrencyModel>
            {
                Data = currenciesModel,
                Total = currenciesModel.Count()
            };
            return View(gridModel);
        }

        public ActionResult ApplyRate(string currencyCode, decimal rate)
        {
            Currency currency = _currencyService.GetCurrencyByCode(currencyCode);
            if (currency != null)
            {
                currency.Rate = rate;
                currency.UpdatedOnUtc = DateTime.UtcNow;
                _currencyService.UpdateCurrency(currency);
            }
            return RedirectToAction("List","Currency", new { liveRates=true });
        }

        [HttpPost]
        public ActionResult Save(FormCollection formValues)
        {
            _currencySettings.ActiveExchangeRateProviderSystemName = formValues["exchangeRateProvider"];
            _currencySettings.AutoUpdateEnabled = formValues["autoUpdateEnabled"].Equals("false")?false:true;
            _settingService.SaveSetting(_currencySettings);
            return RedirectToAction("List","Currency");
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult List(GridCommand command)
        {
            var currencies = _currencyService.GetAllCurrencies(true);
            var gridModel = new GridModel<CurrencyModel>
            {
                Data = currencies.Select(x => x.ToModel()),
                Total = currencies.Count()
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }


        public ActionResult MarkAsPrimaryExchangeRateCurrency(int id)
        {
            _currencySettings.PrimaryExchangeRateCurrencyId = id;
            _settingService.SaveSetting(_currencySettings);
            return RedirectToAction("List");
        }

        public ActionResult MarkAsPrimaryStoreCurrency(int id)
        {
            _currencySettings.PrimaryStoreCurrencyId = id;
            _settingService.SaveSetting(_currencySettings);
            return RedirectToAction("List");
        }

        #endregion

        #region Edit
        public ActionResult Edit(int id)
        {
            var currency = _currencyService.GetCurrencyById(id);
            if (currency == null) throw new ArgumentException("No currency found with the specified id", "id");
            var model = currency.ToModel();
            return View(model);
        }

        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        public ActionResult Edit(CurrencyModel currencyModel, bool continueEditing)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var currency = _currencyService.GetCurrencyById(currencyModel.Id);
            currencyModel.CreatedOnUtc = currency.CreatedOnUtc;
            currency = currencyModel.ToEntity(currency);
            currency.UpdatedOnUtc = DateTime.UtcNow;
            _currencyService.UpdateCurrency(currency);
            return continueEditing ? RedirectToAction("Edit", new { id = currency.Id }) : RedirectToAction("List");
        }

        #endregion

        #region Create

        public ActionResult Create()
        {
            var currencyModel = new CurrencyModel();
            return View(currencyModel);
        }

        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        public ActionResult Create(CurrencyModel model, bool continueEditing)
        {
            model.CreatedOnUtc = DateTime.UtcNow;
            model.UpdatedOnUtc = DateTime.UtcNow;
            var currency = model.ToEntity();
            _currencyService.InsertCurrency(currency);
            return continueEditing ? RedirectToAction("Edit", new { id = currency.Id }) : RedirectToAction("List");
        }

        #endregion

        #region Delete


        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var currency = _currencyService.GetCurrencyById(id);
            _currencyService.DeleteCurrency(currency);
            return RedirectToAction("List");
        }

        #endregion
    }
}
