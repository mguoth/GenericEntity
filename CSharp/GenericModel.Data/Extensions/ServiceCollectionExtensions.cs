using GenericModel.Data.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace GenericModel.Data
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
