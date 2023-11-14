namespace CryptoQuest.SaveSystem
{
    public interface ISaveObject
    {
        /// <summary>
        /// Serialize key for json
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Save data to json string
        /// </summary>
        /// <returns> json string represent the data </returns>
        string ToJson();

        /// <summary>
        /// Load data from json string
        /// </summary>
        /// <param name="json"> The json data </param>
        /// <returns> true if data loaded successfully, else false. </returns>
        bool FromJson(string json);
    }
}
