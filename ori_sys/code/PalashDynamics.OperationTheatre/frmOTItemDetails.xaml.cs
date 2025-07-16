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
using PalashDynamic.Localization;
using PalashDynamics.UserControls;
using CIMS;
using PalashDynamics.ValueObjects.Administration.OTConfiguration;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Inventory;
using System.Collections.ObjectModel;
using EMR;

namespace PalashDynamics.OperationTheatre
{
    public partial class frmOTItemDetails : UserControl
    {
        #region Variable Declaration
        LocalizationManager localizationManager;
        ObservableCollection<clsProcedureItemDetailsVO> ItemList { get; set; }
        //List<clsProcedureItemDetailsVO> ItemList = new List<clsProcedureItemDetailsVO>();
        //List<MasterListItem> ProcedureList = new List<MasterListItem>();
        //List<clsOTDetailsStaffDetailsVO> StaffList = new List<clsOTDetailsStaffDetailsVO>();
        public bool lIsEmergency { get; set; }
        public long lScheduleID = 0;
        public long lPatientID = 0;
        public long lODetailsIDView = 0;
        public LocalizationManager ObjLocalizationManager = null;
        WaitIndicator ObjWiaitIndicator = null;
        public string msgText = string.Empty;
        #endregion

        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion

        #region Constructor and Loaded
        public frmOTItemDetails()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(frmOTItemDetails_Loaded);
            ObjLocalizationManager = ((IApplicationConfiguration)App.Current).LocalizedManager;
        }

        void frmOTItemDetails_Loaded(object sender, RoutedEventArgs e)
        {
            FillDetailsOfProcedureSchedule(lScheduleID);
        }
        #endregion

