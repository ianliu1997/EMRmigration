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
using PalashDynamics.IVF.DashBoard;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.ValueObjects.DashBoardVO;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using System.Collections.ObjectModel;
using System.Text;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class frmReceiveEmbryo : ChildWindow
    {
        public clsCoupleVO CoupleDetails;
        public long PlanTherapyID;
        public long PlanTherapyUnitID;
        public string SourceOfOocyteCode;
        public long DonorID;
        public long DonorUnitID;
        public long PatientID;
        public long PatientUnitID;
        public bool IsClosed;
        public event RoutedEventHandler OnCloseButton_Click;
        public frmReceiveEmbryo()
        {
            InitializeComponent();
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save OPU details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                msgW.Show();
            }
        }
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                SaveDetails();
        }
        private bool Validate()
        {
            bool result = true;
            if (PrcDate.SelectedDate == null)
            {
                PrcDate.SetValidation("Please Select Date");
                PrcDate.RaiseValidationError();
                PrcDate.Focus();
                return false;
            }
            else
                PrcDate.ClearValidationError();
            if (VitriDetails.Count <= 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "You cannot Save the details without Embryo", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
                return false;
            }
            return result;
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
        private void cmdGetEmbryos_Click(object sender, RoutedEventArgs e)
        {
            frmCryoEmbryoList win = new frmCryoEmbryoList();
            win.PatientID = DonorID;
            win.PatientUnitID = DonorUnitID;
            win.MRNO = SourceOfOocyteCode;
            win.OnSaveButton_Click += new RoutedEventHandler(OnSaveButtonWin_Click);
            win.Show();
        }
        private ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO> _VitriDetails = new ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO>();
        public ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO> VitriDetails
        {
            get { return _VitriDetails; }
            set { _VitriDetails = value; }
        }
        private void OnSaveButtonWin_Click(object sender, RoutedEventArgs e)
        {
            if (((frmCryoEmbryoList)sender).SelectedBatches != null && ((frmCryoEmbryoList)sender).SelectedBatches.Count > 0)
            {
                VitriDetails = new ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO>();
                VitriDetails = ((frmCryoEmbryoList)sender).SelectedBatches;
                EmbryoDataGrid.ItemsSource = null;
                EmbryoDataGrid.ItemsSource = VitriDetails;


            }
        }
        private void SaveDetails()
        {
            try
            {
                clsIVFDashboard_UpdateVitrificationDetailsBizActionVO BizAction = new clsIVFDashboard_UpdateVitrificationDetailsBizActionVO();
                BizAction.VitrificationMain = new clsIVFDashBoard_VitrificationVO();
                BizAction.VitrificationDetailsList = new List<clsIVFDashBoard_VitrificationDetailsVO>();
                BizAction.VitrificationMain.PlanTherapyID = PlanTherapyID;
                BizAction.VitrificationMain.PlanTherapyUnitID = PlanTherapyUnitID;
                BizAction.VitrificationMain.PatientID = PatientID;
                BizAction.VitrificationMain.PatientUnitID = PatientUnitID;
                BizAction.VitrificationMain.DonorPatientID = DonorID;
                BizAction.VitrificationMain.DonorPatientUnitID = DonorUnitID;
                if (PrcDate.SelectedDate != null)
                    BizAction.VitrificationMain.ReceivingDate = PrcDate.SelectedDate.Value.Date;
                BizAction.VitrificationDetailsList = VitriDetails.ToList();

                StringBuilder SerialOocyteNo = new StringBuilder();
                if (VitriDetails != null && VitriDetails.Count > 0)
                {
                    foreach (var r in VitriDetails)
                    {
                        if (SerialOocyteNo.Length == 0)
                        {
                            SerialOocyteNo.Append(r.EmbSerialNumber);
                        }
                        else
                        {
                            SerialOocyteNo.Append(",");
                            SerialOocyteNo.Append(r.EmbSerialNumber);
                        }
                    }
                }
                if (SerialOocyteNo != null)
                    BizAction.VitrificationMain.SerialOocyteNumberString = Convert.ToString(SerialOocyteNo);


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Result != null && arg.Error == null)
                    {
                        string strMsg = "Details Added Sucessfully";
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        fillGrid();

                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        private void fillGrid()
        {

            try
            {
                clsIVFDashboard_GetUsedEmbryoDetailsBizActionVO BizAction = new clsIVFDashboard_GetUsedEmbryoDetailsBizActionVO();
                BizAction.VitrificationMain = new clsIVFDashBoard_VitrificationVO();
                BizAction.VitrificationDetailsList = new List<clsIVFDashBoard_VitrificationDetailsVO>();
                BizAction.VitrificationMain.PlanTherapyID = PlanTherapyID;
                BizAction.VitrificationMain.PlanTherapyUnitID = PlanTherapyUnitID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Result != null && arg.Error == null)
                    {
                        if (((clsIVFDashboard_GetUsedEmbryoDetailsBizActionVO)arg.Result).VitrificationDetailsList != null)
                        {
                            VitriDetails = new ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO>();
                            foreach (clsIVFDashBoard_VitrificationDetailsVO r in ((clsIVFDashboard_GetUsedEmbryoDetailsBizActionVO)arg.Result).VitrificationDetailsList)
                            { VitriDetails.Add(r); }
                            if (VitriDetails.Count > 0)
                            {
                                EmbryoDataGrid.ItemsSource = null;
                                EmbryoDataGrid.ItemsSource = VitriDetails;
                                PrcDate.SelectedDate = VitriDetails[0].ReceivingDate;
                                cmdSave.IsEnabled = false;
                            }
                        }

                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (SourceOfOocyteCode != null)
                txtSourceofEmbryo.Text = SourceOfOocyteCode;
            fillGrid();
        }

        private void cmdSave_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
