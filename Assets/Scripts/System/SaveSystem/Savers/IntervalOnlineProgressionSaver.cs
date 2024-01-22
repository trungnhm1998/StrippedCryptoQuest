using UnityEngine;
using IndiGames.Core.Events.ScriptableObjects;

namespace CryptoQuest.System.SaveSystem.Savers
{
    public class IntervalOnlineProgressionSaver : MonoBehaviour
    {
        [SerializeField] private float _saveInterval = 5.0f;
        [SerializeField] private VoidEventChannelSO _forceSaveEvent;

        private void Start()
        {
            InvokeRepeating(nameof(ForceSaveGame), _saveInterval, _saveInterval);
        }

        private void OnDisable()
        {
            CancelInvoke();
        }

        private void ForceSaveGame()
        {
            _forceSaveEvent.RaiseEvent();
        }
    }
}