using System.Linq;
using System.Reflection;
using Abp.Application.Services;
using Abp.AspNetCore.Configuration;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Ayu.Core.MVC.Providers
{
    public class AppServiceControllerFeatureProvider : ControllerFeatureProvider
    {
        private readonly IIocResolver _iocResolver;

        public AppServiceControllerFeatureProvider(IIocResolver iocResolver)
        {
            _iocResolver = iocResolver;
        }

        protected override bool IsController(TypeInfo typeInfo)
        {
            var type = typeInfo.AsType();

            if (!typeof(IApplicationService).IsAssignableFrom(type) ||
                !typeInfo.IsPublic || typeInfo.IsAbstract || typeInfo.IsGenericType)
            {
                return false;
            }

            var remoteServiceAttr = ReflectionHelper.GetSingleAttributeOrDefault<RemoteServiceAttribute>(typeInfo);

            if (remoteServiceAttr != null && !remoteServiceAttr.IsEnabledFor(type))
            {
                return false;
            }

            var settings = _iocResolver.Resolve<AbpAspNetCoreConfiguration>().ControllerAssemblySettings.GetSettings(type);
            return settings.Any(setting => setting.TypePredicate(type));
        }
    }
}
