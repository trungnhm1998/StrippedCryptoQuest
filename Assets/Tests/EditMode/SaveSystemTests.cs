using CryptoQuest.SaveSystem;
using NUnit.Framework;
using UnityEditor;

namespace Tests.EditMode
{
    public class SaveSystemTests
    {
        [Test]
        public void SaveData_ReturnTrue()
        {
            var saveSystem = AssetDatabase.LoadAssetAtPath<SaveSystemSO>("Assets/ScriptableObjects/SaveSystem/SaveSystem.asset");
            
            Assert.NotNull(saveSystem);
            
            Assert.True(saveSystem.SaveData());
            
        }
    }
}