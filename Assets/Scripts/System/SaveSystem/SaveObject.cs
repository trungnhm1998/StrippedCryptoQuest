using CryptoQuest.System;
using IndiGames.Core.Events.ScriptableObjects;
using System;
using System.Collections;
using UnityEngine;

public abstract class SaveObject : MonoBehaviour, ISaveObject
{
    [SerializeField] private VoidEventChannelSO _sceneLoadedEvent;

    protected ISaveSystem SaveSystem;

    protected bool _loadedFromSave = false;
    public bool IsLoaded() => _loadedFromSave;


    protected virtual void OnEnable()
    {
        _sceneLoadedEvent.EventRaised += OnSceneLoaded;
    }

    protected virtual void OnDisable()
    {
        _sceneLoadedEvent.EventRaised -= OnSceneLoaded;
    }

    protected virtual void OnSceneLoaded()
    {
        SaveSystem = ServiceProvider.GetService<ISaveSystem>();
        if (SaveSystem != null && SaveSystem.IsLoadingSaveGame())
        {
            _loadedFromSave = false;
            SaveSystem.LoadObject(this);
        }
        else
        {
            _loadedFromSave = true;
        }
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
        StartCoroutine(CoFromJsonWaiter(json));
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
