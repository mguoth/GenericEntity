using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Org.GenericEntity.Abstractions;

namespace Org.GenericEntity.Model
{
    /// <summary>
    /// Field collection
    /// </summary>
    /// <seealso cref="IField" />
    public class FieldCollection : IEnumerable<IField>
    {
        private readonly IDictionary<string, IField> fields;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldCollection" /> class.
        /// </summary>
        /// <param name="fields">The fields.</param>
        /// <param name="caseInsensitive">Determines whether a field name is case insensitive</param>
        internal FieldCollection(IEnumerable<IField> fields, bool caseInsensitive)
        {
            this.CaseInsensitive = caseInsensitive;

            if (caseInsensitive)
            {
                this.fields = fields.ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);
            }
            else
            {
                this.fields = fields.ToDictionary(x => x.Name);
            }
        }

        /// <summary>
        /// Gets a value indicating whether field name is case insensitive.
        /// </summary>
        public bool CaseInsensitive { get; }

        /// <summary>
        /// Gets the <see cref="IField" /> with the specified name.
        /// </summary>
        /// <value>
        /// The <see cref="IField"/>.
        /// </value>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">@"The field name ""{fieldName}"" doesn't exist in the field collection")</exception>
        public IField this[string fieldName]
        {
            get
            {
                IField field;
                if (this.fields.TryGetValue(fieldName, out field))
                {
                    return field;
                }
                
                throw new KeyNotFoundException($@"The field name ""{fieldName}"" doesn't exist in the field collection");
            }
        }

        /// <summary>
        /// Returns this instance in case it is case sensitive, otherwise returns case sensitive wrapper for this instance.
        /// </summary>
        /// <returns></returns>
        internal FieldCollection AsCaseSensitive()
        {
            if (!this.CaseInsensitive)
            {
                return this;
            }
            return new FieldCollection(this, false);
        }

        /// <summary>
        /// Returns this instance in case it is case insensitive, otherwise returns case insensitive wrapper for this instanc.
        /// </summary>
        /// <returns></returns>
        internal FieldCollection AsCaseInsensitive()
        {
            if (this.CaseInsensitive)
            {
                return this;
            }
            return new FieldCollection(this, true);
        }

        /// <summary>
        /// Gets the <see cref="IField" /> with the specified name.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="field">When this method returns, the field associated with the specified name, if the name is found; otherwise null.</param>
        /// <returns>
        /// true in case the <see cref="IField" /> with specified name exists; otherwise false
        /// </returns>
        public bool TryGetField(string fieldName, out IField field)
        {
            return this.fields.TryGetValue(fieldName, out field);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        public IEnumerator<IField> GetEnumerator()
        {
            return this.fields.Values.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
