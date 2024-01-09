using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.ShopSystem
{
    public class ConsumableDetailsPresenter : MonoBehaviour
    {
        [SerializeField] private UIConsumableDetails _uiConsumableDetails;
        [SerializeField] private BoolEventChannelSO _showConsumableDetailsEvent;

        private void OnEnable()
        {
            _showConsumableDetailsEvent.EventRaised += SetActiveConsumableDetails;
        }

        private void OnDisable()
        {
            _showConsumableDetailsEvent.EventRaised -= SetActiveConsumableDetails;
        }
        
        private void SetActiveConsumableDetails(bool show)
        {
            _uiConsumableDetails.gameObject.SetActive(show);
            if (show)
                SetDetailUI();
        }

        private void SetDetailUI()
        {
            var selectedObject = EventSystem.current.currentSelectedGameObject;
            if (selectedObject == null) return;
            var provider = selectedObject.GetComponent<IConsumableInfoProvider>();
            _uiConsumableDetails.SetupUI(provider.ConsumableInfo);
        }
    }
}