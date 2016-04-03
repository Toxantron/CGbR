using System;
using System.Collections.Generic;

namespace CGbR
{
	/// <summary>
	/// Model representation of an attribute on a class
	/// </summary>
	public class AttributeModel : CodeElementModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CGbR.AttributeModel"/> class.
		/// </summary>
		/// <param name="name">Name of the attribute</param>
		public AttributeModel (string name) : base(name)
		{			
            Parameters = new List<string>();
            Properties = new List<PropertyModel>();
		}

        /// <summary>
        /// Parameters set in attribute constructor
        /// </summary>
	    public IList<string> Parameters { get; private set; }

        /// <summary>
        /// Properties of the attribute
        /// </summary>
	    public IList<PropertyModel> Properties { get; private set; }
	}
}

