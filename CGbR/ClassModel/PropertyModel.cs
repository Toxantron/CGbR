namespace CGbR
{
    /// <summary>
    /// Class representing a property
    /// </summary>
    public class PropertyModel : CodeElementModel
    {
        /// <summary>
        /// Initialize property model with its name
        /// </summary>
        /// <param name="name">Name of the property</param>
        public PropertyModel(string name) : base(name)
        {
        }

        /// <summary>
        /// Value of the property if set
        /// </summary>
        public string Value { get; set; }
    }
}