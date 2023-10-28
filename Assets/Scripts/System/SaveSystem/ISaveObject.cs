using IndiGames.Core.SaveSystem;

public interface ISaveObject: IJsonSerializable
{
    /// <summary>
    /// Serialize key for json
    /// </summary>
    string Key { get; }

    /// <summary>
    /// Check if the loading coroutine has finished on this object
    /// </summary>
    /// <returns>true/false</returns>
    bool IsLoaded();
}
