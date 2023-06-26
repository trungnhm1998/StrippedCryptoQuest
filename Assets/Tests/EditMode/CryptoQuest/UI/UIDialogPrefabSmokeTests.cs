using CryptoQuest.UI;
using NUnit.Framework;

namespace Tests.EditMode.CryptoQuest.UI
{
    public class UIDialogPrefabSmokeTests
    {
        private const string FILTER = "UIDialog t:prefab";
        private UIDialog _dialogComponent;
        private IDialog _dialog;
        private int _instanceCount;

        [SetUp]
        public void Setup()
        {
            var uiDialogPrefabGUIDs = UnityEditor.AssetDatabase.FindAssets(FILTER);

            _instanceCount = uiDialogPrefabGUIDs.Length;

            var guid = uiDialogPrefabGUIDs[0];

            var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);

            _dialogComponent = UnityEditor.AssetDatabase.LoadAssetAtPath<UIDialog>(path);

            _dialog = _dialogComponent;
        }

        [Test]
        public void UIDialogPrefab_ShouldHave1Instance()
        {
            Assert.AreEqual(1, _instanceCount);
        }

        [Test]
        public void UIDialogPrefab_UIDialogComponent_ShouldNotNull()
        {
            Assert.NotNull(_dialog);
        }

        [Test]
        public void UIDialogPrefab_UIDialogComponent_ShouldNotDisable()
        {
            Assert.IsTrue(_dialogComponent.enabled);
        }

        [Test]
        public void UIDialogPrefab_UIDialogComponent_ReferencesShouldNotNull()
        {
            Assert.NotNull(_dialog.ShowDialogEvent);
            Assert.NotNull(_dialog.Content);
        }

        [Test]
        public void UIDialogPrefab_Content_ShouldDisabledByDefault()
        {
            Assert.IsFalse(_dialog.Content.activeSelf);
        }
    }
}