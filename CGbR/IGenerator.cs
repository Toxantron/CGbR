using System;

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

		/// <summary>
		/// Check if this generator has any additions to this class
		/// </summary>
		/// <returns><c>true</c> if this instance can extend the specified model; otherwise, <c>false</c>.</returns>
		/// <param name="model">Class model that shall be extended</param>
		bool CanExtend(ClassModel model);
	}
}

