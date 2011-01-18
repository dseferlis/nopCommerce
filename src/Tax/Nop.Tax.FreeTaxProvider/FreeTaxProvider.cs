﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nop.Services.Tax;
using Nop.Services.Configuration;

namespace Nop.Tax.FreeTaxProvider
{/// <summary>
    /// Free tax provider
    /// </summary>
    public class FreeTaxProvider : ITaxProvider
    {
        /// <summary>
        /// Gets or sets the friendly name
        /// </summary>
        public string FriendlyName
        {
            get
            {
                //TODO localize
                return "Free tax rate provider";
            }
        }

        /// <summary>
        /// Gets or sets the system name
        /// </summary>
        public string SystemName
        {
            get
            {
                return "FreeTaxRate";
            }
        }

        /// <summary>
        /// Gets or sets the setting service
        /// </summary>
        public ISettingService SettingService { get; set; }

        /// <summary>
        /// Gets tax rate
        /// </summary>
        /// <param name="calculateTaxRequest">Tax calculation request</param>
        /// <returns>Tax</returns>
        public CalculateTaxResult GetTaxRate(CalculateTaxRequest calculateTaxRequest)
        {
            var result = new CalculateTaxResult()
            {
                 TaxRate = decimal.Zero
            };
            return result;
        }
    }
}
