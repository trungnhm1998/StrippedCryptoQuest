using IndiGames.Core.SaveSystem;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using System;
using System.Collections;
using System.Threading.Tasks;

public interface ISaveSystem
{
    string PlayerName { get; set; }

    bool SaveObject(ISaveObject jObject);
    IEnumerator CoLoadObject(ISaveObject jObject, Action<bool> callback = null);

    bool SaveGame();
    bool LoadGame();
}
