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
using System.Globalization;
using System.Windows.Resources;
using System.Xml;
using System.IO;
using System.Threading;

namespace PalashDynamics
{
    public class LocalizationManager
    {


        private static bool _downloaded = false;

        public event EventHandler<CultureChangedEventArgs> CultureChanged;
        public event EventHandler<CultureChangedErrorEventArgs> CultureChangeFailed;

        public LocalizationManager()
        {
            WebClient wc = new WebClient();
            wc.OpenReadCompleted += new OpenReadCompletedEventHandler(wc_OnOpenReadCompleted);
            wc.OpenReadAsync(new Uri("PalashDynamic.Localization.xap", UriKind.Relative), (CultureInfo)Thread.CurrentThread.CurrentCulture);

        }

        void wc_OnOpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            CultureInfo culture = (CultureInfo)e.UserState;

            if (e.Error == null)
            {
                try
                {
                    // Get the application manifest from the downloaded XAP
                    StreamResourceInfo sri = new StreamResourceInfo(e.Result, null);
                    XmlReader reader = XmlReader.Create(Application.GetResourceStream(sri, new Uri("AppManifest.xaml", UriKind.Relative)).Stream);
                    AssemblyPartCollection parts = new AssemblyPartCollection();

                    // Enumerate the assemblies in the downloaded XAP
                    if (reader.Read())
                    {
                        reader.ReadStartElement();
                        if (reader.ReadToNextSibling("Deployment.Parts"))
                        {
                            while (reader.ReadToFollowing("AssemblyPart"))
                            {
                                parts.Add(new AssemblyPart() { Source = reader.GetAttribute("Source") });
                            }
                        }
                    }

                    // Load the satellite assemblies
                    foreach (AssemblyPart part in parts)
                    {
                        string source = part.Source;
                        if (part.Source.ToLower().Contains("PalashDynamic.Localization.dll"))
                        {
                            Stream assembly = Application.GetResourceStream(sri, new Uri(part.Source, UriKind.Relative)).Stream;
                            part.Load(assembly);
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            else // Download error
            {

            }
        }

        public void ChangeCulture(CultureInfo culture)
        {
            string name = culture.Name.ToLower();

            if (name.StartsWith("hi-in") || name.StartsWith("fr-FR") || name.StartsWith("en-in"))
            {
                if (!_downloaded)
                {
                    // Download the XAP containing external localization resources
                    WebClient wc = new WebClient();
                    wc.OpenReadCompleted += new OpenReadCompletedEventHandler(OnOpenReadCompleted);
                    wc.OpenReadAsync(new Uri("PalashDynamics.xap", UriKind.Relative), culture);
                    return;
                }
            }

            // Change the culture
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = culture;

            // Fire a CultureChanged event
            if (CultureChanged != null)
                CultureChanged(this, new CultureChangedEventArgs(culture));
        }

        void OnOpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            CultureInfo culture = (CultureInfo)e.UserState;

            if (e.Error == null)
            {
                try
                {
                    _downloaded = true;

                    // Get the application manifest from the downloaded XAP
                    StreamResourceInfo sri = new StreamResourceInfo(e.Result, null);
                    XmlReader reader = XmlReader.Create(Application.GetResourceStream(sri, new Uri("AppManifest.xaml", UriKind.Relative)).Stream);
                    AssemblyPartCollection parts = new AssemblyPartCollection();

                    // Enumerate the assemblies in the downloaded XAP
                    if (reader.Read())
                    {
                        reader.ReadStartElement();
                        if (reader.ReadToNextSibling("Deployment.Parts"))
                        {
                            while (reader.ReadToFollowing("AssemblyPart"))
                            {
                                parts.Add(new AssemblyPart() { Source = reader.GetAttribute("Source") });
                            }
                        }
                    }

                    // Load the satellite assemblies
                    foreach (AssemblyPart part in parts)
                    {
                        string source = part.Source;
                        if (part.Source.ToLower().Contains("palashdynamics.dll"))
                        {
                            Stream assembly = Application.GetResourceStream(sri, new Uri(part.Source, UriKind.Relative)).Stream;
                            part.Load(assembly);
                        }
                    }

                    // Change the culture
                    Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = culture;

                    // Fire a CultureChanged event
                    if (CultureChanged != null)
                        CultureChanged(this, new CultureChangedEventArgs(culture));
                }
                catch (Exception ex)
                {
                    // If anything went wrong, fire a CultureChangeFailed event
                    if (CultureChangeFailed != null)
                        CultureChangeFailed(this, new CultureChangedErrorEventArgs(culture, ex));
                }
            }
            else // Download error
            {
                if (CultureChangeFailed != null)
                    CultureChangeFailed(this, new CultureChangedErrorEventArgs(culture, e.Error));
            }
        }

    }

    public class CultureChangedEventArgs : EventArgs
    {
        public CultureInfo Culture;

        public CultureChangedEventArgs(CultureInfo culture)
        {
            this.Culture = culture;
        }
    }

    public class CultureChangedErrorEventArgs : EventArgs
    {
        public CultureInfo Culture;
        public Exception Error;

        public CultureChangedErrorEventArgs(CultureInfo culture, Exception ex)
        {
            this.Culture = culture;
            this.Error = ex;
        }
    }
}
