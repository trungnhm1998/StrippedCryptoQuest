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
            InvokeRepeating(nameof(ForceSaveGame), 0, _saveInterval);
        }

        private void OnDisable()
        {
            CancelInvoke(nameof(ForceSaveGame));
        }

        private void ForceSaveGame()
        {
            _forceSaveEvent.RaiseEvent();
        }
    }
}