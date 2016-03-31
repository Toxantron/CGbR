using System;

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
		}

		/// <summary>
		/// Name of the element
		/// </summary>
		public string Name 
		{
			get;
			set;
		}
	}
}

