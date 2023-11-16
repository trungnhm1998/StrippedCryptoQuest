using System.Collections;
using CryptoQuest.Item.Equipment;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace CryptoQuest.UI.Tooltips
{
    public class UIEquipmentTooltip : MonoBehaviour
    {
        [SerializeField] private Image _headerBackground;
        [SerializeField] private Image _rarity;
        [SerializeField] private GameObject _nftTag;
        [SerializeField] private Image _illustration;
        [SerializeField] private LocalizeStringEvent _nameLocalize;
        private Coroutine _loadCo;
        private AsyncOperationHandle<Sprite> _handle;

        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Init(EquipmentInfo equipment)
        {
            _illustration.enabled = false;
            _headerBackground.color = equipment.Rarity.Color;
            _rarity.sprite = equipment.Rarity.Icon;
            _nftTag.SetActive(equipment.IsNftItem);
            _nameLocalize.StringReference = equipment.DisplayName;

            SetPosition();
            LoadImage(equipment);
        }
        private void SetPosition()
        {
            var selectedGameObject = EventSystem.current.currentSelectedGameObject;
            if (selectedGameObject == null) return;
            var rectTransform = selectedGameObject.GetComponent<RectTransform>();
            if (rectTransform == null) return;
            _rectTransform.position = rectTransform.position;
            // random between 0 and 1;
            var pivotY = 0.5f;
            var pivotX = 0.5f;
            _rectTransform.pivot = new Vector2(pivotX, pivotY);
        }

        private void LoadImage(EquipmentInfo equipment)
        {
            if (_loadCo != null)
            {
                StopCoroutine(_loadCo);
                if (_handle.IsValid()) Addressables.Release(_handle);
            }

            if (equipment.Config.Image.RuntimeKeyIsValid() == false) return;
            _loadCo = StartCoroutine(CoLoadIllustration(equipment.Config.Image));
        }

        private IEnumerator CoLoadIllustration(AssetReferenceT<Sprite> spriteAsset)
        {
            _handle = spriteAsset.LoadAssetAsync();
            yield return _handle;
            _illustration.sprite = _handle.Result;
            _illustration.enabled = true;
        }

        private void OnDisable()
        {
            if (_handle.IsValid()) Addressables.Release(_handle);
            if (_loadCo != null) StopCoroutine(_loadCo);
        }
    }
}