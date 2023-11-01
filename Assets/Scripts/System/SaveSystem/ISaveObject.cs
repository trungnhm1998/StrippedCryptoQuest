using IndiGames.Core.SaveSystem;
using System;
using System.Collections;

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
    /// <param name="callback"> The callback function </param>
    /// <returns> true if data loaded sucessfully, else false. </returns>
    IEnumerator CoFromJson(string json, Action<bool> callback = null);
}
