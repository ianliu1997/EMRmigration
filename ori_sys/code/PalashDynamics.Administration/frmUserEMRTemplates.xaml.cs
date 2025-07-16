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
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.User;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.Administration
{
    public partial class frmUserEMRTemplates : ChildWindow
    {
        WaitIndicator Indicatior = null;
       // public event RoutedEventHandler OnOKButton_Click;
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        public long UserID { get; set; }
        public frmUserEMRTemplates()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(frmUserEMRTemplates_Loaded);
        }

        void frmUserEMRTemplates_Loaded(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            FillUserEMRTemplateList();
        }
          private void FillUserEMRTemplateList()
        {
            Indicatior = new WaitIndicator();
            Indicatior.Show();

            try
            {
                clsGetUserEMRTemplateListBizActionVO BizAction = new clsGetUserEMRTemplateListBizActionVO();
                
               // BizAction.List = new List<clsServiceMasterVO>();

                

                // if (cmbTariff.SelectedItem != null)
                BizAction.UserID = UserID;
               // BizAction.ClassID = ClassID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                dgTemplateList.ItemsSource = null;
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null && ((clsGetUserEMRTemplateListBizActionVO)arg.Result).List != null)
                    {
                        if (((clsGetUserEMRTemplateListBizActionVO)arg.Result).List != null)
                        {
                            dgTemplateList.ItemsSource = null;
                            //check.Clear();
                            dgTemplateList.ItemsSource = ((clsGetUserEMRTemplateListBizActionVO)arg.Result).List;
                          
                        }
                    }
                    else
                    {
                        //HtmlPage.Window.Alert("Error occured while processing.");
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }

                    Indicatior.Close();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                // throw;
                Indicatior.Close();
            }
            finally
            {
                Indicatior.Close();
            }

        }


        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            string msgText = "Are you sure you want to Save User EMR Template ?";

            MessageBoxControl.MessageBoxChildWindow msgWD =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

            msgWD.OnMessageBoxClosed += (arg) =>
            {
                if (arg == MessageBoxResult.Yes)
                {
                    SaveUserEMRTemplateDetails();
                }
              
            };
            msgWD.Show();

        }

        private void SaveUserEMRTemplateDetails()
        {
            Indicatior = new WaitIndicator();
            Indicatior.Show();

            try
            {
                clsAssignUserEMRTemplatesBizActionVO BizAction = new clsAssignUserEMRTemplatesBizActionVO();

                BizAction.Details = new List<clsUserEMRTemplateDetailsVO>();

                BizAction.Details = (List<clsUserEMRTemplateDetailsVO>)dgTemplateList.ItemsSource;

                // if (cmbTariff.SelectedItem != null)
                BizAction.UserID = UserID;
                // BizAction.ClassID = ClassID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
               // dgTemplateList.ItemsSource = null;
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null && ((clsAssignUserEMRTemplatesBizActionVO)arg.Result).Details != null)
                    {
                        if (((clsAssignUserEMRTemplatesBizActionVO)arg.Result).Details != null)
                        {

                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Template Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    }
                    else
                    {
                        //HtmlPage.Window.Alert("Error occured while processing.");
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }

                    Indicatior.Close();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                // throw;
                Indicatior.Close();
            }
            finally
            {
                Indicatior.Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        

        
    }
}

