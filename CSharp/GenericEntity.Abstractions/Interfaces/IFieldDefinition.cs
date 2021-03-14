using System;
using System.Collections.Generic;
using System.Text;

namespace GenericEntity.Abstractions
{
    public interface IFieldDefinition
    {
        ISchema Schema { get; }
        string Name { get; }
        string Type { get; }
        
        string DisplayName { get; }
        string Description { get; }
    }
}
