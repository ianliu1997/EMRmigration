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
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.ValueObjects.DashBoardVO;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class AdditionalMeasure : ChildWindow
    {
        public long TherapyId { get; set;}
        public AdditionalMeasure()
        {
            InitializeComponent();
            
           
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            clsIVFDashboard_AddPlanTherapyAdditionalmeasureBizActionVO BizAction = new clsIVFDashboard_AddPlanTherapyAdditionalmeasureBizActionVO();
            BizAction.TherapyDetails = new clsPlanTherapyVO();
            BizAction.TherapyDetails = ((clsPlanTherapyVO)AddtionalMeasure.DataContext);
            BizAction.TherapyDetails.ID = TherapyId;
            #region Service Call (Check Validation)
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Addtional Measures Updated Successfully..", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.OnMessageBoxClosed += (result) =>
                    {
                        if (result == MessageBoxResult.OK)
                        {
                            this.Close();
                        }
                    };
                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
            #endregion

            //this.DialogResult = false;
            //Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
        private void GetAddtionalMeasure()
        {
            try
            {
                clsIVFDashboard_GetPlanTherapyAdditionalmeasureBizActionVO BizAction = new clsIVFDashboard_GetPlanTherapyAdditionalmeasureBizActionVO();
                BizAction.TherapyID = TherapyId;
                #region Service Call (Check Validation)
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        AddtionalMeasure.DataContext = ((clsIVFDashboard_GetPlanTherapyAdditionalmeasureBizActionVO)args.Result).TherapyDetails;
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                #endregion
            }
            catch (Exception ex)
            {
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            AddtionalMeasure.DataContext = new clsPlanTherapyVO();
            GetAddtionalMeasure();
        }
    }
}

