using System;
using System.IO;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using ScriptableObjectBrowser;
using UnityEngine;

namespace CryptoQuestEditor.Gameplay.Inventory
{
    public class EquipmentTypeSOEditor : ScriptableObjectBrowserEditor<EquipmentTypeSO>
    {
        public EquipmentTypeSOEditor()
        {
            this.createDataFolder = false;

            this.defaultStoragePath = "Assets/ScriptableObjects/Data/Inventory/ItemTypes/Equipments";
        }

        public override void ImportBatchData(string directory, Action<ScriptableObject> callback)
        {
            string[] allLines = File.ReadAllLines(directory);
        }
    }
}