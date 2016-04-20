namespace CGbR
{
    /// <summary>
    /// The three different access modifiers for classes or members 
    /// </summary>
    public enum AccessModifier
    {
        /// <summary>
        /// Access limited to the class only
        /// </summary>
        Private,

        /// <summary>
        /// Access only to classes of the same assembly
        /// </summary>
        Internal,

        /// <summary>
        /// Access to everyone
        /// </summary>
        Public
    }
}