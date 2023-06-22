using CryptoQuest.Input;
using UnityEngine;

namespace CryptoQuest.Characters
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private InputMediatorSO _inputMediator;

        private void Start()
        {
            _inputMediator.EnableMapGameplayInput();
        }

        private void OnEnable()
        {
            _inputMediator.MoveEvent += MoveEvent_Raised;
        }

        private void OnDisable()
        {
            _inputMediator.MoveEvent -= MoveEvent_Raised;
        }

        private void MoveEvent_Raised(Vector2 axis)
        {
            Debug.Log(axis);
        }
    }
}