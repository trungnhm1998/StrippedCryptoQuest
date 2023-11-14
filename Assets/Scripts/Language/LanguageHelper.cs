using System.Collections.Generic;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace CryptoQuest.Language
{
    public static class LanguageHelper
    {
        public static int GetLocaleIndex(Locale locale) => LocalizationSettings.AvailableLocales.Locales.IndexOf(locale);

        public static List<string> GetLocaleNames() =>
            LocalizationSettings.AvailableLocales.Locales.ConvertAll(locale => locale.name);
    }
}