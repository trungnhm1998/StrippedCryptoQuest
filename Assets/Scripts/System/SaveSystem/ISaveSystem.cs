using IndiGames.Core.SaveSystem;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using System.Threading.Tasks;

public interface ISaveSystem
{
    string PlayerName { get; set; }

    bool SaveScene(SceneScriptableObject sceneSO);
    bool LoadScene(ref SceneScriptableObject sceneSO);

    bool LoadObject(ISaveObject jObject);
    bool SaveObject(ISaveObject jObject);

    bool SaveGame();
    bool LoadSaveGame();

    bool IsLoadingSaveGame();

    Task<bool> SaveGameAsync();
    Task<bool> LoadSaveGameAsync();
}
