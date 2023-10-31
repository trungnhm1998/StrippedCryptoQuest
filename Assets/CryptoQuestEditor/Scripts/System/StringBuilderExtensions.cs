using System.Text;

namespace CryptoQuestEditor.System
{
    public static class StringBuilderExtensions
    {
        public static string ReplaceString(this StringBuilder builder, string keyword = "\n")
        {
            return builder.ToString().Replace(keyword, string.Empty);
        }
    }
}