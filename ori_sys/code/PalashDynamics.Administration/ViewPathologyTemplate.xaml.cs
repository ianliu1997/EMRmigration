using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PalashDynamics.ValueObjects.Pathology;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;
using CIMS;
using C1.Silverlight;
using C1.Silverlight.RichTextBox.Documents;

namespace PalashDynamics.Administration
{
    public partial class ViewPathologyTemplate : ChildWindow
    {
        public long TemplateID { get; set; }
        public int Flag { get; set; }
        public ViewPathologyTemplate()
        {
            InitializeComponent();
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FillTemplate(TemplateID);
        }

        //public Liquid.RichTextEditor Text
        //{
        //    get { return richTextEditor; }
        //}

        private void FillTemplate(long iID)
        {
            clsGetPathoViewTemplateBizActionVO BizAction = new clsGetPathoViewTemplateBizActionVO();
            BizAction.Template = new clsPathoTestTemplateDetailsVO();
            BizAction.TemplateID = iID;
            BizAction.Flag = Flag;
            

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetPathoViewTemplateBizActionVO)arg.Result).Template != null)
                    {
                        richTextEditor.Html = ((clsGetPathoViewTemplateBizActionVO)arg.Result).Template.Template;
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();


        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var oldItem = e.RemovedItems.OfType<C1TabItem>().FirstOrDefault();
                if (oldItem == null) return; // richTextBoxTab and the others are null the first time around because InitializeComponent is running.

                if (oldItem == richTextBoxTab)
                {
                    htmlBox.Text = richTextEditor.Html;
                    rtfBox.Text = new RtfFilter().ConvertFromDocument(richTextEditor.Document);
                }
                else if (oldItem == htmlTab)
                {
                    richTextEditor.Html = htmlBox.Text;
                    rtfBox.Text = new RtfFilter().ConvertFromDocument(richTextEditor.Document);
                }
                else if (oldItem == rtfTab)
                {
                    richTextEditor.Document = new RtfFilter().ConvertToDocument(rtfBox.Text);
                    htmlBox.Text = richTextEditor.Html;
                }
            }
            catch { }
        }

    }
}

