using CryptoQuest.System;
using CryptoQuest.System.SaveSystem;
using UnityEngine;

namespace CryptoQuest.UI.Title
{
    public class UINameConfirmPanel : MonoBehaviour
    {
        [SerializeField] private SaveSystemSO _tempSaveInfo;

        public void ConfirmPlayerName()
        {
            var saveSystem = ServiceProvider.GetService<ISaveSystem>();
            if (saveSystem != null)
            {
                saveSystem.PlayerName = _tempSaveInfo.PlayerName;
                saveSystem.SaveGame();
            }
        }
    }
}