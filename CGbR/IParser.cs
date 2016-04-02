using System;

namespace CGbR
{
	/// <summary>
	/// Parser implementation used to parse class model from the text file
	/// </summary>
	public interface IParser
	{
		/// <summary>
		/// Parse all classes from a file
		/// </summary>
		ClassModel ParseFile(string file);
	}
}

