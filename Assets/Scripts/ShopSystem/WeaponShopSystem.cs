using UnityEngine;

namespace CryptoQuest.ShopSystem
{
    /// <summary>
    /// This shop only sells weapons
    /// </summary>
    public class WeaponShopSystem : ShopSystemBase
    {
        [SerializeField, Header("Configs")] private string[] _sellingItems;
    }
}