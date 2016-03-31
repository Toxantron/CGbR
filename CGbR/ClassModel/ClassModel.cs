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
			Attributes = new List<AttributeModel> ();
		}

		/// <summary>
		/// Attributes defined for this class
		/// </summary>
		/// <value>The attributes.</value>
		public IList<AttributeModel> Attributes 
		{
			get;
			set;
		}
	}
}

