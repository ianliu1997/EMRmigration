using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Resources;
using System.Globalization;

namespace PalashDynamic.Localization
{
    public class LocalizationManager
    {
        public static ResourceManager resourceManager { get; set; }

        public CultureInfo culture;
        public CultureInfo UiCulture
        {
            get { return culture; }
            set { culture = value; }
        }

        public LocalizationManager()
        {
            resourceManager = new ResourceManager("PalashDynamics.Localization.Localized", typeof(PalashDynamic.Localization.LocalizationManager).Assembly);
        }

        public LocalizationManager(CultureInfo culture)
        {
            this.culture = culture;
            resourceManager = new ResourceManager("PalashDynamics.Localization.Localized", typeof(PalashDynamic.Localization.LocalizationManager).Assembly);
        }

        public string GetValue(string resource)
        {
            return resourceManager.GetString(resource, UiCulture);
        }
    }
}