        public void FillDetailsOfProcedureSchedule(long ScheduleID)
        {
            try
            {
                if (ObjWiaitIndicator == null)
                    ObjWiaitIndicator = new WaitIndicator();
                if (ObjWiaitIndicator.Visibility == Visibility.Collapsed)
                {
                    ObjWiaitIndicator.Show();
                }
                else
                {
                    ObjWiaitIndicator.Close();
                    ObjWiaitIndicator.Show();
                }
                clsGetProcScheduleDetailsByProcScheduleIDBizActionVO BizAction = new clsGetProcScheduleDetailsByProcScheduleIDBizActionVO();
                BizAction.PatientProcList = new List<clsPatientProcedureVO>();
                BizAction.DocScheduleDetails = new List<clsPatientProcDocScheduleDetailsVO>();
                BizAction.StaffDetailList = new List<clsPatientProcStaffDetailsVO>();
                BizAction.OTScheduleList = new List<clsPatientProcedureScheduleVO>();
                BizAction.CheckList = new List<clsPatientProcedureChecklistDetailsVO>();
                BizAction.ItemList = new List<clsProcedureItemDetailsVO>();
                BizAction.ScheduleID = ScheduleID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        dgItemDetailsList.ItemsSource = null;
                        dgItemDetailsList.ItemsSource = null;
                        ItemList = new ObservableCollection<clsProcedureItemDetailsVO>();
                        clsGetProcScheduleDetailsByProcScheduleIDBizActionVO DetailsVO = new clsGetProcScheduleDetailsByProcScheduleIDBizActionVO();
                        DetailsVO = (clsGetProcScheduleDetailsByProcScheduleIDBizActionVO)arg.Result;
                        if (DetailsVO.ItemList != null && DetailsVO.ItemList.Count > 0)
                        {
                            foreach (var item in DetailsVO.ItemList)
                            {
                                clsProcedureItemDetailsVO ItemObj = new clsProcedureItemDetailsVO();
                                ItemObj.ID = item.ID;
                                ItemObj.ItemCode = item.ItemCode;
                                ItemObj.ItemName = item.ItemName;
                                ItemObj.Quantity = item.Quantity;
                                ItemList.Add(ItemObj);
                            }
                            dgItemDetailsList.ItemsSource = null;
                            dgItemDetailsList.ItemsSource = ItemList;
                        }
                        if (this.lScheduleID > 0 && this.lPatientID > 0)
                            FillDetailTablesOfOTDetails(lODetailsIDView);
                    }
                    else
                    {
                        //if (ObjLocalizationManager != null)
                        //{
                        //    msgText = ObjLocalizationManager.GetValue("ErrorWhileSaveModify_Msg");
                        //}
                        //else
                        //{
                            msgText = "Error occured while processing.";
                        //}
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    }
                    ObjWiaitIndicator.Close();

                };

                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void FillDetailTablesOfOTDetails(long OtDetailsID)
        {
            WaitIndicator indicator = new WaitIndicator();
            try
            {
                if (ObjWiaitIndicator == null)
                    ObjWiaitIndicator = new WaitIndicator();
                if (ObjWiaitIndicator.Visibility == Visibility.Collapsed)
                {
                    ObjWiaitIndicator.Show();
                }
                else
                {
                    ObjWiaitIndicator.Close();
                    ObjWiaitIndicator.Show();
                }
                clsGetItemDetailsByOTDetailsIDBizActionVO BizAction = new clsGetItemDetailsByOTDetailsIDBizActionVO();
                BizAction.ItemList1 = new List<clsProcedureItemDetailsVO>();               
                BizAction.PatientID = this.lPatientID;
                BizAction.ScheduleId = this.lScheduleID;
                BizAction.OTDetailsID = OtDetailsID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        clsGetItemDetailsByOTDetailsIDBizActionVO DetailsVO = new clsGetItemDetailsByOTDetailsIDBizActionVO();
                        DetailsVO = (clsGetItemDetailsByOTDetailsIDBizActionVO)arg.Result;
                        if (DetailsVO.ItemList1 != null && DetailsVO.ItemList1.Count > 0)
                        {
                            //ItemList = new ObservableCollection<clsProcedureItemDetailsVO>();
                            //foreach (var item in DetailsVO.ItemList1)
                            //{
                            //    //var ObjService = ItemList.SingleOrDefault(z => z.ItemID.Equals(item.ItemID));
                            //    //if (ObjService != null)
                            //    //    ItemList.Remove(ObjService);  
                            //    if (item.ItemID > 0)
                            //    {
                            //        var ObjService = (from i in ItemList
                            //                          where i.ItemID == item.ItemID
                            //                          select i).SingleOrDefault();
                            //        if (ObjService != null)
                            //        {
                            //            ItemList.Remove(ObjService);


                            //        }

                            //    }

                            //    //var objItem = from r in ItemList
                            //    //              where r.ItemCode == item.ItemCode
                            //    //              select new clsProcedureItemDetailsVO
                            //    //              {
                            //    //                  ItemCode=r.ItemCode,                                                  
                            //    //                  ItemName=r.ItemName,
                            //    //                  Quantity=r.Quantity
                            //    //              };
                            //    //if (objItem.ToList().Count == 0)
                            //    //{
                            //    //    ItemList.Add(item);
                            //    //}
                            //}
                            if (DetailsVO.ItemList1.Count > 0)
                            {
                                ItemList.Clear();
                            }
                            foreach (var item in DetailsVO.ItemList1)
                            {
                              //  if (!ItemList.Where(z => z.ItemID.Equals(item.ItemID)).Any())
                                    ItemList.Add(item);
                            }
                            dgItemDetailsList.ItemsSource = null;
                            dgItemDetailsList.ItemsSource = ItemList;
                            dgItemDetailsList.UpdateLayout();
                            cmdModify.IsEnabled = true;
                            cmdSave.IsEnabled = false;
                        }    
                    }
                    else
                    {
                        if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                        {
                            msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ErrorWhileSaveModify_Msg");
                        }
                        else
                        {
                            msgText = "Error occured while processing.";
                        }
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    }
                    ObjWiaitIndicator.Close();
                };

                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                indicator.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private bool ValidateControls()
        {
            bool result = true;
            if (lScheduleID == 0 || lScheduleID == null)
            {
                msgText="Please select schedule.";
                //msgText = ObjLocalizationManager.GetValue("ScheduleVlidation_Msg");
                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                result = false;
                return result;
            }
            if (ItemList.Count > 0)
            {
                foreach (var item in ItemList)
                {
                    if (item.Quantity == 0)
                    {

                        result = false;
                        // return result;
                        break;
                    }
                }
                if (result == false)
                {
                    msgText = "Please Enter Quantity.";
                    //msgText = ObjLocalizationManager.GetValue("ScheduleVlidation_Msg");
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    // return result;
                }
            }
            return result;
        }

