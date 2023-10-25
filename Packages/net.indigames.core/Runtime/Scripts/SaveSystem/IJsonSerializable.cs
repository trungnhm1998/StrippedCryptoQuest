namespace IndiGames.Core.SaveSystem
{
    /// <summary>
    /// Interface for object that can be serialized to json, for SaveGame system
    /// </summary>
    public interface IJsonSerializable
    {

        /// <summary>
        /// Save data to json string
        /// </summary>
        /// <returns> json string represent the data </returns>
        string ToJson();

        /// <summary>
        /// Load data from json string
        /// </summary>
        /// <param name="json"> The json data </param>
        /// <returns> true if data loaded sucessfully, else false. </returns>
        bool FromJson(string json);
    }
}