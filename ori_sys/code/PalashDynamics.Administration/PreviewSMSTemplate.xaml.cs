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
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects;
using PalashDynamics.UserControls;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;

namespace PalashDynamics.Administration
{
    public partial class PreviewSMSTemplate : UserControl
    {
        #region Public Variables
        public long TemplateId { get; set; }
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        WaitIndicator waiting = new WaitIndicator();        
        #endregion

        public PreviewSMSTemplate()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ShowSMSTemplateDetails(TemplateId);
        }
        private void ShowSMSTemplateDetails(long SMSTemplateID)
        {
            waiting.Show();
            clsGetSMSTemplateDetailsBizActionVO obj = new clsGetSMSTemplateDetailsBizActionVO();
            obj.ID = SMSTemplateID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Error == null && ea.Result != null)
                {
                    clsSMSTemplateVO objTemp = new clsSMSTemplateVO();
                    objTemp = ((clsGetSMSTemplateDetailsBizActionVO)ea.Result).SMSDetails;

                    if (objTemp.ID != null)
                    {
                        txtCode.Text = objTemp.Code;
                        txtName.Text = objTemp.Description;
                        txtEnglish.Text = objTemp.EnglishText;
                        txtLocal.Text = objTemp.LocalText;
                    }
                }
                waiting.Close();
            };
            client.ProcessAsync(obj, new clsUserVO());
            client.CloseAsync();
            client = null;
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ((ChildWindow)(this.Parent)).Close();
            }
            catch (Exception ex)
            {
            }
        }

    }
}
