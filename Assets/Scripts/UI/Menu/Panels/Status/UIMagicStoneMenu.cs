using CryptoQuest.Input;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Status
{
    public class UIMagicStoneMenu : MonoBehaviour
    {
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private GameObject _contents;

        public void ShowPanel()
        {
            _contents.SetActive(true);
        }
        
        private void HidePanel()
        {
            _contents.SetActive(false);
        }
    }
}