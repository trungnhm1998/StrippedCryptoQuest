using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace CryptoQuest.UI.Extensions
{
    public static class ImageExtensions
    {
        public static AsyncOperationHandle<Sprite> LoadSpriteAndSet(this Image image,
            AssetReferenceT<Sprite> spriteAsset)
        {
            image.enabled = false;
            if (spriteAsset.RuntimeKeyIsValid() == false) return default;
            if (spriteAsset.IsValid() && spriteAsset.Asset != null)
            {
                image.sprite = (Sprite) spriteAsset.Asset;
                image.enabled = true;
                return default;
            }
            var handle = spriteAsset.LoadAssetAsync();
            handle.Completed += handle1 =>
            {
                image.sprite = handle1.Result;
                image.enabled = true;
            };
            return handle;
        }
    }
}