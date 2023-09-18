using CryptoQuest.Battle;
using CryptoQuest.Battle.Components;
using CryptoQuest.Tests.Runtime.Battle.Builder;
using NUnit.Framework;

namespace CryptoQuest.Tests.Runtime.Battle
{
    [TestFixture]
    public class CharacterBuilderTests
    {
        [Test]
        public void Build_DefaultElement_ShouldBeFire()
        {
            var characterBuilder = A.Character;
            var hero = characterBuilder.Build();
            Assert.AreEqual(An.Element.Fire, characterBuilder.CharacterGameObject.GetComponent<Element>().ElementValue);
        }
    }
}