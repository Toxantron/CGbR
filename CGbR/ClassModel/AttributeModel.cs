using System;

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
		}
	}
}

