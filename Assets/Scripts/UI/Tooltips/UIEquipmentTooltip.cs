using System.Collections;
using CryptoQuest.Item.Equipment;
using UnityEngine;
using UnityEngine.AddressableAssets;
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

        public void Init(EquipmentInfo equipment)
        {
            _illustration.enabled = false;
            _headerBackground.color = equipment.Rarity.Color;
            _rarity.sprite = equipment.Rarity.Icon;
            _nftTag.SetActive(equipment.IsNftItem);
            _nameLocalize.StringReference = equipment.DisplayName;

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