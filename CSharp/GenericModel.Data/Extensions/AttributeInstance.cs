using System;
using System.Collections.Generic;
using System.Text;

namespace GenericModel.Data
{
    public class AttributeInstance<TAttribute>
    {
        public AttributeInstance(Type type, TAttribute attribute)
        {
            this.Type = type;
            this.Attribute = attribute;
        }

        public Type Type { get; }
        public TAttribute Attribute { get; }
    }
}
