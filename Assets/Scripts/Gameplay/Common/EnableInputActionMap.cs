using CryptoQuest.Input;
using UnityEngine;

namespace CryptoQuest.Gameplay.Common
{
    public class EnableInputActionMap : MonoBehaviour
    {
        [SerializeField] private InputMediatorSO _inputMediator;
        private void Start()
        {
            _inputMediator.EnableMapGameplayInput();
        }
    }
}