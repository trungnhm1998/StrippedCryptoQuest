using UnityEngine;

namespace CryptoQuest.Input
{
    public class EnableInputOnAwake : MonoBehaviour
    {
        [SerializeField] private string _actionMapName;
        [SerializeField] private InputMediatorSO _inputMediator;
        private void Awake() => _inputMediator.EnableInputMap(_actionMapName);
    }
}