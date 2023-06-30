using IndiGames.Core.Common;
using NUnit.Framework;

namespace IndiGamesEditor.Core.Tests.Editor.Common
{
    [TestFixture]
    public class SceneAssetReferenceTests
    {
        private SceneAssetReference _sceneAssetReference;

        [SetUp]
        public void Setup()
        {
            _sceneAssetReference = new SceneAssetReference("guid");
        }

        [Test]
        public void ValidateAsset_WithSceneUnit_ValidatesCorrectly()
        {
            Assert.IsTrue(_sceneAssetReference.ValidateAsset("path/to/scene.unity"));
        }
    }
}