        private void SaveItemDetails()
        {
            try
            {
                clsAddUpdateOtItemDetailsBizActionVO BizAction = new clsAddUpdateOtItemDetailsBizActionVO();
                BizAction.objOTDetails.ItemList1 = ItemList.ToList();

                frmOTDetails winOTDetails;
                winOTDetails = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmOTDetails;
                BizAction.objOTDetails.ID = this.lODetailsIDView;
                BizAction.objOTDetails.PatientID = lPatientID;
                BizAction.objOTDetails.ScheduleID = lScheduleID;
                BizAction.objOTDetails.IsEmergency = lIsEmergency;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsAddUpdateOtItemDetailsBizActionVO)arg.Result).SuccessStatus == 1)
                        {

                            //if (ObjLocalizationManager != null)
                            //{
                            //    msgText = ObjLocalizationManager.GetValue("RecordSaved_Msg");
                            //}
                            //else
                            //{
                                msgText = "Record saved successfully.";
                            //}
                           ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            TreeView tvOTDetails = winOTDetails.FindName("tvOTDetails") as TreeView;
                            (tvOTDetails.Items[4] as TreeViewItem).IsSelected = true;

                        }
                    }
                    else
                    {
                        //if (ObjLocalizationManager != null)
                        //{
                        //    msgText = ObjLocalizationManager.GetValue("ErrorWhileSaveModify_Msg");
                        //}
                        //else
                        //{
                            msgText = "Error occured while processing.";
                        //}
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

            }
            catch (Exception Ex)
            {
                throw;
            }
        }

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            bool SaveTest = true;
            SaveTest = ValidateControls();
            if (SaveTest == true)
            {
                SaveItemDetails();
            }
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            bool SaveTest = true;
            SaveTest = ValidateControls();
            if (SaveTest == true)
            {
                SaveItemDetails();
            }
        }

        private void cmdAddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                frmEMRMedDrugSelectionList WinDrugList = new frmEMRMedDrugSelectionList();
                WinDrugList.CurrentVisit = new ValueObjects.OutPatientDepartment.clsVisitVO();
                WinDrugList.Height = this.ActualHeight * 0.75;
                WinDrugList.Width = this.ActualWidth * 0.95;
                WinDrugList.OnAddButton_Click += new RoutedEventHandler(WinDrug_OnAddButton_Click);
                WinDrugList.Show();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        void WinDrug_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (((EMR.frmEMRMedDrugSelectionList)sender).DialogResult == true)
            {
                foreach (var item in (((EMR.frmEMRMedDrugSelectionList)sender).DrugList))
                {
                    if (ItemList.Count > 0)
                    {
                        var item1 = from r in ItemList
                                    where (r.ItemName.Trim() == item.Description.Trim() && r.ItemID == item.ID)
                                    select new clsItemMasterVO
                                    {
                                        ItemCode = r.ItemCode,
                                        ItemName = r.ItemName,
                                        Status = r.Status
                                    };
                        if (item1.ToList().Count == 0)
                        {
                            clsProcedureItemDetailsVO OBj = new clsProcedureItemDetailsVO();
                            OBj.ItemID = item.ID;
                            OBj.ItemName = item.Description;
                            ItemList.Add(OBj);
                            dgItemDetailsList.ItemsSource = null;
                            dgItemDetailsList.ItemsSource = ItemList;
                        }
                    }
                    else
                    {
                        clsProcedureItemDetailsVO OBj = new clsProcedureItemDetailsVO();
                        OBj.ItemID = item.ID;
                        OBj.ItemName = item.Description;
                        ItemList.Add(OBj);
                        dgItemDetailsList.ItemsSource = null;
                        dgItemDetailsList.ItemsSource = ItemList;
                    }
                }

            }
        }

        private void txtDoctorQuantity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.PlatformKeyCode == 109)
            {
                e.Handled = true;
            }
            else
                e.Handled = CIMS.Comman.HandleNumber(sender, e);
        }

        private void txtDoctorQuantity_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)e.OriginalSource).Text))
            {
                if (!((TextBox)e.OriginalSource).Text.IsItSpecialCharAndMinus())
                {
                    ((TextBox)e.OriginalSource).Text = string.Empty;
                    ((TextBox)e.OriginalSource).BorderBrush = new SolidColorBrush(Colors.Red);

                    //if (ObjLocalizationManager != null)
                    //{
                    //    msgText = ObjLocalizationManager.GetValue("SpecialCharValidation_Msg");
                    //}
                    //else
                    //{
                        msgText = "Special characters are not allowed.";

                    //}
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                }
                else
                {
                    ((TextBox)e.OriginalSource).BorderBrush = new SolidColorBrush(Colors.Gray);
                }
            }
        }

        private void cmdDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgItemDetailsList.SelectedItem != null)
                {
                    msgText = "Are you sure you want to delete record ?";
                    //msgText = ObjLocalizationManager.GetValue("DeleteValidation_Msg");
                    int index = dgItemDetailsList.SelectedIndex;

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            ItemList.RemoveAt(dgItemDetailsList.SelectedIndex);
                            if (ItemList.Count == 0)
                            {
                                dgItemDetailsList.ItemsSource = null;
                            }
                            else
                            {
                                dgItemDetailsList.ItemsSource = null;
                                dgItemDetailsList.ItemsSource = ItemList;
                                dgItemDetailsList.UpdateLayout();
                            }
                        }
                    };
                    msgWD.Show();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
