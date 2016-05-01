namespace CGbR
{
    /// <summary>
    /// Interface that declares all helper methods of the binary serializer
    /// </summary>
    public interface IClassSerializationTools
    {
        /// <summary>
        /// Fetch the fixed size of a class reference
        /// </summary>
        /// <param name="model">Class model</param>
        /// <param name="property">Property with class reference</param>
        /// <returns>Fixed size of property if found, otherwise falls</returns>
        int FixedSize(ClassModel model, PropertyModel property);

        /// <summary>
        /// Get binary size of a referenced class
        /// </summary>
        /// <param name="model">Class that holds the reference</param>
        /// <param name="property">Property that references the class</param>
        /// <returns>Size calculation based on property</returns>
        string ReferenceSize(ClassModel model, PropertyModel property);

        /// <summary>
        /// Generate the class to bytes conversion
        /// </summary>
        /// <param name="model">Target class model</param>
        /// <param name="property">Property that references the class</param>
        /// <returns>Conversion string</returns>
        string ClassToBytes(ClassModel model, PropertyModel property);

        /// <summary>
        /// Generate conversion from bytes to class instance
        /// </summary>
        /// <param name="model">Class to generate for</param>
        /// <param name="property">Property that holds the refrence</param>
        /// <returns>Conversion string</returns>
        string ClassFromBytes(ClassModel model, PropertyModel property);
    }
}