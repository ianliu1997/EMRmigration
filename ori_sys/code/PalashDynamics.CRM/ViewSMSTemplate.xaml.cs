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
using PalashDynamics.ValueObjects;
using CIMS;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;

using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.CRM;
using System.IO;

namespace PalashDynamics.CRM
{
    public partial class ViewSMSTemplate : ChildWindow 
    {
        #region
        public long TemplateId { get; set; }
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        WaitIndicator waiting = new WaitIndicator();
        byte[] data;
        public string fileName;
        public long SMSTemplateId;
        public event RoutedEventHandler SMSOk_Click;
        public string msgText;
        public string msgTitle = "";
        public string SMSEnglishText { get; set; }
        public string SMSLocalText { get; set; }
        public string FileName;
        public string FilePath;

        #endregion
        public ViewSMSTemplate()
        {
            InitializeComponent();
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            try
            {
                ((ChildWindow)(this.Parent)).Close();
            }
            catch (Exception ex)
            {

            }
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FillSMS();
            GetLocalLanguage();
            //ShowSMSTemplateDetails(TemplateId);
        }

        private void GetLocalLanguage()
        {
            clsGetAppConfigBizActionVO objGetDetails = new clsGetAppConfigBizActionVO();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Result != null && ea.Error == null)
                {
                    clsAppConfigVO objApp = new clsAppConfigVO();
                    objApp = ((clsGetAppConfigBizActionVO)ea.Result).AppConfig;
                    FileName = objApp.FileName;
                    FilePath = objApp.FilePath;

                    Stream LocalFont = this.GetType().Assembly.GetManifestResourceStream(FilePath);
                    txtLocal.FontSource = new FontSource(LocalFont);
                    txtLocal.FontFamily = new FontFamily(FileName);
                    txtLocal.FontSize = 20;
                }
            };
            client.ProcessAsync(objGetDetails, new clsUserVO());
            client.CloseAsync();
        }

        void ShowSMSTemplateDetails(long SMSTemplateId)
        {
            waiting.Show();
            clsGetSMSTemplateDetailsBizActionVO obj = new clsGetSMSTemplateDetailsBizActionVO();
            obj.ID = SMSTemplateId;

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
                    //txtEnglish.Text = objTemp.EnglishText;
                    //txtLocal.Text = objTemp.LocalText;
                    //SMSEnglishText = objTemp.EnglishText;
                    //SMSLocalText = objTemp.LocalText;
                    //TemplateId = objTemp.ID;
                }
                waiting.Close();
            };
            client.ProcessAsync(obj, new clsUserVO());
            client.CloseAsync();
            client = null;

        }

        private void cmbSelectSMSTemplate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSelectSMSTemplate.SelectedItem != null)
            {
                SMSTemplateId = ((MasterListItem)cmbSelectSMSTemplate.SelectedItem).ID;
                ShowSMSTemplateDetails(SMSTemplateId);
            }
        }

        private void FillSMS()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.T_SMSTemplate;
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                    cmbSelectSMSTemplate.ItemsSource = null;
                    cmbSelectSMSTemplate.ItemsSource = objList;
                }
                if (this.DataContext != null)
                {
                    cmbSelectSMSTemplate.SelectedValue = ((clsCampMasterVO)this.DataContext).SMSTemplateDetails.Description;
                    SMSTemplateId = ((clsCampMasterVO)this.DataContext).SMSTemplateDetails.ID;
                }

                if ((clsSMSTemplateVO)cmbSelectSMSTemplate.SelectedItem != null)
                {
                    clsSMSTemplateVO objMaster = new clsSMSTemplateVO();
                    cmbSelectSMSTemplate.SelectedValue = objMaster.Description;
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
        }

        private void cmdSend_Click(object sender, RoutedEventArgs e)
        {
            msgText = "Are you sure you want to send the Message?";
            MessageBoxControl.MessageBoxChildWindow msgW =
                  new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

            msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
            {
                if (res == MessageBoxResult.Yes)
                {

                    this.DialogResult = true;

                    if (SMSOk_Click != null)
                        SMSOk_Click(this, new RoutedEventArgs());
                }
            };
            msgW.Show();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
    }
}