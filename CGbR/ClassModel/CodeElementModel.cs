using System;
using System.Collections.Generic;

namespace CGbR
{
	/// <summary>
	/// Base class for all code elements
	/// </summary>
	public class CodeElementModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CGbR.CodeElementModel"/> class.
		/// </summary>
		/// <param name="name">Name of the element</param>
		public CodeElementModel (string name)
		{
			Name = name;
            Attributes = new List<AttributeModel>();
        }

		/// <summary>
		/// Name of the element
		/// </summary>
		public string Name { get; set; }

        /// <summary>
        /// Attributes defined for this class
        /// </summary>
        /// <value>The attributes.</value>
        public IList<AttributeModel> Attributes { get; private set; }
    }
}

