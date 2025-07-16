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
using PalashDynamics.ValueObjects.DashBoardVO;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects.IVFPlanTherapy;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class frmApplyBatchForDonor : ChildWindow
    {
        public frmApplyBatchForDonor()
        {
            InitializeComponent();
        }
        #region variables and Properties
        public long DonorID, DonorUnitID;
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        private ObservableCollection<clsNew_SpremFreezingVO> _SpremFreezingDetails = new ObservableCollection<clsNew_SpremFreezingVO>();
        public ObservableCollection<clsNew_SpremFreezingVO> SpremFreezingDetails
        {
            get { return _SpremFreezingDetails; }
            set { _SpremFreezingDetails = value; }
        }

        private List<MasterListItem> _ColorList = new List<MasterListItem>();
        public List<MasterListItem> ColorList
        {
            get
            {
                return _ColorList;
            }
            set
            {
                _ColorList = value;
            }
        }

        private List<MasterListItem> _CanList = new List<MasterListItem>();
        public List<MasterListItem> CanList
        {
            get
            {
                return _CanList;
            }
            set
            {
                _CanList = value;
            }
        }

        private List<MasterListItem> _StrawList = new List<MasterListItem>();
        public List<MasterListItem> StrawList
        {
            get
            {
                return _StrawList;
            }
            set
            {
                _StrawList = value;
            }
        }

        private List<MasterListItem> _GlobletShapeList = new List<MasterListItem>();
        public List<MasterListItem> GlobletShapeList
        {
            get
            {
                return _GlobletShapeList;
            }
            set
            {
                _GlobletShapeList = value;
            }
        }

        private List<MasterListItem> _CanisterList = new List<MasterListItem>();
        public List<MasterListItem> CanisterList
        {
            get
            {
                return _CanisterList;
            }
            set
            {
                _CanisterList = value;
            }
        }

        private List<MasterListItem> _GlobletSizeList = new List<MasterListItem>();
        public List<MasterListItem> GlobletSizeList
        {
            get
            {
                return _GlobletSizeList;
            }
            set
            {
                _GlobletSizeList = value;
            }
        }

        private List<MasterListItem> _TankList = new List<MasterListItem>();
        public List<MasterListItem> TankList
        {
            get
            {
                return _TankList;
            }
            set
            {
                _TankList = value;
            }
        }
        WaitIndicator wait = new WaitIndicator();
        #endregion

        #region Fill Combobox
        private void FillLab()
        {
            try
            {

                wait.Show();

                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_LaboratoryMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        cmbLab.ItemsSource = null;
                        cmbLab.ItemsSource = objList.DeepCopy();
                        cmbLab.SelectedItem = objList[0];

                        FillReceivedBy();
                    }
                    if (this.DataContext != null)
                    {

                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
                wait.Close();
            }

        }
        private void FillReceivedBy()
        {
            try
            {
                clsGetEmbryologistBizActionVO BizAction = new clsGetEmbryologistBizActionVO();
                BizAction.MasterList = new List<MasterListItem>();
                BizAction.UnitId = 0;
                BizAction.DepartmentId = 0;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetEmbryologistBizActionVO)arg.Result).MasterList);

                        cmbReceivedBy.ItemsSource = null;
                        cmbReceivedBy.ItemsSource = objList;
                        cmbReceivedBy.SelectedItem = objList[0];

                    }
                    FillColor();
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
                wait.Close();
            }
        }
        private void FillColor()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVFGobletColor;
                BizAction.Parent = new KeyValue();
                BizAction.Parent.Key = "1";
                BizAction.Parent.Value = "Status";
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        ColorList = null;
                        ColorList = objList;
                    }
                    FillStraw();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
                wait.Close();
            }
        }
        private void FillStraw()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVFStrawMaster;
                BizAction.Parent = new KeyValue();
                BizAction.Parent.Key = "1";
                BizAction.Parent.Value = "Status";
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        StrawList = null;
                        StrawList = objList;
                    }
                    FillGlobletShape();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
                wait.Close();
            }
        }
        private void FillGlobletShape()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVFGobletShapeMaster;
                BizAction.Parent = new KeyValue();
                BizAction.Parent.Key = "1";
                BizAction.Parent.Value = "Status";
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        GlobletShapeList = null;
                        GlobletShapeList = objList;
                    }
                    FillGlobletSize();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
                wait.Close();
            }
        }
        private void FillGlobletSize()
        {
            try
            {

                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVFGobletSizeMaster;
                BizAction.Parent = new KeyValue();
                BizAction.Parent.Key = "1";
                BizAction.Parent.Value = "Status";
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        GlobletSizeList = null;
                        GlobletSizeList = objList;
                    }
                    FillCane();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
                wait.Close();
            }
        }
        private void FillCane()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVFCanMaster;
                BizAction.Parent = new KeyValue();
                BizAction.Parent.Key = "1";
                BizAction.Parent.Value = "Status";
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        CanList = null;
                        CanList = objList;
                    }
                    FillCanister();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
                wait.Close();
            }
        }
        private void FillCanister()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVFCanisterMaster;
                BizAction.Parent = new KeyValue();
                BizAction.Parent.Key = "1";
                BizAction.Parent.Value = "Status";
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        CanisterList = null;
                        CanisterList = objList;
                    }
                    FillTank();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
                wait.Close();
            }
        }
        private void FillTank()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVFTankMaster;
                BizAction.Parent = new KeyValue();
                BizAction.Parent.Key = "1";
                BizAction.Parent.Value = "Status";
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        TankList = null;
                        TankList = objList;
                    }
                    FillViscosity();
                    // FillGrid();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
                wait.Close();
            }
            catch (Exception ex)
            {
                throw ex;
                wait.Close();
            }
        }
        void FillViscosity()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVF_Viscosity;
                BizAction.Parent = new KeyValue();
                BizAction.Parent.Key = "1";
                BizAction.Parent.Value = "Status";
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        cmbViscosity.ItemsSource = null;
                        cmbViscosity.ItemsSource = objList;
                        cmbViscosity.SelectedItem = objList[0];
                    }
                    if (this.DataContext != null)
                    {
                        //cmbSrcTreatmentPlan.SelectedValue = ((clsVO)this.DataContext).;
                    }
                    wait.Close();
                };

                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
                wait.Close();
            }
        }
        #endregion

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtNumber_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtNumber_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtNoofVials_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsNumberValid() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
            else
            {
                if (txtNoofVials.Text != null)
                {
                    dgSpremFreezingDetailsGrid.ItemsSource = null;
                    SpremFreezingDetails = new ObservableCollection<clsNew_SpremFreezingVO>();
                    for (int i = 0; i < Convert.ToInt32(txtNoofVials.Text); i++)
                    {
                        SpremFreezingDetails.Add(new clsNew_SpremFreezingVO() { SpremNostr = "Auto Generated", GobletColorID = 0, ColorListVO = ColorList, StrawId = 0, StrawListVO = StrawList, GobletShapeId = 0, GlobletShapeListVO = GlobletShapeList, GobletSizeId = 0, GlobletSizeListVO = GlobletSizeList, CanID = 0, CanListVO = CanList, CanisterId = 0, CanisterListVO = CanisterList, TankId = 0, TankListVO = TankList, Comments = "", IsThaw = false });

                    }
                    dgSpremFreezingDetailsGrid.ItemsSource = SpremFreezingDetails;
                }
            }
        }

        private void txtNoofVials_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtTotalSpremCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsNumberValid() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtTotalSpremCount_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtvolume_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValueDouble() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtvolume_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtGradeA_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtGradeA_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValueDouble() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtGradeB_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtGradeB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValueDouble() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtGradeC_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtGradeC_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValueDouble() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtMotility_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtMotility_KeyDown(object sender, KeyEventArgs e)
        {

        }



        private void dgSpremFreezingDetailsGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {

        }

        private void cmbGrade_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        public bool Validate()
        {
            bool result = true;
            if (cmbLab.SelectedItem == null)
            {
                cmbLab.TextBox.SetValidation("Please select Lab");
                cmbLab.TextBox.RaiseValidationError();
                cmbLab.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbLab.SelectedItem).ID == 0)
            {
                cmbLab.TextBox.SetValidation("Please select Lab");
                cmbLab.TextBox.RaiseValidationError();
                cmbLab.Focus();
                result = false;
            }
            else
                cmbLab.TextBox.ClearValidationError();

            if (dtpReceivedDate.SelectedDate == null)
            {
                dtpReceivedDate.SetValidation("Please Select Received Date");
                dtpReceivedDate.RaiseValidationError();
                dtpReceivedDate.Focus();
                return false;
            }
            else
                dtpReceivedDate.ClearValidationError();
            if (txtBatch.Text == string.Empty)
            {
                txtBatch.SetValidation("Please Enter Batch Code");
                txtBatch.RaiseValidationError();
                txtBatch.Focus();
                result = false;
            }
            else
                txtBatch.ClearValidationError();
            if (txtNoofVials.Text == string.Empty)
            {
                txtNoofVials.SetValidation("Please Enter Number of Vails");
                txtNoofVials.RaiseValidationError();
                txtNoofVials.Focus();
                result = false;
            }
            else if (Convert.ToInt16(txtNoofVials.Text.Trim()) <= 0)
            {
                txtNoofVials.SetValidation("Number of Vails should be greater than zero");
                txtNoofVials.RaiseValidationError();
                txtNoofVials.Focus();
                result = false;
            }
            else
                txtNoofVials.ClearValidationError();
            if (txtvolume.Text == string.Empty)
            {
                txtvolume.SetValidation("Please Enter Number Volume");
                txtvolume.RaiseValidationError();
                txtvolume.Focus();
                result = false;
            }
            else
                txtvolume.ClearValidationError();

            return result;
        }
        int ClickedFlag = 0;
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;

            if (ClickedFlag == 1)
            {
                if (Validate())
                {
                    string msgText = "";
                    msgText = "Are You Sure \n You Want To Save The Batch Details?";
                    MessageBoxControl.MessageBoxChildWindow msgW =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                    msgW.Show();
                }
            }
            else
                ClickedFlag = 0;
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                SaveDetails();
            else
                ClickedFlag = 0;

        }
        int j = 0;
        bool ISEdit = false;
        private void SaveDetails()
        {
            try
            {
                clsAddUpdateDonorBatchBizActionVO BizActionObj = new clsAddUpdateDonorBatchBizActionVO();
                BizActionObj.BatchDetails = new ValueObjects.IVFPlanTherapy.clsSemenSampleBatchVO();
                BizActionObj.FreezingObj = new cls_NewSpremFreezingMainVO();
                BizActionObj.FreezingDetailsList = new List<clsNew_SpremFreezingVO>();

                //Batch Details
                BizActionObj.BatchDetails.PatientID = DonorID;
                BizActionObj.BatchDetails.PatientUnitID = DonorUnitID;

                if (txtBatch.Text != string.Empty)
                    BizActionObj.BatchDetails.BatchCode = txtBatch.Text;
                if (dtpReceivedDate.SelectedDate != null)
                    BizActionObj.BatchDetails.ReceivedDate = dtpReceivedDate.SelectedDate.Value.Date;
                if (cmbReceivedBy.SelectedItem != null)
                    BizActionObj.BatchDetails.ReceivedByID = ((MasterListItem)cmbReceivedBy.SelectedItem).ID;
                if (cmbLab.SelectedItem != null)
                    BizActionObj.BatchDetails.LabID = ((MasterListItem)cmbLab.SelectedItem).ID;
                BizActionObj.BatchDetails.InvoiceNo = txtInvoiceNumber.Text;
                if (txtNoofVials.Text != null)
                    BizActionObj.BatchDetails.NoOfVails = Convert.ToInt32(txtNoofVials.Text);
                if (txtvolume.Text != null)
                    BizActionObj.BatchDetails.Volume = Convert.ToSingle(txtvolume.Text.Trim());
                BizActionObj.BatchDetails.Remark = txtRemark.Text;
                BizActionObj.BatchDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;


                //freezing
                BizActionObj.FreezingObj.PatientID = DonorID;
                BizActionObj.FreezingObj.PatientUnitID = DonorUnitID;
                if (dtpReceivedDate.SelectedDate != null)
                {
                    BizActionObj.FreezingObj.SpremFreezingDate = dtpReceivedDate.SelectedDate.Value;
                    BizActionObj.FreezingObj.SpremFreezingTime = dtpReceivedDate.SelectedDate.Value;
                }
                if (cmbReceivedBy.SelectedItem != null)
                {
                    BizActionObj.FreezingObj.DoctorID = ((MasterListItem)cmbReceivedBy.SelectedItem).ID;
                    BizActionObj.FreezingObj.EmbryologistID = ((MasterListItem)cmbReceivedBy.SelectedItem).ID;
                }

                // BizActionObj.FreezingObj.CollectionMethodID = ((MasterListItem)cmbSpremCollecionMethod.SelectedItem).ID;
                //  BizActionObj.FreezingObj.CollectionProblem = Convert.ToString(txtcollectnprblm.Text.Trim());
                if (txtAbstinence.Text != string.Empty)
                    BizActionObj.FreezingObj.Abstience = Convert.ToString(txtAbstinence.Text.Trim());
                if (txtvolume.Text != string.Empty)
                    BizActionObj.FreezingObj.Volume = Convert.ToSingle(txtvolume.Text.Trim());
                if (((MasterListItem)cmbViscosity.SelectedItem) != null)
                    BizActionObj.FreezingObj.ViscosityID = ((MasterListItem)cmbViscosity.SelectedItem).ID;
                if (txtGradeA.Text != string.Empty)
                    BizActionObj.FreezingObj.GradeA = Convert.ToDecimal(txtGradeA.Text.Trim());
                if (txtGradeB.Text != string.Empty)
                    BizActionObj.FreezingObj.GradeB = Convert.ToDecimal(txtGradeB.Text.Trim());
                if (txtGradeC.Text != string.Empty)
                    BizActionObj.FreezingObj.GradeC = Convert.ToDecimal(txtGradeC.Text.Trim());
                if (txtTotalSpremCount.Text != string.Empty)
                    BizActionObj.FreezingObj.TotalSpremCount = Convert.ToInt64(txtTotalSpremCount.Text.Trim());
                if (txtMotility.Text != string.Empty)
                    BizActionObj.FreezingObj.Motility = Convert.ToDecimal(txtMotility.Text.Trim());
                if (txtOtherDetails.Text != string.Empty)
                    BizActionObj.FreezingObj.Other = txtOtherDetails.Text.Trim();
                if (txtComments.Text != string.Empty)
                    BizActionObj.FreezingObj.Comments = txtComments.Text.Trim();
                BizActionObj.FreezingObj.Status = true;



                //freezing details

                for (int i = 0; i < SpremFreezingDetails.Count; i++)
                {
                    BizActionObj.FreezingDetailsList.Add(SpremFreezingDetails[i]);
                    //BizActionObj.FreezingDetailsList[i].ID = SpremFreezingDetails[i].ID;
                    BizActionObj.FreezingDetailsList[i].GobletSizeId = SpremFreezingDetails[i].selectedGlobletSizeListVO.ID;
                    BizActionObj.FreezingDetailsList[i].GobletColorID = SpremFreezingDetails[i].selectedColorList.ID;
                    BizActionObj.FreezingDetailsList[i].GobletShapeId = SpremFreezingDetails[i].selectedGlobletShapeListVO.ID;
                    BizActionObj.FreezingDetailsList[i].CanID = SpremFreezingDetails[i].selectedCanListVO.ID;
                    BizActionObj.FreezingDetailsList[i].CanisterId = SpremFreezingDetails[i].selectedCanisterListVO.ID;
                    BizActionObj.FreezingDetailsList[i].StrawId = SpremFreezingDetails[i].selectedStrawListVO.ID;
                    BizActionObj.FreezingDetailsList[i].TankId = SpremFreezingDetails[i].selectedTankListVO.ID;
                    BizActionObj.FreezingDetailsList[i].Status = SpremFreezingDetails[i].Status;
                    BizActionObj.FreezingDetailsList[i].IsThaw = SpremFreezingDetails[i].IsThaw;
                    BizActionObj.FreezingDetailsList[i].IsModify = ISEdit;
                    BizActionObj.FreezingDetailsList[i].Status = true;
                    if (SpremFreezingDetails[i].SpremNostr.Equals("Auto Generated"))
                    {
                        SpremFreezingDetails[i].SpremNostr = "0";
                    }
                    j = i + 1;
                    BizActionObj.FreezingDetailsList[i].SpremNostr = Convert.ToString(j);
                }
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (res == MessageBoxResult.OK)
                            {
                                this.DialogResult = true;
                            }
                        };
                        msgW1.Show();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

                wait.Close();
            }
            catch (Exception ex)
            {
                wait.Show();
                throw ex;
            }
        }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FillLab();
        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
    }
}
