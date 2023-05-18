using GenericModel.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericModel.Data
{
    /// <summary>
    /// Field collection builder
    /// </summary>
    public class FieldCollectionBuilder
    {
        private IList<IField> fields = new List<IField>();

        public FieldCollection Build()
        {
            return new FieldCollection(this.fields);
        }

        /// <summary>
        /// Add field
        /// </summary>
        /// <param name="field">The field.</param>
        public void Add(IField field)
        {
            this.fields.Add(field);
        }
    }
}
