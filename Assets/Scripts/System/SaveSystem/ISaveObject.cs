using IndiGames.Core.SaveSystem;

public interface ISaveObject: IJsonSerializable
{
    /// <summary>
    /// Serialize key for json
    /// </summary>
    string Key { get; }
}
