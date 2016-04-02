﻿using System;

namespace CGbR
{
	/// <summary>
	/// Contract for all generator implementations
	/// </summary>
	public interface IGenerator
	{
		/// <summary>
		/// Name of this generator
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Using the generated code requires
		/// </summary>
		string[] Usings { get; }
	}
}

