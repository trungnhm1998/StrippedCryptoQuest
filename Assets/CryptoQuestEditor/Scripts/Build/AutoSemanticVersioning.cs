using IndiGamesEditor.UnityBuilderAction.Versioning;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace CryptoQuestEditor.Build
{
    public class AutoSemanticVersioning : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            VersionApplicator.SetVersion(VersionGenerator.Generate());
        }
    }
}