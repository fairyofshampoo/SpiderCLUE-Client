using System.Globalization;
using System.Resources;
using System.Threading;

namespace Spider_Clue.Logic
{
    public class LanguageManager
    {
        private static ResourceManager resourceManager = null;

        public static void SetLanguage(string language)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            resourceManager = new ResourceManager("Spider_Clue.Properties.Resources", typeof(LanguageManager).Assembly);
        }
    }
}
