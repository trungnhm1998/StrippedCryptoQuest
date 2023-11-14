using CryptoQuest.Events.UI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CryptoQuest.Wallet
{
    [CustomEditor(typeof(ShowWalletEventChannelSO))]
    public class ShowWalletEventChannelSOEditor : Editor
    {
        [SerializeField] private VisualTreeAsset _uxml;
        private ShowWalletEventChannelSO Target => target as ShowWalletEventChannelSO;

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            _uxml.CloneTree(root);

            var showGoldButton = root.Q<Button>("show-gold-button");
            var showDiamondButton = root.Q<Button>("show-diamond-button");
            var showSoulButton = root.Q<Button>("show-soul-button");
            var hideUIButton = root.Q<Button>("hide-ui-button");

            showGoldButton.clicked += OnShowGold;
            showDiamondButton.clicked += OnShowDiamond;
            showSoulButton.clicked += OnShowSoul;
            hideUIButton.clicked += OnHideUI;

            return root;
        }

        private void OnShowGold() => Target.Show();
        private void OnShowDiamond() => Target.Show(false, true);
        private void OnShowSoul() => Target.Show(false, false, true);
        private void OnHideUI() => Target.Hide();
    }
}