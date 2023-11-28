using UnityEngine;
using UnityEngine.Pool;

namespace CryptoQuest.Menus.DimensionalBox.UI.MagicStoneTransfer
{
    public class MagicStoneUIPool : MonoBehaviour
    {
        [SerializeField] private int _poolSize = 10;
        [SerializeField] private UIMagicStone _magicStonePrefab;

        private IObjectPool<UIMagicStone> _pool;

        private void Awake()
        {
            _pool = new ObjectPool<UIMagicStone>(OnCreate, OnGet, OnRelease, OnDestroyEquipmentUI);
        }

        private UIMagicStone OnCreate()
        {
            var uiMagicStone = Instantiate(_magicStonePrefab, transform);
            uiMagicStone.gameObject.SetActive(false);
            return uiMagicStone;
        }

        private void OnGet(UIMagicStone ui) => ui.gameObject.SetActive(true);

        private void OnRelease(UIMagicStone ui)
        {
            ui.gameObject.SetActive(false);
            ui.EnablePendingTag(false);
            ui.transform.SetParent(transform);
        }

        private void OnDestroyEquipmentUI(UIMagicStone ui) => Destroy(ui.gameObject);

        public UIMagicStone Get()
        {
            var uiMagicStone = _pool.Get();
            return uiMagicStone;
        }

        public void Release(UIMagicStone equipment)
        {
            _pool.Release(equipment);
        }
    }
}