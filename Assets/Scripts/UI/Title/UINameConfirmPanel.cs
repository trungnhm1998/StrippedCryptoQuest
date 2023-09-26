using System.Collections;
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

        private void OnEnable()
        {
            StartCoroutine(CoSelectYesButton());
        }

        private IEnumerator CoSelectYesButton()
        {
            yield return new WaitForSeconds(.03f);
            YesButton.Select();
        }

        public void ConfirmPlayerName()
        {
            _saveSystemSo.PlayerName = _tempSaveInfo.PlayerName;
        }
    }
}