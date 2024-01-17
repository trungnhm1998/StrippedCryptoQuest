using System.Collections.Generic;
using CryptoQuest.Beast;
using CryptoQuest.Beast.ScriptableObjects;
using CryptoQuest.Menus.Beast.UI;
using NSubstitute;
using NUnit.Framework;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Tests.Editor.Beast
{
    [TestFixture]
    public class UIBeastTests
    {
        private const string UI_BEAST_PATH = "Assets/Prefabs/UI/Menu/Beast/UIBeast.prefab";
        private const string UI_BEAST_LIST_PATH = "Assets/Prefabs/UI/Menu/Beast/BeastPanel.prefab";
        private const string BEAST_PROVIDER_PATH = "Assets/ScriptableObjects/Beast/BeastProvider.asset";

        private GameObject _prefab;
        private GameObject _beastListPrefab;

        private UIBeast _uiBeast;
        private UIBeastList _uiBeastList;
        private UIBeastMenu _uiBeastMenu;
        private IBeast _beast;
        private IBeast _currentBeastProvider;

        private BeastProvider _beastProvider;


        [SetUp]
        public void Setup()
        {
            _prefab = AssetDatabase.LoadAssetAtPath<GameObject>(UI_BEAST_PATH);
            _beastProvider = AssetDatabase.LoadAssetAtPath<BeastProvider>(BEAST_PROVIDER_PATH);
            _beastListPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(UI_BEAST_LIST_PATH);

            Assert.IsNotNull(_prefab, $"Prefab not found at {UI_BEAST_PATH}");
            Assert.IsNotNull(_beastProvider, $"BeastProvider not found at {BEAST_PROVIDER_PATH}");

            Assert.IsNotNull(_beastListPrefab, $"Prefab not found at {UI_BEAST_LIST_PATH}");

            _uiBeastMenu = Object.Instantiate(_beastListPrefab).GetComponent<UIBeastMenu>();
            _uiBeastList = _uiBeastMenu.ListBeastUI;

            _uiBeast = Object.Instantiate(_prefab).GetComponent<UIBeast>();
            _beast = Substitute.For<IBeast>();

            _currentBeastProvider = _beastProvider.EquippingBeast;
        }

        [TearDown]
        public void TearDown()
        {
            _beastProvider.EquippingBeast = _currentBeastProvider;
        }

        [TestCase("Dragon")]
        [TestCase("Tiger")]
        [TestCase("Horse")]
        [TestCase("Fox")]
        [TestCase("Bird")]
        public void Init_WithBeast_NameCorrect(string name)
        {
            _beast.LocalizedName.Returns(
                new LocalizedString() { TableReference = "Beasts", TableEntryReference = name });
            _uiBeast.Init(_beast);

            TMP_Text txtName = _uiBeast.transform.Find("Name").GetComponent<TMP_Text>();

            Assert.AreEqual(name, txtName.text);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void IsEquipped_WithBeast_EquipTagActiveIsCorrect(bool isEquipped)
        {
            _uiBeast.MarkedForEquipped = isEquipped;

            GameObject equipTag = _uiBeast.transform.Find("EquippedTag").gameObject;

            Assert.AreEqual(isEquipped, equipTag.activeSelf);
        }

        [Test]
        public void OnPressButton_OnePressWithBeast_BeastProviderMustHaveCorrectBeast()
        {
            _beastProvider.EquippingBeast = NullBeast.Instance;

            BeastProvider(_uiBeast, _beast);

            _uiBeastList.EquipBeast(_uiBeast);

            Assert.AreEqual(_beast, _beastProvider.EquippingBeast, $"BeastProvider.EquippingBeast is not {_beast}.");
        }

        [Test]
        public void OnPressButton_PressTwiceWithBeast_BeastProviderMustReturnNull()
        {
            _beastProvider.EquippingBeast = NullBeast.Instance;

            BeastProvider(_uiBeast, _beast);

            _uiBeastList.EquipBeast(_uiBeast);
            _uiBeastList.EquipBeast(_uiBeast);

            Assert.AreEqual(_beastProvider.EquippingBeast, NullBeast.Instance, $"BeastProvider.EquippingBeast is null");
        }

        private void BeastProvider(UIBeast uiBeast, IBeast beast)
        {
            _uiBeastMenu.gameObject.SetActive(true);
            _uiBeastList.Init();

            uiBeast.Init(beast);
        }
    }
}