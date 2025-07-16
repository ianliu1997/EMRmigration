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
using PalashDynamics.ValueObjects.Administration.DoctorShareRange;
using System.ComponentModel;
using PalashDynamics.Animations;
using System.Reflection;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Administration;

namespace PalashDynamics.Administration
{
    public partial class frmDoctorShareRange : UserControl
    {
        #region Variable & List Declarations
        public string Action { get; set; }
        private SwivelAnimation objAnimation;
        public bool IsCancel;
        public bool IsStatusChecked = false;
        public bool IsValid = true;
        #endregion

        #region Paging
        public PagedSortableCollectionView<clsDoctorShareRangeVO> DataList { get; private set; }
        public int DataListPageSize
        {
            get
            {
                return DataList.PageSize;
            }
            set
            {
                if (value == DataList.PageSize) return;
                DataList.PageSize = value;
            }
        }
        #endregion

        #region Constructor

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
        public frmDoctorShareRange()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            SetCommandButtonState("Load");
            DataList = new PagedSortableCollectionView<clsDoctorShareRangeVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;

            this.dataGrid2Pager.DataContext = DataList;
            this.grdMaster.DataContext = DataList;
        }
        #endregion

        #region Loaded Event
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FetchData();
        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchData();
        }

        #endregion

        #region FrontPanel
        #region ToggleButton Click & Check Box Click Event
        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("New");
            ClearData();
            grdMasterBackPanel.DataContext = new clsDoctorShareRangeVO();
            try
            {
                objAnimation.Invoke(RotationType.Forward);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("PALASH", "Do You Want To Save?.", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        SaveDoctorshareMasterRange();
                    }
                };
                msgW1.Show();
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Cancel");
            objAnimation.Invoke(RotationType.Backward);
            if (IsCancel == true)
            {
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Billing Configuration";
                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmBillingConfiguration") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData as UIElement);
            }
            else
            {
                IsCancel = true;
            }
        }

        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            if (grdMaster.SelectedItem != null)
            {
                try
                {
                    objAnimation.Invoke(RotationType.Forward);
                    SetCommandButtonState("View");
                    clsDoctorShareRangeVO objVO = ((clsDoctorShareRangeVO)grdMaster.SelectedItem);
                    txtUpperLimit.Text = Convert.ToString(objVO.UpperLimit);
                    txtLowerLimit.Text = Convert.ToString(objVO.LowerLimit);
                    txtShareAmount.Text = Convert.ToString(objVO.ShareAmount);
                    txtSharePercentage.Text = Convert.ToString(objVO.SharePercentage);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (grdMaster.SelectedItem != null)
            {
                CheckBox chk = sender as CheckBox;
                if (chk.IsChecked == true)
                {
                    IsStatusChecked = true;
                }
                else
                {
                    IsStatusChecked = false;
                }
                ChangeStatus();
            }
        }
        #endregion

        #region Set Command Button State New/Save/Modify/Print
        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdAdd.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "New":
                    cmdSave.IsEnabled = true;
                    cmdAdd.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    txtUpperLimit.Text = "0.000";
                    txtLowerLimit.Text = "0.000";
                    txtShareAmount.Text = "0.000";
                    txtSharePercentage.Text = "0.00";
                    break;
                case "Save":
                    cmdAdd.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Modify":
                    cmdAdd.IsEnabled = true;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Cancel":
                    cmdAdd.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;
                case "View":
                    cmdSave.IsEnabled = false;
                    cmdAdd.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;
                default:
                    break;
            }
        }
        #endregion

        #endregion

        #region BackPanel

        #endregion

        #region Private Methods
        private void SaveDoctorshareMasterRange()
        {
            clsAddDoctorShareRangeBizActionVO BizAction = new clsAddDoctorShareRangeBizActionVO();
            BizAction.ShareRangeDetails = new clsDoctorShareRangeVO();
            if (!String.IsNullOrEmpty(txtUpperLimit.Text))
                BizAction.ShareRangeDetails.UpperLimit = Convert.ToDecimal(txtUpperLimit.Text);
            if (!String.IsNullOrEmpty(txtLowerLimit.Text))
                BizAction.ShareRangeDetails.LowerLimit = Convert.ToDecimal(txtLowerLimit.Text);
            if (!String.IsNullOrEmpty(txtSharePercentage.Text))
                BizAction.ShareRangeDetails.SharePercentage = Convert.ToDecimal(txtSharePercentage.Text);
            if (!String.IsNullOrEmpty(txtShareAmount.Text))
                BizAction.ShareRangeDetails.ShareAmount = Convert.ToDecimal(txtShareAmount.Text);
            BizAction.ShareRangeDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && ((clsAddDoctorShareRangeBizActionVO)arg.Result).SuccessStatus == 1)
                {
                    objAnimation.Invoke(RotationType.Backward);
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("PALASH", "RECORD SAVED SUCCESSFULLY.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.OK)
                        {
                            FetchData();
                            ClearData();
                            SetCommandButtonState("Load");
                        }
                    };
                    msgW1.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("PALASH", "Error occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void ChangeStatus()
        {
            clsAddDoctorShareRangeBizActionVO BizAction = new clsAddDoctorShareRangeBizActionVO();
            BizAction.ShareRangeDetails = new clsDoctorShareRangeVO();
            if (grdMaster.SelectedItem != null)
            {
                BizAction.ShareRangeDetails.ID = ((clsDoctorShareRangeVO)grdMaster.SelectedItem).ID;
            }
            BizAction.ShareRangeDetails.Status = IsStatusChecked;
            BizAction.IsStatusChanged = true;
            BizAction.ShareRangeDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && ((clsAddDoctorShareRangeBizActionVO)arg.Result).SuccessStatus == 1)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("PALASH", "STATUS CHANGED SUCCESSFULLY.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.OK)
                        {
                            SetCommandButtonState("Load");
                            FetchData();
                            ClearData();
                        }
                    };
                    msgW1.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("PALASH", "Error occurred while Changing Status.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FetchData()
        {
            clsGetDoctorShareRangeListBizActionVO bizActionVO = new clsGetDoctorShareRangeListBizActionVO();
            bizActionVO.ShareRangeList = new List<clsDoctorShareRangeVO>();
            bizActionVO.PagingEnabled = true;
            bizActionVO.StartRowIndex = DataList.PageIndex * DataList.PageSize;
            bizActionVO.MaximumRows = DataList.PageSize;
            try
            {
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        clsGetDoctorShareRangeListBizActionVO result = args.Result as clsGetDoctorShareRangeListBizActionVO;
                        DataList.Clear();
                        if (result.ShareRangeList != null)
                        {
                            DataList.TotalItemCount = (int)((clsGetDoctorShareRangeListBizActionVO)args.Result).TotalRows;
                            foreach (clsDoctorShareRangeVO item in result.ShareRangeList)
                            {
                                DataList.Add(item);
                            }
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "ERROR OCCURRED WHILE PROCESSING.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }
                };
                client.ProcessAsync(bizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {

            }

        }

        private bool Validation()
        {
            if ((Convert.ToDecimal(txtUpperLimit.Text) < (Convert.ToDecimal(txtLowerLimit.Text))))
            {
                txtUpperLimit.SetValidation("Upper Limit Value Must Be Greater than Lower Limit Value.");
                txtUpperLimit.RaiseValidationError();
                txtUpperLimit.Focus();
                IsValid = false;
            }
            else
                txtUpperLimit.ClearValidationError();

            if ((Convert.ToDecimal(txtSharePercentage.Text) > 100))
            {
                txtSharePercentage.SetValidation("Please, Enter Share % Less Than Or Equal to 100.");
                txtSharePercentage.RaiseValidationError();
                txtSharePercentage.Focus();
                IsValid = false;
            }
            else
                txtSharePercentage.ClearValidationError();
            return IsValid;
        }

        private void ClearData()
        {
            IsValid = true;
            txtLowerLimit.Text = String.Empty;
            txtUpperLimit.Text = String.Empty;
            txtShareAmount.Text = String.Empty;
            txtSharePercentage.Text = String.Empty;
        }
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        private void txtNumber_TextChanged(object sender, TextChangedEventArgs e)
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

        private void txtNumber_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }
        #endregion
    }
}
