using IndiGames.Core.SaveSystem;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Title
{
    public class UINameConfirmPanel : MonoBehaviour
    {
        [SerializeField] private SaveSystemSO _saveSystemSo;
        [SerializeField] private SaveSystemSO _tempSaveInfo;
        [field: SerializeField] public Button YesButton { get; private set; }
        [field: SerializeField] public Button NoButton { get; private set; }

        private void OnEnable() => Invoke(nameof(SelectYesButton), 0);
        private void SelectYesButton() => YesButton.Select();
        public void ConfirmPlayerName() => _saveSystemSo.PlayerName = _tempSaveInfo.PlayerName;
    }
}