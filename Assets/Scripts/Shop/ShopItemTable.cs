using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Shop
{
    /// <summary>
    /// https://docs.google.com/spreadsheets/d/1WkX1DyDOGf6EiAppo8Buz2sUkSKV5OnDENEvmHzKXNQ/edit#gid=853450474
    /// </summary>
    [CreateAssetMenu(fileName ="New shop table", menuName = "Crypto Quest/Shop/New Shop Table")]
    public class ShopItemTable : ScriptableObject
    {
        public int Id;
        public List<string> Items = new ();
    }
}
