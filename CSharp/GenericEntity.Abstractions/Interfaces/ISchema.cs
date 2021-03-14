using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace GenericEntity.Abstractions
{
    public interface ISchema
    {      
        string Namespace { get; }
        string EntityType { get; }
        IList<IFieldDefinition> Fields { get; }
    }
}
