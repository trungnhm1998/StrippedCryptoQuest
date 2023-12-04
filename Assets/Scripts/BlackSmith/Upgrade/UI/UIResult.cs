using UnityEngine;

namespace CryptoQuest.BlackSmith.Upgrade.UI
{
    public class UIResult : MonoBehaviour
    {
        [SerializeField] private RectTransform _equipmentDetail;
        [SerializeField] private Transform _resultEquipmentContainer;

        private Transform _equipmentUIParent;
        private Vector2 _equipmentUIPosition;

        private void OnEnable()
        {
            SetActiveEquipmentUI(false);
            CacheEquipmentUIPosition();
            SetupEquipmentUI();
            SetActiveEquipmentUI(true);
        }

        private void OnDisable()
        {
            ResetEquipmentUIPosition();
        }

        private void CacheEquipmentUIPosition()
        {
            _equipmentUIParent = _equipmentDetail.transform.parent;
            _equipmentUIPosition = _equipmentDetail.anchoredPosition;
        }

        private void SetupEquipmentUI()
        {
            _equipmentDetail.transform.SetParent(_resultEquipmentContainer);
            _equipmentDetail.anchoredPosition = Vector2.zero;
        }

        private void ResetEquipmentUIPosition()
        {
            _equipmentDetail.transform.SetParent(_equipmentUIParent);
            _equipmentDetail.anchoredPosition = _equipmentUIPosition;
        }

        private void SetActiveEquipmentUI(bool value)
        {
            _equipmentDetail.gameObject.SetActive(value);
        }
    }
}