using CryptoQuest.Input;
using UnityEngine;

namespace CryptoQuest.Menu
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private GameObject _menuPrefab;
        private GameObject _menuInstance;

        private void OpenMenu()
        {
            if (_menuInstance == null)
                _menuInstance = Instantiate(_menuPrefab);

            _menuInstance.SetActive(true);
            _inputMediator.EnableMenuInput();
        }

        private void UnpauseMenu()
        {
            _menuInstance.SetActive(false);
            _inputMediator.EnableMapGameplayInput();
        }
    }
}
