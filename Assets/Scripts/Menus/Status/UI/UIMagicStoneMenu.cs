using CryptoQuest.Input;
using Input;
using UnityEngine;

namespace CryptoQuest.Menus.Status.UI
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