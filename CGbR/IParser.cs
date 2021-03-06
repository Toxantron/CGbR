﻿using System;

namespace CGbR
{
	/// <summary>
	/// Parser implementation used to parse class model from the text file
	/// </summary>
	public interface IParser
	{
        /// <summary>
        /// Name of the parsers
        /// </summary>
        string Name { get; }

		/// <summary>
		/// Parse all classes from a file
		/// </summary>
		CodeElementModel ParseFile(string file);
	}
}

