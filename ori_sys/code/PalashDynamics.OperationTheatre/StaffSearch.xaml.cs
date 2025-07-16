using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.ValueObjects.Administration.OTConfiguration;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using System.Text;


namespace PalashDynamics.OperationTheatre
{
    public partial class StaffSearch : ChildWindow
    {
        #region Variable Declaration
        string msgText;
        public event RoutedEventHandler OnAddButton_Click;
        public List<clsOTDetailsStaffDetailsVO> OTDetailsStaffList = new List<clsOTDetailsStaffDetailsVO>();
        public List<clsOTDetailsStaffDetailsVO> AddedStaffList = new List<clsOTDetailsStaffDetailsVO>();
        #endregion

        #region Constructor and Loaded
        public StaffSearch()
        {
            InitializeComponent();
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FillDesignations();

        }
        #endregion

        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        #region Button Click Events
        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (OnAddButton_Click != null)
                {
                    this.DialogResult = true;
                    OnAddButton_Click(this, new RoutedEventArgs());

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (CmbDesignation.SelectedItem != null)
            {
                if (((MasterListItem)CmbDesignation.SelectedItem).ID > 0)
                    FetchStaff(((MasterListItem)CmbDesignation.SelectedItem).ID);
                else
                {
                    //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                    //{
                    //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("SelectDesignationValidation_Msg");
                    //}
                    //else
                    //{
                        msgText = "Please select designation.";
                    //}
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                }
            }
        }
        #endregion

        #region Fetched Methods
        private void FetchStaff(long designationID)
        {
            clsStaffByDesignationIDBizActionVO BizAction = new clsStaffByDesignationIDBizActionVO();
            BizAction.DesignationID = designationID;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    OTDetailsStaffList = new List<clsOTDetailsStaffDetailsVO>();
                    if (((clsStaffByDesignationIDBizActionVO)arg.Result).StaffDetails != null)
                    {
                        foreach (var item in ((clsStaffByDesignationIDBizActionVO)arg.Result).StaffDetails)
                        {
                            clsOTDetailsStaffDetailsVO staffObj = new clsOTDetailsStaffDetailsVO();
                            staffObj.StaffID = item.ID;
                            staffObj.StaffDesc = item.Description;
                            staffObj.Status = false;
                            var obj = OTDetailsStaffList.FirstOrDefault(q => q.StaffID == ((MasterListItem)item).ID);
                            if (obj != null)
                                continue;
                            else
                                OTDetailsStaffList.Add(staffObj);

                        }
                        dgStaffList.ItemsSource = null;
                        dgStaffList.ItemsSource = OTDetailsStaffList;
                    }

                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void FillDesignations()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_DesignationMaster;
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

                    CmbDesignation.ItemsSource = null;
                    CmbDesignation.ItemsSource = objList;
                    CmbDesignation.SelectedItem = objList[0];
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }
        #endregion

        #region Checked-UnChecked Events
        private void chkStatus_Checked(object sender, RoutedEventArgs e)
        {
            //if (dgStaffList.SelectedItem != null)
            //    ((clsOTDetailsStaffDetailsVO)dgStaffList.SelectedItem).Status = true;
        }

        private void chkStatus_Unchecked(object sender, RoutedEventArgs e)
        {
            //if (dgStaffList.SelectedItem != null)
            //    ((clsOTDetailsStaffDetailsVO)dgStaffList.SelectedItem).Status = false;
        }
        #endregion

        private void CmbDesignation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbDesignation.SelectedItem != null)
            {
                if (((MasterListItem)CmbDesignation.SelectedItem).ID > 0)
                    FetchStaff(((MasterListItem)CmbDesignation.SelectedItem).ID);
                else
                {
                    //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                    //{
                    //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("SelectDesignationValidation_Msg");
                    //}
                    //else
                    //{
                        msgText = "Please select designation.";
                    //}
                    //ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                }
            }
        }

        private void chkStatus_Click(object sender, RoutedEventArgs e)
        {
            bool IsValid = true;
            if (dgStaffList.SelectedItem != null)
            {
                try
                {
                    if (AddedStaffList == null)
                        AddedStaffList = new List<clsOTDetailsStaffDetailsVO>();

                    CheckBox chk = (CheckBox)sender;
                    StringBuilder strError = new StringBuilder();
                    if (chk.IsChecked == true)
                    {
                        if (AddedStaffList.Count > 0)
                        {
                            var item = from r in AddedStaffList
                                       where r.StaffID== ((clsOTDetailsStaffDetailsVO)dgStaffList.SelectedItem).StaffID
                                       select new clsOTDetailsStaffDetailsVO
                                       {
                                           Status = r.Status,
                                           StaffID = r.StaffID
                                       };
                            if (item.ToList().Count > 0)
                            {
                                if (strError.ToString().Length > 0)
                                    strError.Append(",");
                                strError.Append(((MasterListItem)dgStaffList.SelectedItem).Code);
                                if (!string.IsNullOrEmpty(strError.ToString()))
                                {
                                    string strMsg = "Staff already Selected : " + strError.ToString();

                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                               new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.Show();
                                    ((clsOTDetailsStaffDetailsVO)dgStaffList.SelectedItem).Status= false;

                                    IsValid = false;
                                }
                            }
                            else
                            {
                                AddedStaffList.Add((clsOTDetailsStaffDetailsVO)dgStaffList.SelectedItem);
                            }
                        }
                        else
                        {
                            AddedStaffList.Add((clsOTDetailsStaffDetailsVO)dgStaffList.SelectedItem);
                        }
                    }
                    else
                        AddedStaffList.Remove((clsOTDetailsStaffDetailsVO)dgStaffList.SelectedItem);

                }
                catch (Exception) { }
            }
        }



    }
}

