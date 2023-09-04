using Org.GenericEntity.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.GenericEntity.Model
{
    /// <summary>
    /// Field collection builder
    /// </summary>
    internal class FieldCollectionBuilder
    {
        private IList<IField> fields = new List<IField>();

        /// <summary>
        /// Builds field collection.
        /// </summary>
        /// <param name="caseInsensitive">Determines whether a field name is case insensitive</param>
        /// <returns></returns>
        public FieldCollection Build(bool caseInsensitive)
        {
            return new FieldCollection(this.fields, caseInsensitive);
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
