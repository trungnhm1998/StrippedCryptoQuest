using System.Collections.Generic;
using System.Linq;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace CryptoQuest.Language
{
    public static class LanguageHelper
    {
        public static int GetLocaleIndex(Locale locale) =>
            LocalizationSettings.AvailableLocales.Locales.IndexOf(locale);

        public static List<string> GetLocaleNames()
        {
            return LocalizationSettings.AvailableLocales.Locales.Select(locale => locale.Identifier.CultureInfo != null
                    ? locale.Identifier.CultureInfo.NativeName
                    : locale.ToString())
                .ToList();
        }
    }
}