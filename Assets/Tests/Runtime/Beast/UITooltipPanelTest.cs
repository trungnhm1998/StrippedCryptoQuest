using System.Collections;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Beast;
using CryptoQuest.Character;
using CryptoQuest.Gameplay;
using CryptoQuest.Ranch.Tooltip;
using CryptoQuest.Ranch.UI;
using CryptoQuest.UI.Tooltips.Events;
using NSubstitute;
using NUnit.Framework;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace CryptoQuest.Tests.Runtime.Beast
{
    [TestFixture, Category("Integration")]
    public class UITooltipPanelTest
    {
        [Tooltip("Constants")]
        public const string EVENT_PATH = "Assets/ScriptableObjects/Beast/Tooltip/ShowBeastTooltipEventChannel.asset";
        public const string TOOLTIP_PATH = "Assets/Prefabs/UI/Tooltips/BeastTooltip.prefab";
        public const string BEAST_ITEM_PATH = "Assets/Prefabs/UI/Farm/BeastItem.prefab";
        public const float WAIT_TIME = 0.5f;

        [Tooltip("Field")]
        private ShowTooltipEvent _showTooltipChannelSO;
        private TooltipController _tooltipController;
        private GameObject _prefab;
        private IBeast _beast;
        private UIBeastItem _beastItem;
        private Button _button;
        private Image _element;
        private Image _illustration;
        private TextMeshProUGUI _level;
        private LocalizeStringEvent _passiveLocalized;

        [UnitySetUp]
        public IEnumerator OneTimeSetup()
        {
            yield return LoadTestScene();
            SetupUIElements();
            SetupNewBeast();
        }

        private IEnumerator LoadTestScene()
        {
            var scene = "Assets/Scenes/WIP/Playground.unity";
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode(scene,
                new LoadSceneParameters(LoadSceneMode.Single));
            yield return new WaitForSeconds(1f);
        }

        private void SetupUIElements()
        {
            var canvasObject = Object.FindObjectOfType<Canvas>();
            _showTooltipChannelSO = AssetDatabase.LoadAssetAtPath<ShowTooltipEvent>(EVENT_PATH);
            _prefab = AssetDatabase.LoadAssetAtPath<GameObject>(TOOLTIP_PATH);

            var beastItem = AssetDatabase.LoadAssetAtPath<GameObject>(BEAST_ITEM_PATH);
            _tooltipController = Object.Instantiate(_prefab).GetComponent<TooltipController>();
            _beastItem = Object.Instantiate(beastItem).GetComponent<UIBeastItem>();
            _beastItem.transform.SetParent(canvasObject.transform);
            _tooltipController.transform.SetParent(canvasObject.transform);

            var rectTransform = _beastItem.GetComponent<RectTransform>();
            rectTransform.localPosition = Vector3.zero;

            EventSystem.current.SetSelectedGameObject(_beastItem.gameObject);
            _button = _beastItem.GetComponent<Button>();
            _button.Select();
        }

        private void SetupNewBeast()
        {
            var originalClass = ScriptableObject.CreateInstance<CharacterClass>();
            var originalElement = ScriptableObject.CreateInstance<Elemental>();
            var originalType = ScriptableObject.CreateInstance<BeastTypeSO>();
            var originalPassive =
                AssetDatabase.LoadAssetAtPath<PassiveAbility>(
                    "Assets/ScriptableObjects/Character/Skills/Passive/4001.asset");
            var originalStats = new StatsDef();
            var localizeName = new LocalizedString();

            _beast = Substitute.For<IBeast>();
            _beast.Class.Returns(originalClass);
            _beast.Elemental.Returns(originalElement);
            _beast.Type.Returns(originalType);
            _beast.LocalizedName.Returns(localizeName);
            _beast.Level.Returns(1);
            _beast.Stars.Returns(1);
            _beast.Passive.Returns(originalPassive);
            _beast.Stats.Returns(originalStats);
            _beast.IsValid().Returns(true);

            // _beastItem.Initialize(_beast);
        }

        [UnityTest]
        public IEnumerator Show_WithBeastIsValid_ShouldShowTooltip()
        {
            var tooltip = _tooltipController.transform.Find("Tooltip").GetComponent<UIBeastTooltip>();
            _beastItem.Beast.IsValid().Returns(true);
            yield return new WaitForSeconds(WAIT_TIME);
            _showTooltipChannelSO.RaiseEvent(true);
            Assert.IsTrue(tooltip.gameObject.activeSelf);
        }

        [UnityTest]
        public IEnumerator Show_WithBeastNotValid_ShouldNotShowTooltip()
        {
            var tooltip = _tooltipController.transform.Find("Tooltip").GetComponent<UIBeastTooltip>();
            _beastItem.Beast.IsValid().Returns(false);
            yield return new WaitForSeconds(WAIT_TIME);
            _showTooltipChannelSO.RaiseEvent(true);
            Assert.IsFalse(tooltip.gameObject.activeSelf);
        }

        [UnityTest]
        public IEnumerator Show_WithFullData_ShouldDisplayCorrect()
        {
            var originalClass =
                AssetDatabase.LoadAssetAtPath<CharacterClass>(
                    "Assets/ScriptableObjects/Character/Classes/Basic/Warrior.asset");
            var originalElement =
                AssetDatabase.LoadAssetAtPath<Elemental>(
                    "Assets/ScriptableObjects/Character/Attributes/Elemental/Fire/Fire.asset");
            var originalType =
                AssetDatabase.LoadAssetAtPath<BeastTypeSO>("Assets/ScriptableObjects/Beast/Origins/Tiger.asset");
            int level = 35;
            int stars = 4;

            _level = _tooltipController.transform.Find("Tooltip/BasicStats/TopPanel/Level")
                .GetComponent<TextMeshProUGUI>();
            _element = _tooltipController.transform.Find("Tooltip/BasicStats/TopPanel/Element").GetComponent<Image>();
            _illustration = _tooltipController.transform.Find("Tooltip/Beast/Image").GetComponent<Image>();
            _passiveLocalized = _tooltipController.transform.Find("Tooltip/PassiveSkill/Passive/Name")
                .GetComponent<LocalizeStringEvent>();

            _beastItem.Beast.Class.Returns(originalClass);
            _beastItem.Beast.Elemental.Returns(originalElement);
            _beastItem.Beast.Type.Returns(originalType);
            _beastItem.Beast.Level.Returns(level);
            _beastItem.Beast.Stars.Returns(stars);
            yield return new WaitForSeconds(WAIT_TIME);
            _showTooltipChannelSO.RaiseEvent(true);
            yield return new WaitForSeconds(WAIT_TIME);
            Assert.AreEqual(_level.text, $"Lv{_beastItem.Beast.Level}");
            Assert.AreEqual(_element.sprite, _beastItem.Beast.Elemental.Icon);
            Assert.AreEqual(_passiveLocalized.StringReference, _beastItem.Beast.Passive.Description);
            Assert.IsNotNull(_illustration.sprite);
        }

        [UnityTest]
        public IEnumerator Hide_TooltipShouldNotDisplay()
        {
            var tooltip = _tooltipController.transform.Find("Tooltip").GetComponent<UIBeastTooltip>();
            yield return new WaitForSeconds(WAIT_TIME);
            _showTooltipChannelSO.RaiseEvent(false);
            Assert.IsFalse(tooltip.gameObject.activeSelf);
        }
    }
}