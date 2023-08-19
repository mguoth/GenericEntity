using Org.GenericEntity.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Org.GenericEntity.Model
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGenericEntityExtensions(this IServiceCollection services, Action<IGenericEntityExtensions> configureDelegate)
        {
            configureDelegate(GenericEntity.Extensions);
            return services;
        }
    }
}
