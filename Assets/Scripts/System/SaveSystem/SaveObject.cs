using CryptoQuest.System;
using IndiGames.Core.Events.ScriptableObjects;
using System;
using System.Collections;
using UnityEngine;

public abstract class SaveObject : MonoBehaviour, ISaveObject
{
    protected ISaveSystem SaveSystem;

    protected bool _loadedFromSave = false;
    public bool IsLoaded() => _loadedFromSave;

    protected virtual void OnEnable()
    {
        SaveSystem = ServiceProvider.GetService<ISaveSystem>();
        SaveSystem?.RegisterObject(this);
    }

    protected virtual void OnDisable()
    {
    }

    public abstract string Key { get; }
    public abstract string ToJson();    
    public abstract IEnumerator CoFromJson(string json);

    private IEnumerator CoFromJsonWaiter(string json)
    {
        yield return CoFromJson(json);
        _loadedFromSave = true;
    }

    public bool FromJson(string json)
    {
        if (SaveSystem != null && SaveSystem.IsLoadingSaveGame())
        {
            _loadedFromSave = false;
            StartCoroutine(CoFromJsonWaiter(json));
        }
        else
        {
            _loadedFromSave = true;
        }
        return true;
    }

    public IEnumerator WaitUntilTrue(Func<bool> checkMethod)
    {
        while (checkMethod() == false)
        {
            yield return null;
        }
    }
}
