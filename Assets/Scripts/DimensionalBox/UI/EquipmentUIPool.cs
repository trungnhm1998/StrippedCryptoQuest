using UnityEngine;
using UnityEngine.Pool;

namespace CryptoQuest.DimensionalBox.UI
{
    /// <summary>
    /// In-game and inbox will use this pool
    /// </summary>
    public class EquipmentUIPool : MonoBehaviour
    {
        [SerializeField] private int _poolSize = 10;
        [SerializeField] private UIEquipment _equipmentPrefab;

        private IObjectPool<UIEquipment> _pool;

        private void Awake()
        {
            _pool = new ObjectPool<UIEquipment>(OnCreate, OnGet, OnRelease, OnDestroyEquipmentUI);
        }

        private UIEquipment OnCreate()
        {
            var uiEquipment = Instantiate(_equipmentPrefab, transform);
            uiEquipment.gameObject.SetActive(false);
            return uiEquipment;
        }

        private void OnGet(UIEquipment equipment) => equipment.gameObject.SetActive(true);

        private void OnRelease(UIEquipment equipment) => equipment.gameObject.SetActive(false);

        private void OnDestroyEquipmentUI(UIEquipment equipment) => Destroy(equipment.gameObject);

        public UIEquipment Get()
        {
            var uiEquipment = _pool.Get();
            return uiEquipment;
        }

        public void Release(UIEquipment equipment)
        {
            _pool.Release(equipment);
        }
    }
}