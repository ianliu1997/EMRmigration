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
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using CIMS;
using System.Windows.Media.Imaging;
using System.Windows.Browser;
using PalashDynamics.ValueObjects.DashBoardVO;
using System.IO;
using System.Reflection;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class FrmCryoRenewalDate : ChildWindow
    {
        public event RoutedEventHandler OnCloseButton_Click;
        public bool IsOocyteFreezed;
        public bool IsSprem;       
        public long VitificationID ;
	    public long VitificationUnitID ;
	    public long VitificationDetailsID ;
	    public long VitificationDetailsUnitID ;
	    public long SpremFreezingID ;
	    public long SpremFreezingUnitID ;
        public DateTime? PriviousExpiryDate;
        //public Date ,
        //public ExpiryDate ,
        //public ExpiryTime ,
        //public LongTerm ,
        //public ShortTerm ,
	


        public FrmCryoRenewalDate()
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

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            if (OnCloseButton_Click != null)
                OnCloseButton_Click(this, new RoutedEventArgs());
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private bool Validate()
        {
            bool result = true;
            DateTime? dtDate;
            DateTime? dtExDate;

            if (dtObservationDate.SelectedDate == null)
            {
                dtObservationDate.SetValidation("Please Select Date");
                dtObservationDate.RaiseValidationError();
                dtObservationDate.Focus();
                return false;
            }
            else
                dtObservationDate.ClearValidationError();

            if (dtObservationTime.Value == null)
            {
                dtObservationTime.SetValidation("Please Select Time");
                dtObservationTime.RaiseValidationError();
                dtObservationTime.Focus();
                return false;
            }
            else
                dtObservationTime.ClearValidationError();


            if (dtExpiryDate.SelectedDate == null)
            {
                dtExpiryDate.SetValidation("Please Select Date");
                dtExpiryDate.RaiseValidationError();
                dtExpiryDate.Focus();
                return false;
            }
            else
                dtObservationDate.ClearValidationError();

            if (dtObservationTime.Value == null)
            {
                dtObservationTime.SetValidation("Please Select Time");
                dtObservationTime.RaiseValidationError();
                dtObservationTime.Focus();
                return false;
            }
            else
                dtObservationTime.ClearValidationError();

            dtDate = dtObservationDate.SelectedDate.Value.Date;
            dtDate = dtDate.Value.Add(dtObservationTime.Value.Value.TimeOfDay);

            if (dtDate < PriviousExpiryDate)
            {
                dtObservationDate.SetValidation("Start Date Cannot Be Less Than Privious Expiry Date");
                dtObservationDate.RaiseValidationError();
                dtObservationDate.Focus();
                dtObservationDate.Text = " ";
                dtObservationDate.Focus();
                result = false;
            }
            else
                dtObservationDate.ClearValidationError();

            dtExDate = dtExpiryDate.SelectedDate.Value.Date;
            dtExDate = dtExDate.Value.Add(dtExpiryTime.Value.Value.TimeOfDay);

            if (dtExDate < dtDate)
            {
                dtExpiryDate.SetValidation("Expiry Date Cannot Be Less Than Start Date  ");
                dtExpiryDate.RaiseValidationError();
                dtExpiryDate.Focus();
                dtExpiryDate.Text = " ";
                dtExpiryDate.Focus();
                result = false;
            }
            else
                dtExpiryDate.ClearValidationError();


            return result;
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                msgW.Show();
            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                Save();           
        }

        private void Save()
        {
            clsIVFDashboard_AddUpdateVitrificationBizActionVO BizAction = new clsIVFDashboard_AddUpdateVitrificationBizActionVO();          
            BizAction.IsRenewal = true;
            BizAction.IsOocyteFreezed = IsOocyteFreezed;
            BizAction.IsSprem = IsSprem;
            BizAction.VitificationID = VitificationID;
            BizAction.VitificationUnitID = VitificationUnitID;
            BizAction.VitificationDetailsID = VitificationDetailsID;
            BizAction.VitificationDetailsUnitID = VitificationDetailsUnitID;
            BizAction.SpremFreezingID = SpremFreezingID;
            BizAction.SpremFreezingUnitID = SpremFreezingUnitID;
            BizAction.StartDate = dtObservationDate.SelectedDate.Value.Date;
            BizAction.StartDate = BizAction.StartDate.Value.Add(dtObservationTime.Value.Value.TimeOfDay);
            BizAction.StartTime = dtObservationTime.Value.Value;
            BizAction.ExpiryDate = dtExpiryDate.SelectedDate.Value.Date;
            BizAction.ExpiryDate = BizAction.ExpiryDate.Value.Add(dtExpiryTime.Value.Value.TimeOfDay);
            BizAction.ExpiryTime = dtExpiryTime.Value.Value;
            if (rdoLongTerm.IsChecked == true)
                BizAction.LongTerm = true;
            else
                BizAction.LongTerm = false;
            if (rdoShortTerm.IsChecked == true)
                BizAction.ShortTerm = true;
            else
                BizAction.ShortTerm = false;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();                   
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
    }
}

