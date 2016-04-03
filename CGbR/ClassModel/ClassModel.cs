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
            References = new List<ClassModel>();
		}

        /// <summary>
        /// Namespace the class is located in
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Reference to the base class
        /// </summary>
	    public string BaseClass { get; set; }

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
	    public IList<ClassModel> References { get; set; }
	}
}

