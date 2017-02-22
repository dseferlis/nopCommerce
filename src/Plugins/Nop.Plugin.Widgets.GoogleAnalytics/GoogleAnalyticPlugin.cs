using System.Collections.Generic;
using System.Web.Routing;
using Nop.Core.Plugins;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;

namespace Nop.Plugin.Widgets.GoogleAnalytics
{
    /// <summary>
    /// Live person provider
    /// </summary>
    public class GoogleAnalyticPlugin : BasePlugin, IWidgetPlugin
    {
        private readonly ISettingService _settingService;
        private readonly GoogleAnalyticsSettings _googleAnalyticsSettings;

        public GoogleAnalyticPlugin(ISettingService settingService, GoogleAnalyticsSettings googleAnalyticsSettings)
        {
            this._settingService = settingService;
            this._googleAnalyticsSettings = googleAnalyticsSettings;
        }

        /// <summary>
        /// Gets widget zones where this widget should be rendered
        /// </summary>
        /// <returns>Widget zones</returns>
        public IList<string> GetWidgetZones()
        {
            return !string.IsNullOrWhiteSpace(_googleAnalyticsSettings.WidgetZone)
                       ? new List<string>() { _googleAnalyticsSettings.WidgetZone }
                       : new List<string>() { "body_end_html_tag_before" };
        }

        /// <summary>
        /// Gets a route for provider configuration
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "WidgetsGoogleAnalytics";
            routeValues = new RouteValueDictionary { { "Namespaces", "Nop.Plugin.Widgets.GoogleAnalytics.Controllers" }, { "area", null } };
        }

        /// <summary>
        /// Gets a route for displaying widget
        /// </summary>
        /// <param name="widgetZone">Widget zone where it's displayed</param>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetDisplayWidgetRoute(string widgetZone, out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "PublicInfo";
            controllerName = "WidgetsGoogleAnalytics";
            routeValues = new RouteValueDictionary
            {
                {"Namespaces", "Nop.Plugin.Widgets.GoogleAnalytics.Controllers"},
                {"area", null},
                {"widgetZone", widgetZone}
            };
        }

        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {
            var settings = new GoogleAnalyticsSettings
            {
                GoogleId = "UA-0000000-0",
                TrackingScript = @"<!-- Google code for Analytics tracking -->
<script type=""text/javascript"">
var _gaq = _gaq || [];
_gaq.push(['_setAccount', '{GOOGLEID}']);
_gaq.push(['_trackPageview']);
{ECOMMERCE}
(function() {
    var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
    ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
})();
</script>",
                EcommerceScript = @"_gaq.push(['_addTrans', '{ORDERID}', '{SITE}', '{TOTAL}', '{TAX}', '{SHIP}', '{CITY}', '{STATEPROVINCE}', '{COUNTRY}']);
{DETAILS} 
_gaq.push(['_trackTrans']); ",
                EcommerceDetailScript = @"_gaq.push(['_addItem', '{ORDERID}', '{PRODUCTSKU}', '{PRODUCTNAME}', '{CATEGORYNAME}', '{UNITPRICE}', '{QUANTITY}' ]); ",

            };
            _settingService.SaveSetting(settings);

            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.GoogleId", "ID");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.GoogleId.Hint", "Enter Google Analytics ID.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.TrackingScript", "Tracking code with {ECOMMERCE} line");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.TrackingScript.Hint", "Paste the tracking code generated by Google Analytics here. {GOOGLEID} and {ECOMMERCE} will be dynamically replaced.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.EcommerceScript", "Tracking code for {ECOMMERCE} part, with {DETAILS} line");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.EcommerceScript.Hint", "Paste the tracking code generated by Google analytics here. {ORDERID}, {SITE}, {TOTAL}, {TAX}, {SHIP}, {CITY}, {STATEPROVINCE}, {COUNTRY}, {DETAILS} will be dynamically replaced.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.EcommerceDetailScript", "Tracking code for {DETAILS} part");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.EcommerceDetailScript.Hint", "Paste the tracking code generated by Google analytics here. {ORDERID}, {PRODUCTSKU}, {PRODUCTNAME}, {CATEGORYNAME}, {UNITPRICE}, {QUANTITY} will be dynamically replaced.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.IncludingTax", "Include tax");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.IncludingTax.Hint", "Check to include tax when generating tracking code for {ECOMMERCE} part.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.Instructions", "<p>Google Analytics is a free website stats tool from Google. It keeps track of statisticsabout the visitors and ecommerce conversion on your website.<br /><br />Follow the next steps to enable Google Analytics integration:<br /><ul><li><a href=\"http://www.google.com/analytics/\" target=\"_blank\">Create a Google Analyticsaccount</a> and follow the wizard to add your website</li><li>Copy the Tracking ID into the 'ID' box below</li><li>Click the 'Save' button below and Google Analytics will be integrated into your store</li></ul><br />If you would like to switch between Google Analytics (used by default) and Universal Analytics, then please use the buttons below:</p>");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.Note", "<p><em>Please note that {ECOMMERCE} line works only when you have \"Disable order completed page\" order setting unticked.</em></p>");

            base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<GoogleAnalyticsSettings>();

            //locales
            this.DeletePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.GoogleId");
            this.DeletePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.GoogleId.Hint");
            this.DeletePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.TrackingScript");
            this.DeletePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.TrackingScript.Hint");
            this.DeletePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.EcommerceScript");
            this.DeletePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.EcommerceScript.Hint");
            this.DeletePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.EcommerceDetailScript");
            this.DeletePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.EcommerceDetailScript.Hint");
            this.DeletePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.IncludingTax");
            this.DeletePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.IncludingTax.Hint");
            this.DeletePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.Instructions");
            this.DeletePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.Note");

            base.Uninstall();
        }
    }
}
