using UnityEngine;

namespace CryptoQuest.SaveSystem
{
    public abstract class SaveManagerSO : ScriptableObject
    {
        [SerializeField] private string fileSaveName = "save.json";

        private ISaveManager _saveManager;
    }
}