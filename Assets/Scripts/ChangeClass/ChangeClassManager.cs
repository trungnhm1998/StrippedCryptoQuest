using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CryptoQuest.ChangeClass.ScriptableObjects;

namespace CryptoQuest.ChangeClass
{
    public class ChangeClassManager : MonoBehaviour
    {
        [SerializeField] private ShowChangeClassEventChannelSO _showChangeClassEvent;
        [SerializeField] private ChangeClassStateController _stateController;
        [SerializeField] private ChangeClassInputManager _input;
        [SerializeField] private GameObject _changeClassContent;


        private void OnEnable()
        {
            _showChangeClassEvent.EventRaised += ShowChangeClass;
            _stateController.ExitStateEvent += HideChangeClass;
        }

        private void OnDisable()
        {
            _showChangeClassEvent.EventRaised -= ShowChangeClass;
            _stateController.ExitStateEvent -= HideChangeClass;
        }

        private void ShowChangeClass()
        {
            _changeClassContent.SetActive(true);
            _input.EnableInput();
        }

        private void HideChangeClass()
        {
            _changeClassContent.SetActive(false);
            _input.DisableInput();
        }
    }
}
