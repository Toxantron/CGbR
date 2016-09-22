using System;

namespace CGbR.Lib
{
    /// <summary>
    /// Decorate a class with this attribute to activate
    /// the clone generator
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CloneableAttribute : Attribute
    {
    }

    /// <summary>
    /// Attribute to signal CGbR this property shall be ignored
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class CloneIgnoreAttribute : Attribute
    {
    }

    /// <summary>
    /// Modifiy the way reference types are cloned
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class ReferenceCloneAttribute : Attribute
    {
        /// <summary>
        /// Mode the generator shall use
        /// </summary>
        public CloneMode Mode { get; private set; }

        /// <summary>
        /// Decorate a member with the <see cref="ReferenceCloneAttribute"/> to alter the cloning behavior
        /// </summary>
        public ReferenceCloneAttribute(CloneMode mode)
        {
            Mode = mode;
        }
    }

    /// <summary>
    /// Cloning mode that shall be used
    /// </summary>
    public enum CloneMode
    {
        /// <summary>
        /// Copy only the reference. This is the fastet way
        /// </summary>
        Reference,
        /// <summary>
        /// Create a new object using a flat copy
        /// </summary>
        Flat,
        /// <summary>
        /// Create a deep copy of the referenced object
        /// </summary>
        Deep
    }
}