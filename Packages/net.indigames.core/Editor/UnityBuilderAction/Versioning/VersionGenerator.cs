namespace IndiGamesEditor.UnityBuilderAction.Versioning
{
    public static class VersionGenerator
    {
        /// <inheritdoc cref="IndiGamesEditor.UnityBuilderAction.Versioning.Git.GenerateSemanticCommitVersion"/>
        public static string Generate()
        {
            return Git.GenerateSemanticCommitVersion();
        }
    }
}