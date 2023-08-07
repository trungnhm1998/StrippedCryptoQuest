using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using ScriptableObjectBrowser;

namespace CryptoQuestEditor.Gameplay.Inventory
{
    public class WeaponSOEditor : ScriptableObjectBrowserEditor<WeaponSO>
    {
        public WeaponSOEditor()
        {
            this.createDataFolder = false;

            this.defaultStoragePath = "Assets/ScriptableObjects/Data/Inventory/Items/Equipments/Weapons";
        }
    }
}