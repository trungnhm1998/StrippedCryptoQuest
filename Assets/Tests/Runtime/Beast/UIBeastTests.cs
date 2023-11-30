using CryptoQuest.Character.Beast;
using CryptoQuest.Menus.Beast.UI;
using NSubstitute;
using NUnit.Framework;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace CryptoQuest.Tests.Runtime.Beast
{
    [TestFixture]
    public class UIBeastTests
    {
        [TestCase("キツネ")]
        [TestCase("Dragon")]
        public void Init_WithBeast_NameCorrect(string name)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/UI/Menu/Beast/UIBeast.prefab");

            Assert.IsNotNull(prefab);

            var uiBeast = Object.Instantiate(prefab).GetComponent<UIBeast>();
            var beast = Substitute.For<IBeast>();
            beast.Name.Returns(name);
            uiBeast.Init(beast);

            var txtName = uiBeast.transform.Find("Name").GetComponent<TMP_Text>();

            Assert.AreEqual(name, txtName.text);
        }
    }
}