﻿using Nop.Core;
using Nop.Core.Infrastructure;
using Nop.Services.Localization;
using Nop.Web.Framework.Mvc;

namespace Nop.Web.Framework
{
    public class NopResourceDisplayName : System.ComponentModel.DisplayNameAttribute, IModelAttribute
    {
        private string _resourceValue = string.Empty;
        private bool _resourceValueRetrived;

        public NopResourceDisplayName(string resourceKey)
            : base(resourceKey)
        {
            ResourceKey = resourceKey;
        }

        public string ResourceKey { get; set; }

        public override string DisplayName
        {
            get
            {
                if (!_resourceValueRetrived)
                {
                    _resourceValue = EngineContext.Current.Resolve<ILocalizationService>().GetResource(ResourceKey,
                                                                                     EngineContext.Current.Resolve
                                                                                         <IWorkContext>().
                                                                                         WorkingLanguage.Id, true,
                                                                                     ResourceKey);
                    _resourceValueRetrived = true;
                }
                return _resourceValue;
            }
        }

        public string Name
        {
            get { return "NopResourceDisplayName"; }
        }
    }
}
