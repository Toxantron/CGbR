using System;
using System.Collections.Generic;

namespace CGbR
{
	/// <summary>
	/// Object representation of a class
	/// </summary>
	public class ClassModel : CodeElementModel
	{
		/// <summary>
		/// Initialize a new class model
		/// </summary>
		public ClassModel (string name) : base(name)
		{
            Properties = new List<PropertyModel>();
            References = new List<CodeElementModel>();
		}

        /// <summary>
        /// Reference to the base class
        /// </summary>
	    public string BaseClass { get; set; }

		/// <summary>
		/// Flag if this is a partial class
		/// </summary>
		/// <value><c>true</c> if this instance is partial; otherwise, <c>false</c>.</value>
		public bool IsPartial { get; set; }

        /// <summary>
        /// Access modifier of this type
        /// </summary>
	    public AccessModifier AccessModifier { get; set; }

        /// <summary>
        /// Interfaces of this class
        /// </summary>
        public string[] Interfaces { get; set; }

	    /// <summary>
	    /// All properties of the class
	    /// </summary>
	    public IList<PropertyModel> Properties { get; private set; }

        /// <summary>
        /// Classes referenced by this class
        /// </summary>
	    public IList<CodeElementModel> References { get; set; }
	}
}

