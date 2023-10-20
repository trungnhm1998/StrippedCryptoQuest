using IndiGames.Core.SaveSystem;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using System.Threading.Tasks;

public interface ISaveSystem
{
    string PlayerName { get; set; }

    bool SaveScene(SceneScriptableObject sceneSO);
    bool LoadScene(SceneScriptableObject sceneSO);

    bool LoadObject(IJsonSerializable jObject);
    bool SaveObject(IJsonSerializable jObject);

    bool SaveGame();
    bool LoadSaveGame();

    Task<bool> SaveGameAsync();
    Task<bool> LoadSaveGameAsync();
}
