using CryptoQuest.System;
using CryptoQuest.System.SaveSystem;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Title
{
    public class UINameConfirmPanel : MonoBehaviour
    {
        [SerializeField] private SaveSystemSO _tempSaveInfo;
        [field: SerializeField] public Button YesButton { get; private set; }
        [field: SerializeField] public Button NoButton { get; private set; }

        private void OnEnable() => Invoke(nameof(SelectYesButton), 0);

        private void SelectYesButton() => YesButton.Select();

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