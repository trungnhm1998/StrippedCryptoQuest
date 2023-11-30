using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Menus.Beast.UI;
using CryptoQuest.Tests.Runtime.Beast.Builder;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Tests.Runtime.Beast
{
    [TestFixture]
    public class UIBeastDetailsTests
    {
        private UIBeastDetail _uiBeastDetails;

        [SetUp]
        public void Setup()
        {
            var panelPrefab =
                AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/UI/Menu/Beast/BeastPanel.prefab");
            var panel = Object.Instantiate(panelPrefab);
            _uiBeastDetails = panel.GetComponentInChildren<UIBeastDetail>();
        }

        [Test]
        public void FillUI_WithDarkBeast_CorrectDarkElementSprite()
        {
            var darkElement =
                AssetDatabase.LoadAssetAtPath<Elemental>(
                    "Assets/ScriptableObjects/Character/Attributes/Elemental/Dark/Dark.asset");

            _uiBeastDetails.FillUI(A.Beast.WithElement(darkElement).Build());

            var panelSO = new SerializedObject(_uiBeastDetails);
            var beastElementImage = panelSO.FindProperty("_beastElement").objectReferenceValue as Image;

            Assert.AreEqual(darkElement.Icon, beastElementImage.sprite);
        }

        [Test]
        public void FillUI_WithNullPassive_ShouldShowsEmptyPassive()
        {
            _uiBeastDetails.FillUI(A.Beast.Build());

            var panelSO = new SerializedObject(_uiBeastDetails);
            var beastPassiveSkill =
                panelSO.FindProperty("_beastPassiveSkill").objectReferenceValue as LocalizeStringEvent;

            Assert.AreEqual(new LocalizedString(), beastPassiveSkill.StringReference);
        }

        [Test]
        public void FillUI_WithPassive_ShouldShowsPassive()
        {
            var passive =
                AssetDatabase.LoadAssetAtPath<PassiveAbility>(
                    "Assets/ScriptableObjects/Character/Skills/Conditionals/3001.asset");
            _uiBeastDetails.FillUI(A.Beast.WithPassive(passive).Build());

            var panelSO = new SerializedObject(_uiBeastDetails);
            var beastPassiveSkill =
                panelSO.FindProperty("_beastPassiveSkill").objectReferenceValue as LocalizeStringEvent;

            Assert.AreEqual(passive.Description, beastPassiveSkill.StringReference);
        }

        [Test]
        public void FillUI_WithAnotherBeast_ShouldUpdateBeastUI()
        {
            var passive =
                AssetDatabase.LoadAssetAtPath<PassiveAbility>(
                    "Assets/ScriptableObjects/Character/Skills/Conditionals/3001.asset");
            _uiBeastDetails.FillUI(A.Beast.WithPassive(passive).Build());

            var passiveName = _uiBeastDetails.transform.Find("Detail/Panel/Passives/Panel/Text (Legacy)")
                .GetComponent<LocalizeStringEvent>();
            Assert.AreEqual(passive.Description, passiveName.StringReference);

            passive = AssetDatabase.LoadAssetAtPath<PassiveAbility>(
                "Assets/ScriptableObjects/Character/Skills/Conditionals/3005.asset");
            _uiBeastDetails.FillUI(A.Beast.WithPassive(passive).Build());

            Assert.AreEqual(passive.Description, passiveName.StringReference);
        }
    }
}