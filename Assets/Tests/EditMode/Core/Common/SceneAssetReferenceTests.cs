using Core.Runtime.Common;
using NUnit.Framework;

namespace Tests.EditMode.Core.Common
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