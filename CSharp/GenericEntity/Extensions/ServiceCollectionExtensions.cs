using GenericEntity.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace GenericEntity
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGenericEntityExtensions(this IServiceCollection services, Action<IGenericEntityExtensions> configureDelegate)
        {
            GenericEntityExtensions extensions = new GenericEntityExtensions();
            configureDelegate(extensions);

            services.AddSingleton<IGenericEntityExtensions>(extensions);
            GenericEntity.DefaultExtensions = extensions;
            return services;
        }
    }
}
